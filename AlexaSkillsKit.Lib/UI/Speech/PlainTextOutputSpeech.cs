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

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace Ra.AlexaSkillsKit.UI.Speech
{
    public class PlainTextOutputSpeech : OutputSpeech
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public override OutputSpeechTypeEnum Type => OutputSpeechTypeEnum.PlainText;

        [JsonProperty("text")]
        public virtual string Text { get; set; }

        public PlainTextOutputSpeech() { }

        /// <summary>
        /// Create a new Instance of <see cref="AlexaSkillsKit.UI.Speech.PlainTextOutputSpeech"/>
        /// </summary>
        /// <param name="text"></param>
        public PlainTextOutputSpeech(string text)
        {
            Text = text;
        }
    }
}