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

namespace Ra.AlexaSkillsKit
{
    public enum RequestTypeEnum
    {
        Intent,
        Lunch,

        SessionEnded,

        AudioPlayer_PlaybackFailed,

        /// <summary>
        /// Sent when the stream Alexa is playing comes to an end on its own.
        /// </summary>
        AudioPlayer_PlaybackFinished,

        /// <summary>
        /// Sent when the device is ready to add the next stream to the queue.
        /// To progress through a playlist of audio streams, 
        /// respond to this request with a Play directive for the next stream and set playBehavior to ENQUEUE or REPLACE_ENQUEUED. 
        /// This adds the new stream to the queue without stopping the current playback.Alexa begins streaming the new audio item once the currently playing track finishes.
        /// </summary>
        AudioPlayer_PlaybackNearlyFinished,

        /// <summary>
        /// Sent when Alexa begins playing the audio stream previously sent in a Play directive. This lets your skill verify that playback began successfully.
        /// This request is also sent when Alexa resumes playback after pausing it for a voice request.
        /// </summary>
        AudioPlayer_PlaybackStarted,

        /// <summary>
        /// Sent when Alexa stops playing an audio stream in response to one of the following AudioPlayer directives: Stop
        /// Play with a playBehavior of REPLACE_ALL. ClearQueue with a clearBehavior of CLEAR_ALL. 
        /// This request is also sent if the user makes a voice request to Alexa, since this temporarily pauses the playback. 
        /// In this case, the playback begins automatically once the voice interaction is complete.
        /// </summary>
        AudioPlayer_PlaybackStopped,


        /// <summary>
        /// Sent when the user uses a “next” button with the intent to skip to the next audio item.
        /// </summary>
        /// <remarks>Your skill can respond to NextCommandIssued with any AudioPlayer directive.</remarks>
        PlaybackController_NextCommandIssued,

        /// <summary>
        /// Sent when the user uses a “pause” button with the intent to stop playback.
        /// </summary>
        /// <remarks>Your skill can respond to NextCommandIssued with any AudioPlayer directive.</remarks>
        PlaybackController_PauseCommandIssued,

        /// <summary>
        /// Sent when the user uses a “play” or “resume” button to start or resume playback.
        /// </summary>
        /// <remarks>Your skill can respond to NextCommandIssued with any AudioPlayer directive.</remarks>
        PlaybackController_PlayCommandIssued,

        /// <summary>
        /// Sent when the user uses a “previous” button with the intent to go back to the previous audio item.
        /// </summary>
        /// <remarks>Your skill can respond to NextCommandIssued with any AudioPlayer directive.</remarks>
        PlaybackController_PreviousCommandIssued,

        [EnumMember(Value = "System.ExceptionEncountered")]
        System_ExceptionEncountered
    }
}
 
 
 
 
 