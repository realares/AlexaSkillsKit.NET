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


namespace Ra.AlexaSkillsKit
{
    public class Amazon_BuiltIns
    {
        public const string YesIntent = "AMAZON.YesIntent";
        public const string NoIntent = "AMAZON.NoIntent";

        public const string HelpIntent = "AMAZON.HelpIntent";
        public const string StopIntent = "AMAZON.StopIntent";
        public const string CancelIntent = "AMAZON.CancelIntent";

        public const string LoopOffIntent = "AMAZON.LoopOffIntent";
        public const string LoopOnIntent = "AMAZON.LoopOnIntent";
        public const string NextIntent = "AMAZON.NextIntent";
        public const string PreviousIntent = "AMAZON.PreviousIntent";
        public const string RepeatIntent = "AMAZON.RepeatIntent";

        // For AudioPlayer
        public const string PauseIntent = "AMAZON.PauseIntent";
        public const string ResumeIntent = "AMAZON.ResumeIntent";


        public const string ShuffleOffIntent = "AMAZON.ShuffleOffIntent";
        public const string ShuffleOnIntent = "AMAZON.ShuffleOnIntent";

        /// <summary>
        /// Let the user request to restart an action, such as restarting a game, transaction, or audio track.
        /// For skills that stream audio using the AudioPlayer interface, 
        /// users can invoke this intent without using your invocation name if your skill is currently playing audio or was the most recent skill to play audio.
        /// The intent is sent to your skill in this case even if AMAZON.StartOverIntent is not in your intent schema.
        /// </summary>
        public const string StartOverIntent = "AMAZON.StartOverIntent";

    }
}