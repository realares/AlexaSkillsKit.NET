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
    public abstract class SpeechletApp : ISpeechlet
    {
        
        
        public SpeechletRequestEnvelope RequestEnvelope { get; set; } = null;

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


            //var alexaBytes = AsyncHelpers.RunSync<byte[]>(() => httpRequest.Content.ReadAsByteArrayAsync());
            var alexaBytes = httpRequest.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();

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
                OnRequestIncome(alexaContent);
                RequestEnvelope = SpeechletRequestEnvelope.FromJson(alexaContent);
            }
            catch (Newtonsoft.Json.JsonReaderException e1)
            {
                OnParsingError(e1);
                validationResult = validationResult | SpeechletRequestValidationResult.InvalidJson;
            }
            catch (InvalidCastException e2)
            {
                OnParsingError(e2);
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

            try
            {
                string alexaResponsejson = DoProcessRequest(RequestEnvelope);

                HttpResponseMessage httpResponse;
                if (alexaResponsejson == null)
                {
                    return null;
                }

                httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(alexaResponsejson, Encoding.UTF8, "application/json")
                };
                OnResponseOutgoing(alexaResponsejson);
                return httpResponse;

            }
            catch (NotImplementedException)
            {
                return null;
            }
            catch (Exception)
            {
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
                return null;
            
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

        public virtual void OnRequestIncome(string msg) { }

        public virtual void OnResponseOutgoing(string msg) { }
    
        public virtual void OnParsingError(Exception exception) { }
        public virtual void OnSessionStarted(SpeechletRequestEnvelope requestEnvelope) { }


        public abstract SpeechletResponse OnIntent(IntentRequest intentRequest, Session session, Context context);
        public abstract SpeechletResponse OnLaunch(LaunchRequest launchRequest, Session session, Context context);
        public abstract SpeechletResponse OnAudioIntent(AudioPlayerRequest audioRequest, Session session, Context context);
        
        public abstract void OnSessionEnded(SessionEndedRequest sessionEndedRequest, Session session, Context context);


        #region Responses

        /// <summary>
        /// Create a SpeechletResponse to send Alexa a command to stream the audio file identified by the specified audioItem. 
        /// Use the playBehavior parameter to determine whether the stream begins playing immediately,
        /// or is added to the queue.
        /// </summary>
        /// <param name="stream">Representing the audio stream to play</param>
        /// <param name="playbehavior">Describes playback behavior.</param>
        /// <returns>The SpeechletResponse</returns>
        public SpeechletResponse AudioPlayer_Play(AudioStream stream, PlayBehaviorEnum playbehavior = PlayBehaviorEnum.REPLACE_ALL)
        {

            var response = new SpeechletResponse()
            {
                Directives = new List<Directive>()
                 {
                     new AudioPlayerPlayDirective()
                     {
                           PlayBehavior = playbehavior,
                           AudioItem = new AudioItem()
                           {
                               Stream =  stream
                           }
                     }
                 },
                ShouldEndSession = true
            };

            return response;
        }

        /// <summary>
        /// Create a SpeechletResponse to send Alexa a command to stream the audio file identified by the specified audioItem. 
        /// Use the playBehavior parameter to determine whether the stream begins playing immediately,
        /// or is added to the queue.
        /// </summary>
        /// <param name="stream">Representing the audio stream to play</param>
        /// <param name="playbehavior">Describes playback behavior.</param>
        /// <param name="outputSpeech"></param>
        /// <returns>The SpeechletResponse</returns>
        public SpeechletResponse AudioPlayer_Play(AudioStream stream, OutputSpeech outputSpeech, PlayBehaviorEnum playbehavior = PlayBehaviorEnum.REPLACE_ALL)
        {

            var response = new SpeechletResponse()
            {                
                Directives = new List<Directive>()
                {
                     new AudioPlayerPlayDirective()
                     {
                           PlayBehavior = playbehavior,
                           AudioItem = new AudioItem()
                           {
                               Stream =  stream
                           }
                     }
                },
                OutputSpeech = outputSpeech,
                ShouldEndSession = true
            };

            return response;
        }

        /// <summary>
        /// Create a SpeechletResponse to Stop the current audio playback.
        /// </summary>
        /// <returns>The SpeechletResponse</returns>
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

        /// <summary>
        /// Create a SpeechletResponse to Clear the audio playback queue. 
        /// You can set this directive to clear the queue without stopping the currently playing stream, 
        /// or clear the queue and stop any currently playing stream.
        /// </summary>
        /// <param name="clearBehavior">Describes the clear queue behavior</param>
        /// <returns>The SpeechletResponse</returns>
        public SpeechletResponse AudioPlayer_ClearQueue(ClearBehaviorEnum clearBehavior = ClearBehaviorEnum.CLEAR_ALL)
        {
            var response = new SpeechletResponse()
            {
                Directives = new List<Directive>()
                 {
                     new AudioPlayerClearQueueDirective()
                     {
                           ClearBehavior = clearBehavior
                     }
                 },
                ShouldEndSession = true,
            };

            return response;
        }

        public SpeechletResponse VideoApp_Launch(VideoItem videoItem)
        {
            var response = new SpeechletResponse()
            {
                Directives = new List<Directive>()
                 {
                     new VideoAppLaunchDirective()
                     {
                           VideoItem = videoItem
                     }
                 },
                //The shouldEndSession parameter must not be included in the response
                ShouldEndSession = null,
            };

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceUrl">Dentifies the location of video content at a remote HTTPS location.</param>
        /// <param name="metadata">[OPTIONAL] Information that can be displayed on VideoApp</param>
        /// <returns>The SpeechletResponse</returns>
        public SpeechletResponse VideoApp_Launch(string sourceUrl, VideoItemMetadata metadata = null)
        {
            var response = new SpeechletResponse()
            {
                Directives = new List<Directive>()
                 {
                     new VideoAppLaunchDirective()
                     {
                           VideoItem = new VideoItem()
                           {
                                Source = sourceUrl,
                                Metadata = metadata
                           }
                     }
                 },
                //The shouldEndSession parameter must not be included in the response
                ShouldEndSession = null,
            };

            return response;
        }

        /// <summary>
        /// Ask for Permissions
        /// </summary>
        /// <param name="permissionType"></param>
        /// <returns>The SpeechletResponse</returns>
        public SpeechletResponse AskForPermissionsConsentCard(params PermissionTypeEnum[] permissionType)
        {
            return new SpeechletResponse()
            {
                Card = new AskForPermissionsConsentCard(permissionType),
                //do not send
                ShouldEndSession = null
            };
        }

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
                          UpdatedIntent = intentRequest.Intent
                     }
                 }
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
        public SpeechletResponse Say(SsmlBuilder ssmblBuilder, bool shouldEndSession = true)
        {
            return new SpeechletResponse()
            {
                OutputSpeech = new SsmlOutputSpeech() { Ssml = ssmblBuilder.ToString() },
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
                    Text = Resource.NO_INTENT_FOUND,
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
                    Text = Resource.NO_LAUNCH_FUNCTION,
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
                    Text = Resource.GENERIC_ERROR,
                },
                ShouldEndSession = true
            };
        }

        #endregion

        #region Address Request

        public bool CanRequestAdress
        {
            get
            {
                return (!string.IsNullOrEmpty(RequestEnvelope?.Context?.System?.Device?.DeviceId)
                 && !string.IsNullOrEmpty(RequestEnvelope.Context.System.User.Permissions.ConsentToken));
            }
        }

        public enum GetAdressResultEnum
        {
            OK,
            Error_NoDeviceId,
            Error_NoConsentToken,
            Error_RequestNotSupported,
            Error_Unkown,
            /// <summary>
            /// The query did not return any results
            /// </summary>
            Error_NoContent,
            /// <summary>
            /// The authentication token is invalid or doesn’t have access to the resource.
            /// </summary>
            Error_Forbidden,
            /// <summary>
            /// The method is not supported.
            /// </summary>
            Error_Method_Not_Allowed,
            /// <summary>
            /// The skill has been throttled due to an excessive number of requests.
            /// </summary>
            Error_Too_Many_Requests,
            /// <summary>
            /// An unexpected error occurred.
            /// </summary>
            Error_InternalServerError,

        }

        public class AddressRequestResult
        {
            public AddressRequestResult(GetAdressResultEnum resultCode, User_Address address = null)
            {
                this.ResultCode = resultCode;
                this.Address = address;
            }

            public GetAdressResultEnum ResultCode { get; private set; }

            public User_Address Address { get; private set; }
        }

        public AddressRequestResult GetAddress(PermissionTypeEnum permissionType)
        {
            return GetAddressAsync(permissionType, geographicLocation).Result;
        }

        public async Task<AddressRequestResult> GetAddressAsync(PermissionTypeEnum permissionType)
        {
            if (string.IsNullOrEmpty(RequestEnvelope?.Context?.System?.Device?.DeviceId))
                return new AddressRequestResult(GetAdressResultEnum.Error_NoDeviceId);

            if (string.IsNullOrEmpty(RequestEnvelope.Context.System.User.Permissions.ConsentToken))
                return new AddressRequestResult(GetAdressResultEnum.Error_NoConsentToken);


            string _Url;

            switch (permissionType)
            {
                case PermissionTypeEnum.FullAddress:
                    _Url = $"/v1/devices/{RequestEnvelope.Context.System.Device.DeviceId}/settings/address";
                    break;
                case PermissionTypeEnum.CountryAndPostalCode:
                    _Url = $"/v1/devices/{RequestEnvelope.Context.System.Device.DeviceId}/settings/address/countryAndPostalCode";
                    break;
                default:
                    return new AddressRequestResult(GetAdressResultEnum.Error_RequestNotSupported);
            }

            var f = new System.Net.Http.HttpClient();
            f.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + RequestEnvelope.Context.System.User.Permissions.ConsentToken);

            try
            {
                var response = await f.GetAsync(RequestEnvelope.Context.System.ApiEndpoint + _Url).ConfigureAwait(false);
               
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NoContent: 
                        return new AddressRequestResult(GetAdressResultEnum.Error_NoContent);
                    case HttpStatusCode.MethodNotAllowed:
                        return new AddressRequestResult(GetAdressResultEnum.Error_Unkown);
                    case HttpStatusCode.Forbidden: 
                        return new AddressRequestResult(GetAdressResultEnum.Error_Forbidden);
                    case (HttpStatusCode)429: // Too Many Requests
                        return new AddressRequestResult(GetAdressResultEnum.Error_Too_Many_Requests);
                    case HttpStatusCode.InternalServerError:
                        return new AddressRequestResult(GetAdressResultEnum.Error_Unkown);
                    case HttpStatusCode.OK:
                        break;
                    default:
                        return new AddressRequestResult(GetAdressResultEnum.Error_Unkown);
                }

                var address =  Newtonsoft.Json.JsonConvert.DeserializeObject<User_Address>(
                    await response.Content.ReadAsStringAsync().ConfigureAwait(false), 
                    Sdk.DeserializationSettings);

                return new AddressRequestResult(GetAdressResultEnum.OK, address);
            }
            catch (Exception)
            {
                return new AddressRequestResult(GetAdressResultEnum.Error_Unkown);
            }
        }

        #endregion
    }
}
    