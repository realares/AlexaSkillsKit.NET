/*
 * Copyright 2017 realares
 * Original Copyright 2015 Stefan Negritoiu (FreeBusy). 
 * See LICENSE file for more information.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;
using Ra.AlexaSkillsKit.Authentication;
using Ra.AlexaSkillsKit.Json;
using System.Threading.Tasks;
using Ra.AlexaSkillsKit.Helper;
using Ra.AlexaSkillsKit.UI.Speech;
using Ra.AlexaSkillsKit.Directives;
using Ra.AlexaSkillsKit.UI.Cards;
using Ra.AlexaSkillsKit.UI;
using Ra.AlexaSkillsKit.Ressources;

namespace Ra.AlexaSkillsKit
{

    public abstract class SpeechletApp : _SpeechletAppBase
    {
        
        /// <summary>
        /// Processes Alexa request AND validates request signature
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <returns></returns>
        public virtual HttpResponseMessage GetResponse(HttpRequestMessage httpRequest)
        {
            DateTime now = DateTime.UtcNow; // reference time for this request

            RequestHeader requestHeader = new RequestHeader();
            requestHeader.Read(httpRequest);
            OnRequestIncome(requestHeader);

#if HasBouncyCastle
            var cerValidator = new BouncyCastleCertValidator();
#else
            var cerValidator = new DotNetCertValidator();
#endif
            var validationResult = cerValidator.Verify(requestHeader);

            if (!OnRequestValidation(requestHeader, validationResult))
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
                OnParsingError(e1);
                validationResult = SpeechletRequestValidationResult.InvalidJson;
            }


            try
            {
                string alexaResponsejson = DoProcessRequest(RequestEnvelope);
                if (alexaResponsejson == null)
                {
                    OnResponseOutgoing("->alexaResponsejson == null");
                   
                    return null;
                }                    

                OnResponseOutgoing(alexaResponsejson);
                
                return  new HttpResponseMessage(HttpStatusCode.OK)
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
                OnParsingError(e2);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

           
        }
     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestEnvelope"></param>
        /// <returns></returns>
        private string DoProcessRequest(SpeechletRequestEnvelope requestEnvelope)
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
                            OnSessionStarted(requestEnvelope);
                        }
                        response = OnLaunch(request, session, context);
                    }
                    break;

                case AudioPlayerRequest request:
                    {
                        response = OnAudioIntent(request, session, context);
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
                            OnSessionStarted(requestEnvelope);
                        }
                        response = OnIntent(request, session, context);
                    }
                    break;

                // process session ended request
                case SessionEndedRequest request:  
                    {
                        OnSessionEnded(request, session, context);
                    }
                    break;

            }

            if (response == null)
            {
                return new SpeechletResponseEnvelope
                {
                    Version = requestEnvelope.Version,
                    Response = new SpeechletResponse() { ShouldEndSession = null}             
                }.ToJson();
            }
                
            
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
        public virtual bool OnRequestValidation(RequestHeader header, SpeechletRequestValidationResult result)
        {

            return result == SpeechletRequestValidationResult.OK;
        }

        public virtual void OnRequestIncome(RequestHeader header) { }

        public virtual void OnResponseOutgoing(string msg) { }

        public virtual void OnParsingError(Exception exception) { }

        public virtual void OnSessionStarted(SpeechletRequestEnvelope requestEnvelope) { }

        public virtual void OnSessionEnded(SessionEndedRequest sessionEndedRequest, Session session, Context context) { }

        public abstract SpeechletResponse OnIntent(IntentRequest intentRequest, Session session, Context context);
        public abstract SpeechletResponse OnLaunch(LaunchRequest launchRequest, Session session, Context context);
        public abstract SpeechletResponse OnAudioIntent(AudioPlayerRequest audioRequest, Session session, Context context);

    }

    
}
