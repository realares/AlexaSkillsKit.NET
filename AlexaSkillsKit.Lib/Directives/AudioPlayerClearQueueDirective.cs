/* Copyright(c) 2017 Frank Kuchta
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

namespace AlexaSkillsKit.Directives
{
    /// <summary>
    /// Clears the audio playback queue. 
    /// You can set this directive to clear the queue without stopping the currently playing stream, 
    /// or clear the queue and stop any currently playing stream.
    /// </summary>
    public class AudioPlayerClearQueueDirective : Directive
    {
        /// <summary>
        /// Set to AudioPlayer.ClearQueue.
        /// </summary>
        public override DirectiveTypesEnum Type { get => DirectiveTypesEnum.AudioPlayer_ClearQueue; }

        /// <summary>
        /// Describes the clear queue behavior
        /// </summary>
        [JsonProperty("clearBehavior")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ClearBehaviorEnum ClearBehavior { get; set; }

    }

    public enum ClearBehaviorEnum
    {
        /// <summary>
        /// clears the queue and continues to play the currently playing stream
        /// </summary>
        CLEAR_ENQUEUED,

        /// <summary>
        /// clears the entire playback queue and stops the currently playing stream (if applicable)
        /// </summary>
        CLEAR_ALL
    }
}