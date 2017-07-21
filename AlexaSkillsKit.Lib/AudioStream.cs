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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexaSkillsKit
{
    /// <summary>
    /// Representing the audio stream to play.
    /// </summary>
    public class AudioStream
    {
        /// <summary>
        /// Identifies the location of audio content at a remote HTTPS location.
        /// The audio file must be hosted at an Internet-accessible HTTPS endpoint.HTTPS is required, 
        /// and the domain hosting the files must present a valid, trusted SSL certificate.Self-signed certificates cannot be used.
        /// Many content hosting services provide this. For example, 
        /// you could host your files at a service such as Amazon Simple Storage Service (Amazon S3) (an Amazon Web Services offering).
        /// The supported formats for the audio file include AAC/MP4, MP3, HLS, PLS and M3U.Bitrates: 16kbps to 384 kbps.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// An opaque token that represents the audio stream. This token cannot exceed 1024 characters.
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }

        /// <summary>
        /// An opaque token that represents the expected previous stream.
        /// This property is required and allowed only when the playBehavior is ENQUEUE.
        /// This is used to prevent potential race conditions if requests to progress through a playlist and change tracks occur at the same time
        /// For example: <see cref="https://developer.amazon.com/public/solutions/alexa/alexa-skills-kit/docs/custom-audioplayer-interface-reference"/>
        /// <para>Note: Including this property when playBehavior is any other value (REPLACE_ALL or REPLACE_ENQUEUED) causes an error.</para>
        /// </summary>
        [JsonProperty("expectedPreviousToken")]
        public string ExpectedPreviousToken { get; set; }

        /// <summary>
        /// The timestamp in the stream from which Alexa should begin playback.
        /// Set to 0 to start playing the stream from the beginning. 
        /// Set to any other value to start playback from that associated point in the stream
        /// </summary>
        [JsonProperty("expectedPreviousToken")]
        public long OffsetInMilliseconds { get; set; }
    }
}
