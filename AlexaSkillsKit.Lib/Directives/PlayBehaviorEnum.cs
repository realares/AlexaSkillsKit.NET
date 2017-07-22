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


using System.Runtime.Serialization;

namespace Ra.AlexaSkillsKit.Directives
{
    public enum PlayBehaviorEnum
    {
        /// <summary>
        /// Immediately begin playback of the specified stream, and replace current and enqueued streams
        /// </summary>
        [EnumMember(Value = "REPLACE_ALL")]
        REPLACE_ALL,

        /// <summary>
        /// Add the specified stream to the end of the current queue. This does not impact the currently playing stream
        /// </summary>
        [EnumMember(Value = "ENQUEUE")]
        ENQUEUE,

        /// <summary>
        /// Replace all streams in the queue. This does not impact the currently playing stream
        /// </summary>
        [EnumMember(Value = "REPLACE_ENQUEUED")]
        REPLACE_ENQUEUED,
    }
}