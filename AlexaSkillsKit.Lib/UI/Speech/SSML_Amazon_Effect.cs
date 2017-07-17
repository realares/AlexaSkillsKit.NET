/* Copyright 2017 Frank Kuchta

The MIT License (MIT)
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AlexaSkillsKit.UI.Speech
{
    /// <summary>
    /// Applies Amazon-specific effects to the speech.
    /// </summary>
    public class SSML_Amazon_Effect
    {
        public enum SSML_AmazonEffectEnum
        {
            /// <summary>
            /// Applies a whispering effect to the speech.
            /// </summary>
            [EnumMember(Value = "whispered")]
            whispered
        }

        public SSML_AmazonEffectEnum Name { get; set; }

        public string Text { get; set; }

        public SSML_Amazon_Effect()
        {
            Name = SSML_AmazonEffectEnum.whispered;
            Text = string.Empty;
        }

        public SSML_Amazon_Effect(SSML_AmazonEffectEnum name)
        {
            Name = name;
            Text = string.Empty;
        }

        public SSML_Amazon_Effect(SSML_AmazonEffectEnum name, string text)
        {
            Name = name;
            Text = text;
        }

        public override string ToString()
        {
            return $"{Open}{Text}{Close}";
        }

        public string Open => $"<amazon:effect name=\"{Name.ToString()}\">";
        public string Close => $"</amazon:effect>";
    }
}
