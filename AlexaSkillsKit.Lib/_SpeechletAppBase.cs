/*
 * Copyright 2017 realares
 * Original Copyright 2015 Stefan Negritoiu (FreeBusy). 
 * See LICENSE file for more information.
 */

using System;
using System.Collections.Generic;
using System.Net;
using Ra.AlexaSkillsKit.Json;
using System.Threading.Tasks;
using Ra.AlexaSkillsKit.UI.Speech;
using Ra.AlexaSkillsKit.Directives;
using Ra.AlexaSkillsKit.UI.Cards;
using Ra.AlexaSkillsKit.UI;
using Ra.AlexaSkillsKit.Ressources;

namespace Ra.AlexaSkillsKit
{
    public class _SpeechletAppBase 
    {
        public SpeechletRequestEnvelope RequestEnvelope { get; set; } = null;

        /// <summary>
        /// 
        /// </summary>
        protected void DoSessionManagement(IntentRequest request, Session session)
        {
            if (session.IsNew)
            {
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
        public SpeechletResponse AskForPermissionsConsentCard(OutputSpeech outputSpeech, params PermissionTypeEnum[] permissionType)
        {
            return new SpeechletResponse()
            {
                OutputSpeech = outputSpeech,
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
                 },
                ShouldEndSession = false
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
                 },
                ShouldEndSession = false

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
                 },
                ShouldEndSession = false
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


        /// <summary>
        /// Get if the permission is set, the address of the device.
        /// </summary>
        /// <param name="permissionType"></param>
        /// <returns></returns>
        public AddressRequestResult GetAddress(PermissionTypeEnum permissionType)
        {
            return GetAddressAsync(permissionType).Result;
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

                var address = Newtonsoft.Json.JsonConvert.DeserializeObject<User_Address>(
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
