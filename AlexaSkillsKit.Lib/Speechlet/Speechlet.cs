//  Copyright 2015 Stefan Negritoiu (FreeBusy). See LICENSE file for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;
using AlexaSkillsKit.Authentication;
using AlexaSkillsKit.Json;
using AlexaSkillsKit.UI;
using AlexaSkillsKit.UI.Cards;
using System.Runtime.InteropServices;

namespace AlexaSkillsKit.Speechlet
{
    public abstract class Speechlet : ISpeechlet
    {
        /// <summary>
        /// Processes Alexa request AND validates request signature
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <returns></returns>
        public virtual HttpResponseMessage GetResponse(HttpRequestMessage httpRequest) {
            SpeechletRequestValidationResult validationResult = SpeechletRequestValidationResult.OK;
            DateTime now = DateTime.UtcNow; // reference time for this request

            string chainUrl = null;
#if !DEBUG
            if (!httpRequest.Headers.Contains(Sdk.SIGNATURE_CERT_URL_REQUEST_HEADER) ||
                String.IsNullOrEmpty(chainUrl = httpRequest.Headers.GetValues(Sdk.SIGNATURE_CERT_URL_REQUEST_HEADER).First())) {

                validationResult = validationResult | SpeechletRequestValidationResult.NoCertHeader;
            }

            string signature = null;
            if (!httpRequest.Headers.Contains(Sdk.SIGNATURE_REQUEST_HEADER) ||
                String.IsNullOrEmpty(signature = httpRequest.Headers.GetValues(Sdk.SIGNATURE_REQUEST_HEADER).First())) {
                validationResult = validationResult | SpeechletRequestValidationResult.NoSignatureHeader;
            }

#endif
            var alexaBytes = AsyncHelpers.RunSync<byte[]>(() => httpRequest.Content.ReadAsByteArrayAsync());
            //Debug.WriteLine(httpRequest.ToLogString());
            //RequestLog(httpRequest.ToLogString());

#if !DEBUG
            // attempt to verify signature only if we were able to locate certificate and signature headers
            if (validationResult == SpeechletRequestValidationResult.OK) {
                if (!SpeechletRequestSignatureVerifier.VerifyRequestSignature(alexaBytes, signature, chainUrl)) 
                {
                    validationResult = validationResult | SpeechletRequestValidationResult.InvalidSignature;
                }
            }
#endif

            SpeechletRequestEnvelope alexaRequest = null;
            try
            {
                var alexaContent = UTF8Encoding.UTF8.GetString(alexaBytes);
                alexaContent = alexaContent.Trim();
                System.IO.File.WriteAllText($@"c:\fku\logs\{DateTime.UtcNow.Ticks.ToString()}_h2.txt", alexaContent);
                //RequestLog(alexaContent);
                alexaRequest = SpeechletRequestEnvelope.FromJson(alexaContent);
            }
            catch (Newtonsoft.Json.JsonReaderException e1)
            {
                System.IO.File.WriteAllText($@"c:\fku\logs\{DateTime.UtcNow.Ticks.ToString()}_e1.txt", e1.Message);
                validationResult = validationResult | SpeechletRequestValidationResult.InvalidJson;
            }
            catch (InvalidCastException e2)
            {
                System.IO.File.WriteAllText($@"c:\fku\logs\{DateTime.UtcNow.Ticks.ToString()}_e2.txt", e2.Message + e2.StackTrace);
                validationResult = validationResult | SpeechletRequestValidationResult.InvalidJson;
            }

#if !DEBUG
            // attempt to verify timestamp only if we were able to parse request body
            if (alexaRequest != null) 
            {
                if (!SpeechletRequestTimestampVerifier.VerifyRequestTimestamp(alexaRequest, now)) {
                    validationResult = validationResult | SpeechletRequestValidationResult.InvalidTimestamp;
                }
            }
#endif

            if (alexaRequest == null || !OnRequestValidation(validationResult, now, alexaRequest))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    ReasonPhrase = validationResult.ToString()
                };
            }

            string alexaResponsejson = DoProcessRequest(alexaRequest);

            System.IO.File.WriteAllText($@"c:\fku\logs\{DateTime.UtcNow.Ticks.ToString()}_r1.txt", alexaResponsejson);

            HttpResponseMessage httpResponse;
            if (alexaResponsejson == null)
            {
                httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            else
            {
                httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
                httpResponse.Content = new StringContent(alexaResponsejson, Encoding.UTF8, "application/json");
                Debug.WriteLine(httpResponse.ToLogString());
            }

            return httpResponse;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestContent"></param>
        /// <returns></returns>
        public virtual string ProcessRequest(string requestContent)
        {
            var requestEnvelope = SpeechletRequestEnvelope.FromJson(requestContent);
            return DoProcessRequest(requestEnvelope);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestJson"></param>
        /// <returns></returns>
        public virtual string ProcessRequest(JObject requestJson) {
            var requestEnvelope = SpeechletRequestEnvelope.FromJson(requestJson);
            return DoProcessRequest(requestEnvelope);
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
                            OnSessionStarted(new SessionStartedRequest(request.RequestId, request.Timestamp), session);
                        }
                        response = OnLaunch(request, session, context);
                    }
                    break;

                case AudioPlayerRequest request:
                    {
                        response = OnAudioIntent(request, context);
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
                            OnSessionStarted(new SessionStartedRequest(request.RequestId, request.Timestamp), session);
                        }
                        response = OnIntent(request, session, requestEnvelope.Context);
                    }
                    break;

                // process session ended request
                case SessionEndedRequest request:  
                    {
                        OnSessionEnded(request, session);
                    }
                    break;
            }

            
            var responseEnvelope = new SpeechletResponseEnvelope
            {
                Version = requestEnvelope.Version,
                Response = response,
                SessionAttributes = requestEnvelope.Session.Attributes ?? new Dictionary<string, string>()
            };
            return responseEnvelope.ToJson();
        }


        /// <summary>
        /// 
        /// </summary>
        private void DoSessionManagement(IntentRequest request, Session session) {
            if (session.IsNew) {
                session.Attributes[Session.INTENT_SEQUENCE] = request.Intent.Name;
            }
            else {
                // if the session was started as a result of a launch request 
                // a first intent isn't yet set, so set it to the current intent
                if (!session.Attributes.ContainsKey(Session.INTENT_SEQUENCE)) {
                    session.Attributes[Session.INTENT_SEQUENCE] = request.Intent.Name;
                }
                else {
                    session.Attributes[Session.INTENT_SEQUENCE] += Session.SEPARATOR + request.Intent.Name;
                }
            }

            // Auto-session management: copy all slot values from current intent into session
            foreach (var slot in request.Intent.Slots.Values) {
                session.Attributes[slot.Name] = slot.Value;
            }
        }


        /// <summary>
        /// Opportunity to set policy for handling requests with invalid signatures and/or timestamps
        /// </summary>
        /// <returns>true if request processing should continue, otherwise false</returns>
        public virtual bool OnRequestValidation(
            SpeechletRequestValidationResult result, DateTime referenceTimeUtc, SpeechletRequestEnvelope requestEnvelope) {

            return result == SpeechletRequestValidationResult.OK;
        }


        public abstract SpeechletResponse OnIntent(IntentRequest intentRequest, Session session, Context context);
        public abstract SpeechletResponse OnLaunch(LaunchRequest launchRequest, Session session, Context context);
        public abstract SpeechletResponse OnAudioIntent(AudioPlayerRequest audioRequest, Context context);
        public abstract void OnSessionStarted(SessionStartedRequest sessionStartedRequest, Session session);
        public abstract void OnSessionEnded(SessionEndedRequest sessionEndedRequest, Session session);

    }
}
    