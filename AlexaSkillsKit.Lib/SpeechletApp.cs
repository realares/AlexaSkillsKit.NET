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
using AlexaSkillsKit.Authentication;
using AlexaSkillsKit.Json;
using NLog;
using System.Threading.Tasks;
using AlexaSkillsKit.Helper;
using AlexaSkillsKit.UI.Speech;
using AlexaSkillsKit.Directives;
using AlexaSkillsKit.UI.Cards;
using AlexaSkillsKit.UI;

namespace AlexaSkillsKit
{
    public abstract class SpeechletApp : ISpeechlet
    {
        public static Logger log = LogManager.GetCurrentClassLogger();

        public SpeechletRequestEnvelope RequestEnvelope { get; private set; } = null;

        /// <summary>
        /// Processes Alexa request AND validates request signature
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <returns></returns>
        public virtual HttpResponseMessage GetResponse(HttpRequestMessage httpRequest)
        {
            SpeechletRequestValidationResult validationResult = SpeechletRequestValidationResult.OK;
            DateTime now = DateTime.UtcNow; // reference time for this request

            string chainUrl = null;

            if (!httpRequest.Headers.Contains(Sdk.SIGNATURE_CERT_URL_REQUEST_HEADER) ||
                String.IsNullOrEmpty(chainUrl = httpRequest.Headers.GetValues(Sdk.SIGNATURE_CERT_URL_REQUEST_HEADER).First()))
            {

                validationResult = validationResult | SpeechletRequestValidationResult.NoCertHeader;
            }

            string signature = null;
            if (!httpRequest.Headers.Contains(Sdk.SIGNATURE_REQUEST_HEADER) ||
                String.IsNullOrEmpty(signature = httpRequest.Headers.GetValues(Sdk.SIGNATURE_REQUEST_HEADER).First()))
            {
                validationResult = validationResult | SpeechletRequestValidationResult.NoSignatureHeader;
            }


            var alexaBytes = AsyncHelpers.RunSync<byte[]>(() => httpRequest.Content.ReadAsByteArrayAsync());

            // attempt to verify signature only if we were able to locate certificate and signature headers
            if (validationResult == SpeechletRequestValidationResult.OK) {
                if (!SpeechletRequestSignatureVerifier.VerifyRequestSignature(alexaBytes, signature, chainUrl)) 
                {
                    validationResult = validationResult | SpeechletRequestValidationResult.InvalidSignature;
                }
            }
            
            try
            {
                var alexaContent = UTF8Encoding.UTF8.GetString(alexaBytes);
                log.Debug("Request income: {0}", alexaContent);
                RequestEnvelope = SpeechletRequestEnvelope.FromJson(alexaContent);
            }
            catch (Newtonsoft.Json.JsonReaderException e1)
            {
                log.Error(e1, "Parsing Request Fail:JsonReaderException");
                validationResult = validationResult | SpeechletRequestValidationResult.InvalidJson;
            }
            catch (InvalidCastException e2)
            {
                log.Error(e2, "Parsing Request Fail:InvalidCastException");
                validationResult = validationResult | SpeechletRequestValidationResult.InvalidJson;
            }

            // attempt to verify timestamp only if we were able to parse request body
            if (RequestEnvelope != null) 
            {
                if (!SpeechletRequestTimestampVerifier.VerifyRequestTimestamp(RequestEnvelope, now)) {
                    validationResult = validationResult | SpeechletRequestValidationResult.InvalidTimestamp;
                }
            }

            if (RequestEnvelope == null || !OnRequestValidation(validationResult, now, RequestEnvelope))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    ReasonPhrase = validationResult.ToString()
                };
            }

            string alexaResponsejson = DoProcessRequest(RequestEnvelope);

            HttpResponseMessage httpResponse;
            if (alexaResponsejson == null)
            {
                httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            else
            {
                httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
                httpResponse.Content = new StringContent(alexaResponsejson, Encoding.UTF8, "application/json");
                log.Debug("Response: {0}", alexaResponsejson);
            }

            return httpResponse;
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
        private void DoSessionManagement(IntentRequest request, Session session)
        {
            if (session.IsNew) {
                session.Attributes[Session.INTENT_SEQUENCE] = request.Intent.Name;
            }
            else
            {
                // if the session was started as a result of a launch request 
                // a first intent isn't yet set, so set it to the current intent
                if (!session.Attributes.ContainsKey(Session.INTENT_SEQUENCE))
                {
                    session.Attributes[Session.INTENT_SEQUENCE] = request.Intent.Name;
                }
                else
                {
                    session.Attributes[Session.INTENT_SEQUENCE] += Session.SEPARATOR + request.Intent.Name;
                }
            }

            // Auto-session management: copy all slot values from current intent into session
            foreach (var slot in request.Intent.Slots.Values)
            {
                session.Attributes[slot.Name] = slot.Value;
            }
        }


        /// <summary>
        /// Opportunity to set policy for handling requests with invalid signatures and/or timestamps
        /// </summary>
        /// <returns>true if request processing should continue, otherwise false</returns>
        public virtual bool OnRequestValidation(SpeechletRequestValidationResult result, DateTime referenceTimeUtc, SpeechletRequestEnvelope requestEnvelope) {

            return result == SpeechletRequestValidationResult.OK;
        }


        public abstract SpeechletResponse OnIntent(IntentRequest intentRequest, Session session, Context context);
        public abstract SpeechletResponse OnLaunch(LaunchRequest launchRequest, Session session, Context context);
        public abstract SpeechletResponse OnAudioIntent(AudioPlayerRequest audioRequest, Context context);
        public abstract void OnSessionStarted(SessionStartedRequest sessionStartedRequest, Session session);
        public abstract void OnSessionEnded(SessionEndedRequest sessionEndedRequest, Session session);


        #region Responses

        public SpeechletResponse DialogDelegate()
        {

            var intentRequest = (RequestEnvelope.Request as IntentRequest);
            if (intentRequest == null) throw new SpeechletException("IntentRequest required");

            var response = new SpeechletResponse()
            {
                Directives = new List<Directive>()
                 {
                     new DialogDelegateDirective()
                     {
                          ConfirmationStatus = intentRequest.ConfirmationStatus,
                          UpdatedIntent = intentRequest.Intent
                     }
                 }
            };

            return response;
        }


        
        public SpeechletResponse AudioPlayer_Play(AudioStream stream)
        {

            var response = new SpeechletResponse()
            {
                Directives = new List<Directive>()
                 {
                     new AudioPlayerPlayDirective()
                     {
                           PlayBehavior = AudioPlayerPlayDirective.PlayBehaviorEnum.REPLACE_ALL,
                            AudioItem = stream
                     }
                 },
                ShouldEndSession = true
            };

            return response;
        }
        public SpeechletResponse AudioPlayer_Stop()
        {

            var response = new SpeechletResponse()
            {
                Directives = new List<Directive>()
                 {
                     new AudioPlayerStopDirective()
                 }
                ,
                ShouldEndSession = true
            };

            return response;
        }


        public SpeechletResponse DialogElicitSlot(Slot slotToElicit, OutputSpeech outputSpeech = null)
        {
            var intentRequest = (RequestEnvelope.Request as IntentRequest);
            if (intentRequest == null) throw new SpeechletException("IntentRequest required");
            
            var response = new SpeechletResponse()
            {
                OutputSpeech = outputSpeech,
                Directives = new List<Directive>()
                 {
                     new DialogElicitSlotDirective()
                     {
                          SlotToElicit = slotToElicit.Name,
                          UpdatedIntent = intentRequest.Intent
                     }
                 }
            };

            return response;
        }

        public SpeechletResponse DialogConfirmSlot(Slot slotToElicit, OutputSpeech outputSpeech)
        {
            var intentRequest = (RequestEnvelope.Request as IntentRequest);
            if (intentRequest == null) throw new SpeechletException("IntentRequest required");

            if (slotToElicit == null) throw new ArgumentNullException(nameof(slotToElicit));
            if (outputSpeech == null) throw new ArgumentNullException(nameof(outputSpeech));
            
            var response = new SpeechletResponse()
            {
                OutputSpeech = outputSpeech,
                Directives = new List<Directive>()
                 {
                     new DialogConfirmSlotDirective()
                     {
                          SlotToConfirm = slotToElicit.Name,
                          UpdatedIntent = intentRequest.Intent
                     }
                 }
            };

            return response;
        }

        public SpeechletResponse Say(string speechOutput, bool shouldEndSession = true)
        {
            return Say(new PlainTextOutputSpeech() { Text = speechOutput }, shouldEndSession);
        }
        public SpeechletResponse Say(PlainTextOutputSpeech speechOutput, bool shouldEndSession = true)
        {
            return new SpeechletResponse()
            {
                OutputSpeech = speechOutput,
                ShouldEndSession = shouldEndSession
            };
        }
        public SpeechletResponse Say(SsmlOutputSpeech speechOutput, bool shouldEndSession = true)
        {
            return new SpeechletResponse()
            {
                OutputSpeech = speechOutput,
                ShouldEndSession = shouldEndSession
            };
        }

        public static SpeechletResponse SayWithCard(string speechOutput, string cardTitle, string cardContent, bool shouldEndSession = true)
        {
            return new SpeechletResponse()
            {
                OutputSpeech = new PlainTextOutputSpeech() { Text = speechOutput },
                Card = new SimpleCard() { Content = cardContent, Title = cardTitle },
                ShouldEndSession = shouldEndSession
            };
        }
        public static SpeechletResponse SayWithCard(string speechOutput, string cardTitle, string cardContent, Image imageObj)
        {
            return new SpeechletResponse()
            {
                OutputSpeech = new PlainTextOutputSpeech() { Text = speechOutput },
                Card = new StandardCard() { Text = cardContent, Title = cardTitle, Image = imageObj },
                ShouldEndSession = true
            };
        }
        public static SpeechletResponse SayWithLinkAccountCard(string speechOutput)
        {
            return new SpeechletResponse()
            {
                OutputSpeech = new PlainTextOutputSpeech() { Text = speechOutput },
                Card = new LinkAccountCard() { },
                ShouldEndSession = true
            };
        }

        public static SpeechletResponse Error_NoIntentFound()
        {
            return new SpeechletResponse()
            {
                OutputSpeech = new PlainTextOutputSpeech()
                {
                    Text = AlexaSkillsKit.Properties.Resource.NO_INTENT_FOUND,
                },
                ShouldEndSession = true
            };
        }
        public static SpeechletResponse Error_NoLaunchFunction()
        {
            return new SpeechletResponse()
            {
                OutputSpeech = new PlainTextOutputSpeech()
                {
                    Text = AlexaSkillsKit.Properties.Resource.NO_LAUNCH_FUNCTION,
                },
                ShouldEndSession = true
            };
        }
        public static SpeechletResponse Error_GenericError()
        {
            return new SpeechletResponse()
            {
                OutputSpeech = new PlainTextOutputSpeech()
                {
                    Text = AlexaSkillsKit.Properties.Resource.GENERIC_ERROR,
                },
                ShouldEndSession = true
            };
        }

        #endregion

    }
}
    