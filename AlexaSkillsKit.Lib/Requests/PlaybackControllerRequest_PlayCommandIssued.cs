namespace Ra.AlexaSkillsKit
{
    /// <summary>
    /// Sent when the user uses a “play” or “resume” button with the intent to start or resume playback.
    /// </summary>
    public class PlaybackControllerRequest_PlayCommandIssued : AudioPlayerRequest
    {
        public override RequestTypeEnum Type => RequestTypeEnum.PlaybackController_PlayCommandIssued;
    }

}
