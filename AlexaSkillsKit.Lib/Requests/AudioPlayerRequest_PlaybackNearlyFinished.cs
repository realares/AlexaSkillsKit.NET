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
    /// <summary>
    /// Sent when the device is ready to add the next stream to the queue.
    /// To progress through a playlist of audio streams, respond to this request with a Play directive for the next stream and set playBehavior to ENQUEUE or REPLACE_ENQUEUED. 
    /// This adds the new stream to the queue without stopping the current playback.Alexa begins streaming the new audio item once the currently playing track finishes.
    /// </summary>
    public class AudioPlayerRequest_PlaybackNearlyFinished : AudioPlayerRequest
    {
        public override RequestTypeEnum Type => RequestTypeEnum.PlaybackNearlyFinished;
    }
}
 
 
 
 
 