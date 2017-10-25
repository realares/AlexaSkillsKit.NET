/*
 * Copyright 2017 realares
 * Original Copyright 2015 Stefan Negritoiu (FreeBusy). 
 * See LICENSE file for more information.
 */

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Ra.AlexaSkillsKit.Authentication;
using Ra.AlexaSkillsKit.Json;
using System.Threading.Tasks;
using Ra.AlexaSkillsKit.Ressources;

namespace Ra.AlexaSkillsKit
{
    public abstract class SpeechletAppAsync : _SpeechletAppBase
    {
        /// <summary>
        /// Processes Alexa request AND validates request signature
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <returns></returns>
        public virtual async Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage httpRequest)
        {
            DateTime now = DateTime.UtcNow; // reference time for this request

            
            RequestHeader requestHeader = new RequestHeader();
            requestHeader.Read(httpRequest);
            await OnRequestIncomeAsync(requestHeader);

#if HasBouncyCastle
            var cerValidator = new BouncyCastleCertValidator();
#else
            var cerValidator = new DotNetCertValidator();
#endif
            var validationResult = await cerValidator.VerifyAsync(requestHeader);

            if (!await OnRequestValidationAsync(requestHeader, validationResult))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    ReasonPhrase = validationResult.ToString()
                };
            }

            try
            {
                RequestEnvelope = SpeechletRequestEnvelope.FromJson(requestHeader.RequestAsString);

                if (!SpeechletRequestTimestampVerifier.VerifyRequestTimestamp(RequestEnvelope, now))
                    validationResult = validationResult | SpeechletRequestValidationResult.InvalidTimestamp;
            }
            catch (Exception e1)
            {
                await OnParsingErrorAsync(e1);
                validationResult = SpeechletRequestValidationResult.InvalidJson;
            }


            try
            {
                string alexaResponsejson = await DoProcessRequestAsync(RequestEnvelope);
                if (alexaResponsejson == null)
                {
                    await OnResponseOutgoingAsync("->alexaResponsejson == null");
                    return null;
                }

                await OnResponseOutgoingAsync(alexaResponsejson);

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(alexaResponsejson, Encoding.UTF8, "application/json")
                }; ;

            }
            catch (NotImplementedException)
            {
                return null;
            }
            catch (Exception e2)
            {
                await OnParsingErrorAsync(e2);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestEnvelope"></param>
        /// <returns></returns>
        public async Task<string> DoProcessRequestAsync(SpeechletRequestEnvelope requestEnvelope)
        {
         
            Session session = requestEnvelope.Session;
            Context context = requestEnvelope.Context;
            SpeechletResponse response = null;

            switch (requestEnvelope.Request)
            {
                case LaunchRequest request:
                    {
                        if (requestEnvelope.Session.IsNew)
                        {
                            await OnSessionStartedAsync(requestEnvelope);
                        }
                        response = await OnLaunchAsync(request, session, context);
                    }
                    break;

                case AudioPlayerRequest request:
                    {
                        response = await OnAudioIntentAsync(request, session, context);
                    }
                    break;

                // process intent request
                case IntentRequest request:
                    {
                        // Do session management prior to calling OnSessionStarted and OnIntentAsync 
                        // to allow dev to change session values if behavior is not desired
                        DoSessionManagement(request, session);

                        if (requestEnvelope.Session.IsNew)
                        {
                            await OnSessionStartedAsync(requestEnvelope);
                        }
                        response = await OnIntentAsync(request, session, context);
                    }
                    break;

                // process session ended request
                case SessionEndedRequest request:
                    {
                        await OnSessionEndedAsync(request, session, context);
                    }
                    break;

            }

            if (response == null)
                return new SpeechletResponseEnvelope
                {
                    Version = requestEnvelope.Version,
                    Response = new SpeechletResponse() { ShouldEndSession = null }
                }.ToJson();

            var responseEnvelope = new SpeechletResponseEnvelope
            {
                Version = requestEnvelope.Version,
                Response = response,
                SessionAttributes = requestEnvelope.Session?.Attributes ?? new Dictionary<string, string>()
            };
            return responseEnvelope.ToJson();
        }

        /// <summary>
        /// Opportunity to set policy for handling requests with invalid signatures and/or timestamps
        /// </summary>
        /// <returns>true if request processing should continue, otherwise false</returns>
        public virtual Task<bool> OnRequestValidationAsync(RequestHeader header, SpeechletRequestValidationResult result)
        {

            return Task.Run(()=> result == SpeechletRequestValidationResult.OK);
        }

        public virtual Task OnRequestIncomeAsync(RequestHeader header) { return Task.FromResult(0); }

        public virtual Task OnResponseOutgoingAsync(string msg) { return Task.FromResult(0); }

        public virtual Task OnParsingErrorAsync(Exception exception) { return Task.FromResult(0); }

        public virtual Task OnSessionStartedAsync(SpeechletRequestEnvelope requestEnvelope) { return Task.FromResult(0); }

        public virtual Task OnSessionEndedAsync(SessionEndedRequest sessionEndedRequest, Session session, Context context) { return Task.FromResult(0); }


        public abstract Task<SpeechletResponse> OnIntentAsync(IntentRequest intentRequest, Session session, Context context);
        public abstract Task<SpeechletResponse> OnLaunchAsync(LaunchRequest launchRequest, Session session, Context context);
        public abstract Task<SpeechletResponse> OnAudioIntentAsync(AudioPlayerRequest audioRequest, Session session, Context context);
    }

    
}
