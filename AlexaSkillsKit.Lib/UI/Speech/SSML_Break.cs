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
