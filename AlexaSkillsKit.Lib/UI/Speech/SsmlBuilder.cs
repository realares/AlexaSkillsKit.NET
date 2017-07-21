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

using System.Text;
using static AlexaSkillsKit.UI.Speech.SSML_Amazon_Effect;

namespace AlexaSkillsKit.UI.Speech
{
    public class SsmlBuilder
    {
        protected StringBuilder _builder;

        public SsmlBuilder()
        {
            _builder = new StringBuilder();
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="effect"></param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public SsmlBuilder Append_AmazonEffect(Speech.SSML_Amazon_Effect effect)
        {
            _builder.Append(effect.ToString());
            return this;
        }
        public SsmlBuilder Append_Audio(Speech.SSML_Audio effect)
        {
            _builder.Append(effect.ToString());
            return this;
        }

        public SsmlBuilder Break(Speech.SSML_Break speechBreak)
        {
            _builder.Append(speechBreak).ToString();
            return this;
        }
        public SsmlBuilder Break(Speech.SSML_Break.SSML_BreakstrengthEnum speechBreakStrength)
        {
            _builder.Append(new Speech.SSML_Break(speechBreakStrength).ToString());
            return this;
        }
        public SsmlBuilder Break(int speechBreakTimeMS)
        {
            _builder.Append(new Speech.SSML_Break(speechBreakTimeMS).ToString());
            return this;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public SsmlBuilder Wisper(string text)
        {
            _builder.Append(new Speech.SSML_Amazon_Effect(SSML_AmazonEffectEnum.whispered, text).ToString());
            return this;
        }



        /// <summary>
        /// Appends a copy of the specified string to this instance.
        /// </summary>
        /// <param name="value">The string to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException:" Enlarging the value of this instance would exceed System.Text.StringBuilder.MaxCapacity. </exception>
        public SsmlBuilder Append(string value)
        {
            this._builder.Append(value);
            return this;
        }

        /// <summary>
        /// Appends the string returned by processing a composite format string, which contains
        /// zero or more format items, to this instance. Each format item is replaced by
        /// the string representation of a single argument.
        /// </summary>
        /// <param name="format">A composite format string (see Remarks).</param>
        /// <param name="arg0">An object to format.</param>
        /// <returns> A reference to this instance with format appended. Each format item in format
        /// is replaced by the string representation of arg0.</returns>
        public SsmlBuilder AppendFormat(string format, object arg0)
        {
            _builder.AppendFormat(format, arg0);
            return this;
        }

        /// <summary>
        /// Appends the string returned by processing a composite format string, which contains
        /// zero or more format items, to this instance. Each format item is replaced by
        /// the string representation of a single argument.
        /// </summary>
        /// <param name="format">A composite format string (see Remarks).</param>
        /// <param name="arg0">An object to format.</param>
        /// <param name="arg1">The second object to format.</param>
        /// <returns>A reference to this instance with format appended. Each format item in format
        /// is replaced by the string representation of the corresponding object argument.</returns>
        public SsmlBuilder AppendFormat(string format, object arg0, object arg1)
        {
            _builder.AppendFormat(format, arg0, arg1);
            return this;
        }

        /// <summary>
        /// Appends the string returned by processing a composite format string, which contains
        /// zero or more format items, to this instance. Each format item is replaced by
        /// the string representation of a single argument.
        /// </summary>
        /// <param name="format">A composite format string (see Remarks).</param>
        /// <param name="arg0">An object to format.</param>
        /// <param name="arg1">The second object to format.</param>
        /// <param name="arg2">The third object to format.</param>
        /// <returns>A reference to this instance with format appended. Each format item in format
        /// is replaced by the string representation of the corresponding object argument.</returns>
        public SsmlBuilder AppendFormat(string format, object arg0, object arg1, object arg2)
        {
            _builder.AppendFormat(format, arg0, arg1, arg2);
            return this;
        }

        /// <summary>
        /// Appends the string returned by processing a composite format string, which contains
        /// zero or more format items, to this instance. Each format item is replaced by
        /// the string representation of a corresponding argument in a parameter array.
        /// </summary>
        /// <param name="format">A composite format string (see Remarks).</param>
        /// <param name="args">An array of objects to format.</param>
        /// <returns>A reference to this instance with format appended. Each format item in format
        /// is replaced by the string representation of the corresponding object argument.</returns>
        public SsmlBuilder AppendFormat(string format, params object[] args)
        {
            this._builder.AppendFormat(format, args);
            return this;
        }

        /// <summary>
        /// Appends a copy of the specified string followed by the default line terminator
        /// to the end of the current System.Text.StringBuilder object.
        /// </summary>
        /// <param name="value">The string to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public SsmlBuilder AppendLine(string value)
        {
            this._builder.AppendLine(value);
            return this;
        }
        
        /// <summary>
        /// Appends the default line terminator to the end of the current System.Text.StringBuilder object.
        /// </summary>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public SsmlBuilder AppendLine()
        {
            this._builder.AppendLine();
            return this;
        }

        /// <summary>
        /// Removes all characters from the current System.Text.StringBuilder instance.
        /// </summary>
        /// <returns>An object whose System.Text.StringBuilder.Length is 0 (zero).</returns>
        public SsmlBuilder Clear()
        {
            this._builder.Clear();
            return this;
        }



        public override string ToString()
        {
            return $"<speak>{_builder.ToString()}</speak>";
        }
    }
}