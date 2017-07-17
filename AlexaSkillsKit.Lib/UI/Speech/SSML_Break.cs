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
    /// Represents a pause in the speech. Set the length of the pause with the strength or time attributes.
    /// </summary>
    public class SSML_Break
    {
        
        public enum SSML_BreakstrengthEnum
        {
            /// <summary>
            /// No pause should be outputted. This can be used to remove a pause that would normally occur (such as after a period).
            /// </summary>
            [EnumMember(Value = "none")]
            none,

            /// <summary>
            ///  No pause should be outputted (same as none).
            /// </summary>
            [EnumMember(Value = "x-weak")]
            x_weak,

            /// <summary>
            /// Treat adjacent words as if separated by a single comma (equivalent to medium).
            /// </summary>
            [EnumMember(Value = "weak")]
            weak,

            /// <summary>
            /// Treat adjacent words as if separated by a single comma.
            /// </summary>
            [EnumMember(Value = "medium")]
            medium,

            /// <summary>
            /// Make a sentence break (equivalent to using the <s> tag).
            /// </summary>
            [EnumMember(Value = "strong")]
            strong,

            /// <summary>
            /// Make a paragraph break (equivalent to using the <p> tag).
            /// </summary>
            [EnumMember(Value = "x-strong")]
            x_strong
        }

        public SSML_BreakstrengthEnum Strength { get; set; }

        public int Time { get; set; }

        public SSML_Break()
        {
            Strength = SSML_BreakstrengthEnum.medium;
            Time = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strength"></param>
        /// <param name="time">Pause time in milliseconds</param>
        public SSML_Break(SSML_BreakstrengthEnum strength)
        {
            Strength = strength;
            Time = 0;
        }        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strength"></param>
        /// <param name="time">Pause time in milliseconds</param>
        public SSML_Break(int time)
        {
            Strength =  SSML_BreakstrengthEnum.medium;
            Time = time;
        }

        public override string ToString()
        {
           if (Time == 0)
                return $"<break strength=\"{Strength.ToString()}\"/> ";
            else
                return $"<break time=\"{Time}ms\"/> ";
        }
    }
}
