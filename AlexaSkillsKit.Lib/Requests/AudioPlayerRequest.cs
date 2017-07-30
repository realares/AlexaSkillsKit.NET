﻿/* Copyright(c) 2017 Frank Kuchta
The MIT License (MIT)
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ra.AlexaSkillsKit
{
    public abstract class AudioPlayerRequest : SpeechletRequest
    {
        /// <summary>
        /// An opaque token that represents the audio stream. 
        /// You provide this token when sending the Play directiveAn opaque token that represents the audio stream. 
        /// You provide this token when sending the Play directive
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("")]
        public long OffsetInMilliseconds { get; set; }

    }
    
    public enum RequestTypeEnum
    {
        Intent,
        Lunch,

        SessionEnded,

        PlaybackFailed,
        PlaybackFinished,
        PlaybackNearlyFinished,
        PlaybackStarted,
        PlaybackStopped,

        PlaybackController,

        [EnumMember(Value = "System.ExceptionEncountered")]
        System_ExceptionEncountered
    }
}
 
 
 
 
 