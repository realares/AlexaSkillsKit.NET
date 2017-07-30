using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ra.AlexaSkillsKit
{
    /// <summary>
    /// If a response to an AudioPlayer request causes an error, your skill is sent a System.ExceptionEncountered request. 
    /// Any directives included in the response are ignored.
    /// </summary>
    public class AudioPlayerRequest_System_ExceptionEncountered : AudioPlayerRequest
    {
        public override RequestTypeEnum Type => RequestTypeEnum.System_ExceptionEncountered;

        /// <summary>
        /// Contains an object with error information
        /// </summary>
        [JsonProperty("error")]
        public AudioPlayerException Error { get; set; }

        /// <summary>
        /// Contains an object with the cause the error
        /// </summary>
        [JsonProperty("cause")]
        public AudioPlayerExceptionCause Cause { get; set; }



    }
}
