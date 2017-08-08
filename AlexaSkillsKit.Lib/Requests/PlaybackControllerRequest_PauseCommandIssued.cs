namespace Ra.AlexaSkillsKit
{
    /// <summary>
    /// Sent when the user uses a “pause” button with the intent to stop playback.
    /// </summary>
    public class PlaybackControllerRequest_PauseCommandIssued : AudioPlayerRequest
    {
        public override RequestTypeEnum Type => RequestTypeEnum.PlaybackController_PauseCommandIssued;
    }

}
