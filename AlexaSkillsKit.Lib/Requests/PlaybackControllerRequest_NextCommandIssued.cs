namespace Ra.AlexaSkillsKit
{
    /// <summary>
    /// Sent when the user uses a “next” button with the intent to skip to the next audio item.
    /// </summary>
    public class PlaybackControllerRequest_NextCommandIssued : AudioPlayerRequest
    {
        public override RequestTypeEnum Type => RequestTypeEnum.PlaybackController_NextCommandIssued;

    }

}
