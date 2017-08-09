using NLog;
using Ra.AlexaSkillsKit.Resolution;
using Ra.AlexaSkillsKit.UI.Speech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ra.AlexaSkillsKit.WebSample.AlexaApps
{
    public class AlexaSample42App : SpeechletApp
    {
        public static Logger log = LogManager.GetCurrentClassLogger();

        public override SpeechletResponse OnIntent(IntentRequest intentRequest, Session session, Context context)
        {
            try
            {
                switch (intentRequest.Intent.Name)
                {
                    case Amazon_BuiltIns.CancelIntent:
                    case Amazon_BuiltIns.StopIntent:
                        return Say("OK");

                    case Amazon_BuiltIns.HelpIntent:
                        return Say("Ask: is the answer 42 or what is the answer to life, the universe and everything");
                    

                    case "number": // is the answer {slot_number}?
                        {
                            var slot_number = intentRequest.Intent.Slots["slot_number"];

                            // failsafe
                            if (slot_number == null)
                                return Say("Something went wrong with the slot");

                            #region // Show ElicitSlot
                            
                            // Check if the Number is recognized
                            if (slot_number.GetResolutionsCode() != ResolutionStatusCodeEnum.ER_SUCCESS_MATCH)
                                return DialogElicitSlot(slot_number, new PlainTextOutputSpeech() { Text = "Say a Number, please?" });
                            
                            #endregion

                            #region // Show Slot Confirm

                            // Confirm the recognized value...because....i can
                            switch (slot_number.ConfirmationStatus)
                            {
                                case ConfirmationStatusEnum.NONE:
                                default:
                                    return DialogConfirmSlot(slot_number, new PlainTextOutputSpeech($"Did you say {slot_number.Value}?"));

                                case ConfirmationStatusEnum.DENIED:
                                    return DialogElicitSlot(slot_number, new PlainTextOutputSpeech() { Text = "Say a Number, please" });

                                case ConfirmationStatusEnum.CONFIRMED:
                                    break;
                            }

                            #endregion

                            if (slot_number.Value == "42")
                            {
                                return Say("that's correct");
                            }

                            // Build a SSML Answer
                            var ssml = new SsmlBuilder();
                            ssml.Append("No");
                            ssml.Break(1500);
                            ssml.Whisper("The correct answer is 42");

                            // Return a SSML Answer
                            return Say(ssml);                            
                        }

                    case "question": // what is the answer to life the universe and everything?

                        #region Show Dialog Delegate
                        if (intentRequest.DialogState != DialogStateEnum.COMPLETED)
                        {
                            switch (intentRequest.Intent.ConfirmationStatus)
                            {
                                default:
                                case ConfirmationStatusEnum.NONE:
                                    return DialogDelegate(); // Ask "Was your Question what is the answer to life the universe and everything?" Set in Intent Setting

                                case ConfirmationStatusEnum.DENIED:
                                    Say("hmm Sorry");
                                    break;
                                case ConfirmationStatusEnum.CONFIRMED:
                                    break;
                            }
                        }
                        #endregion

                        return SayWithCard("42", "Alexa Testapp", "The Answer is 42", true);

                    default:
                        return Error_NoIntentFound();
                }
            }
            catch (Exception exception)
            {
                log.Error(exception);

                return Error_GenericError();
            }

        }

        public override SpeechletResponse OnLaunch(LaunchRequest launchRequest, Session session, Context context)
        {
            return Say("I'am Ready", false);
        }

        public override SpeechletResponse OnAudioIntent(AudioPlayerRequest audioRequest, Session session, Context context)
        {
            // This Sample has no Audio-Intent#

            switch (audioRequest.Type)
            {
                case RequestTypeEnum.AudioPlayer_PlaybackFailed:
                    var request = audioRequest as AudioPlayerRequest_PlaybackFailed;
                    log.Error(request.Error.Message);
                    break;

                case RequestTypeEnum.AudioPlayer_PlaybackFinished:
                case RequestTypeEnum.AudioPlayer_PlaybackNearlyFinished:
                case RequestTypeEnum.AudioPlayer_PlaybackStarted:
                case RequestTypeEnum.AudioPlayer_PlaybackStopped:
                case RequestTypeEnum.PlaybackController_NextCommandIssued:
                case RequestTypeEnum.PlaybackController_PauseCommandIssued:
                case RequestTypeEnum.PlaybackController_PlayCommandIssued:
                case RequestTypeEnum.PlaybackController_PreviousCommandIssued:
                default:
                    break;
            }

            return null;
        }

        public override void OnSessionEnded(SessionEndedRequest sessionEndedRequest, Session session, Context context)
        {           
        }

        #region Optional e.g. for Logging

        public override void OnParsingError(Exception exception)
        {
            log.Error(exception);
            base.OnParsingError(exception);
        }

        public override void OnRequestIncome(RequestHeader header)
        {
            log.Debug(header);
            base.OnRequestIncome(header);
        }

        public override void OnResponseOutgoing(string msg)
        {
            log.Debug(msg);
            base.OnResponseOutgoing(msg);
        }
        #endregion

    }
}