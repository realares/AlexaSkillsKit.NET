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

namespace Ra.AlexaSkillsKit
{
    public enum AudioPlayerErrorTypeEnum
    {
        NOT_SET,

        /// <summary>
        /// An unknown error occurred.
        /// </summary>
        MEDIA_ERROR_UNKNOWN,

        /// <summary>
        /// Alexa recognized the request as being malformed. E.g. bad request, unauthorized, forbidden, not found, etc.
        /// </summary>
        MEDIA_ERROR_INVALID_REQUEST,

        /// <summary>
        /// Alexa was unable to reach the URL for the stream.
        /// </summary>
        MEDIA_ERROR_SERVICE_UNAVAILABLE,

        /// <summary>
        /// Alexa accepted the request, but was unable to process the request as expected.
        /// </summary>
        MEDIA_ERROR_INTERNAL_SERVER_ERROR,

        /// <summary>
        /// There was an internal error on the device.
        /// </summary>
        MEDIA_ERROR_INTERNAL_DEVICE_ERROR
    }
}
 
 
 
 
 