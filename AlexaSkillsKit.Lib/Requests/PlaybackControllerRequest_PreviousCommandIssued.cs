namespace Ra.AlexaSkillsKit
{
    /// <summary>
    /// Sent when the user uses a “previous” button with the intent to go back to the previous audio item.
    /// </summary>
    public class PlaybackControllerRequest_PreviousCommandIssued : AudioPlayerRequest
    {
        public override RequestTypeEnum Type => RequestTypeEnum.PlaybackController_PreviousCommandIssued;
    }

}
