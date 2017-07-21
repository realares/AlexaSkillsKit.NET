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

namespace AlexaSkillsKit
{
    public class AudioPlayer
    {
        /// <summary>
        /// An opaque token provided in the Play directive.
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }

        /// <summary>
        /// Identifies the component state of AudioPlayer. 
        /// Accepted Values: IDLE, PLAYING, STOPPED, PAUSED, BUFFER_UNDERRUN, and FINISHED.
        /// </summary>
        [JsonProperty("playerActivity")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PlayerActivityEnum PlayerActivity { get; set; }

        /// <summary>
        /// Identifies a track's current offset in milliseconds.
        /// </summary>
        [JsonProperty("offsetInMilliseconds")]
        public long OffsetInMilliseconds { get; set; }


        public enum PlayerActivityEnum
        {
            /// <summary>
            /// Nothing was playing, no enqueued items.
            /// </summary>
            IDLE,

            /// <summary>
            /// Stream was playing.
            /// </summary>
            PLAYING,

            /// <summary>
            /// Stream was interrupted.
            /// </summary>
            STOPPED,

            /// <summary>
            /// Stream was paused.
            /// </summary>
            PAUSED,

            /// <summary>
            /// Buffer underrun.
            /// </summary>
            BUFFER_UNDERRUN,

            /// <summary>
            /// Stream was finished playing
            /// </summary>
            FINISHED




        }
    }
}