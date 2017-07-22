/* 
Original work Copyright (c) 2015 Stefan Negritoiu (FreeBusy) 
Modified work Copyright 2017 Frank Kuchta

The MIT License (MIT)
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using Ra.AlexaSkillsKit.Json;
using JsonSubTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Ra.AlexaSkillsKit
{
    [JsonConverter(typeof(JsonSubtypes), "type")]
    [JsonSubtypes.KnownSubType(typeof(IntentRequest), "IntentRequest")]
    [JsonSubtypes.KnownSubType(typeof(LaunchRequest), "LaunchRequest")]
    [JsonSubtypes.KnownSubType(typeof(SessionEndedRequest), "SessionEndedRequest")]
    [JsonSubtypes.KnownSubType(typeof(SessionStartedRequest), "SessionStartedRequest")]

    [JsonSubtypes.KnownSubType(typeof(AudioPlayerRequest_PlaybackStarted), "AudioPlayer.PlaybackStarted")]
    [JsonSubtypes.KnownSubType(typeof(AudioPlayerRequest_PlaybackFinished), "AudioPlayer.PlaybackFinished")]
    [JsonSubtypes.KnownSubType(typeof(AudioPlayerRequest_PlaybackStopped), "AudioPlayer.PlaybackStopped")]
    [JsonSubtypes.KnownSubType(typeof(AudioPlayerRequest_PlaybackNearlyFinished), "AudioPlayer.PlaybackNearlyFinished")]
    [JsonSubtypes.KnownSubType(typeof(AudioPlayerRequest_PlaybackFailed), "AudioPlayer.PlaybackFailed")]

    public abstract class SpeechletRequest
    {
        public SpeechletRequest() { }

        public SpeechletRequest(string requestId, DateTime timestamp)
        {
            RequestId = requestId;
            Timestamp = timestamp;
        }

        [JsonProperty("requestId")]
        public string RequestId
        {
            get;
            set;
        }

        [JsonConverter(typeof(AlexaDateTimeConverter))]
        [JsonProperty("timestamp")]
        public DateTime Timestamp
        {
            get;
            set;
        }
    }
}