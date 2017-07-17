using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AlexaSkillsKit.UI.Speech
{
    /// <summary>
    /// Describes how the text should be interpreted. 
    /// This lets you provide additional context to the text and eliminate any ambiguity on how Alexa should render the text. 
    /// Indicate how Alexa should interpret the text with the interpret-as attribute.
    /// </summary>
    public class SSML_SayAs
    {
        
        public enum SSML_SayAsinterpretAsEnum
        {
            /// <summary>
            /// Spell out each letter
            /// </summary>
            [EnumMember(Value = "characters")]
            characters,

            /// <summary>
            /// Spell out each letter
            /// </summary>
            [EnumMember(Value = "spell-out")]
            spell_out,

            /// <summary>
            ///  Interpret the value as a cardinal number.
            /// </summary>
            [EnumMember(Value = "cardinal")]
            cardinal,

            /// <summary>
            ///  Interpret the value as a cardinal number.
            /// </summary>
            [EnumMember(Value = "number")]
            number,

            /// <summary>
            /// Interpret the value as an ordinal number.
            /// </summary>
            [EnumMember(Value = "ordinal")]
            ordinal,

            /// <summary>
            /// Spell each digit separately .
            /// </summary>
            [EnumMember(Value = "digits")]
            digits,

            /// <summary>
            ///  Interpret the value as a fraction. This works for both common fractions (such as 3/20) and mixed fractions (such as 1+1/2).
            /// </summary>
            [EnumMember(Value = "fraction")]
            fraction,

            /// <summary>
            /// Interpret a value as a measurement. The value should be either a number or fraction followed by a unit (with no space in between) or just a unit.
            /// </summary>
            [EnumMember(Value = "unit")]
            unit,

            /// <summary>
            /// Interpret the value as a date. Specify the format with the format attribute
            /// </summary>
            [EnumMember(Value = "date")]
            date,

            /// <summary>
            /// Interpret a value such as 1'21" as duration in minutes and seconds.
            /// </summary>
            [EnumMember(Value = "time")]
            time,

            /// <summary>
            /// Interpret a value as a 7-digit or 10-digit telephone number. This can also handle extensions (for example, 2025551212x345).
            /// </summary>
            [EnumMember(Value = "telephone")]
            telephone,

            /// <summary>
            /// Interpret a value as part of street address.
            /// </summary>
            [EnumMember(Value = "address")]
            address,

            /// <summary>
            /// Interpret the value as an interjection. Alexa speaks the text in a more expressive voice. 
            /// For optimal results, only use the supported interjections and surround each one with a pause. 
            /// For example: <say-as interpret-as="interjection">Wow.</say-as>. 
            /// Speechcons are supported for English (US), English (UK), and German.
            /// </summary>
            [EnumMember(Value = "interjection")]
            interjection,

            /// <summary>
            /// “Bleep” out the content inside the tag.
            /// </summary>
            [EnumMember(Value = "expletive")]
            expletive
        }

        public SSML_SayAsinterpretAsEnum InterpretAs { get; set; }

        /// <summary>
        /// Only used when interpret-as is set to date. Set to one of the following to indicate format of the date:
        /// mdy, dmy, ymd, md, dm, ym, my, d, m, y
        /// Alternatively, if you provide the date in YYYYMMDD format, the format attribute is ignored. 
        /// You can include question marks (?) for portions of the date to leave out. For instance, 
        /// Alexa would speak <say-as interpret-as="date">????0922</say-as> as “September 22nd”.
        /// </summary>
        public string Format { get; set; }

        public string Text { get; set; }

         /// <summary>
         /// 
         /// </summary>
         /// <param name="interpretAs"></param>
         /// <param name="text"></param>
         /// <param name="format"></param>
        public SSML_SayAs(SSML_SayAsinterpretAsEnum interpretAs, string text, string format)
        {
            InterpretAs = interpretAs;
            Text = text;
            Format = format;
        }

        public SSML_SayAs(SSML_SayAsinterpretAsEnum interpretAs, string text)
        {
            InterpretAs = interpretAs;
            Text = text;
            Format = null;
        }


        public override string ToString()
        {
            if (InterpretAs == SSML_SayAsinterpretAsEnum.date && !string.IsNullOrWhiteSpace(Format))
                return $"<say-as interpret-as=\"{InterpretAs.ToString()}\" format=\"{Format}\">{Text}</say-as>";
            else
                return $"<say-as interpret-as=\"{InterpretAs.ToString()}\">{Text}</say-as>";
        }
    }
}
