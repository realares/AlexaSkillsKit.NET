using System;
using System.Collections.Generic;
using System.Text;

namespace Ra.AlexaSkillsKit
{
    public class PlaybackControllerRequest : SpeechletRequest
    {
        public override RequestTypeEnum Type =>  RequestTypeEnum.PlaybackController;
    }
}
