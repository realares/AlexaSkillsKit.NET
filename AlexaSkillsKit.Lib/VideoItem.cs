/*  
Copyright (c) 2017 Frank Kuchta

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
using System.Text;

namespace Ra.AlexaSkillsKit
{
    /// <summary>
    /// Providing information about the video stream to play.
    /// </summary>
    public class VideoItem
    {
        /// <summary>
        /// dentifies the location of video content at a remote HTTPS location. The video file must be hosted at an Internet-accessible HTTPS endpoint.
        /// HTTPS is required, and the domain hosting the files must present a valid, trusted SSL certificate. Self-signed certificates cannot be used. 
        /// Many content hosting services provide this. 
        /// For example, you could host your files at a service such as Amazon Simple Storage Service (Amazon S3) (an Amazon Web Services offering).
        /// </summary>
        [JsonProperty("source", Required = Required.Always)]
        public string Source { get; set; }

        /// <summary>
        /// Contains an object that provides the information that can be displayed on VideoApp.
        /// </summary>
        [JsonProperty("metadata", NullValueHandling = NullValueHandling.Ignore)]
        public VideoItemMetadata Metadata { get; set; }
    }
}
