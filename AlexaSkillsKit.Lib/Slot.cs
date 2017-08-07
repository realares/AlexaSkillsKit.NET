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

using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Ra.AlexaSkillsKit.Resolution;
using System.Xml;

namespace Ra.AlexaSkillsKit
{
    public class Slot
    {
        [JsonProperty("name")]
        public virtual string Name { get; set; }

        [JsonProperty("value")]
        public virtual string Value { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("confirmationStatus")]
        public virtual ConfirmationStatusEnum ConfirmationStatus { get; set; }
        
        //[JsonIgnore]
        [JsonProperty("resolutions")]
        public virtual Resolutions Resolutions { get; set; }

        public bool ShouldSerializeResolutions() => false;

        public ResolutionStatusCodeEnum GetResolutionsCode()
        {
            if (Resolutions == null ||
                Resolutions.ResolutionsPerAuthority == null ||
                Resolutions.ResolutionsPerAuthority.Count == 0 ||
                Resolutions.ResolutionsPerAuthority[0].Status == null)
            {
                return ResolutionStatusCodeEnum.ER_NONE;
            }

            return Resolutions.ResolutionsPerAuthority[0].Status.Code;
        }

        /// <summary>
        /// AMAZON.DURATION
        /// Converts words that indicate durations into a numeric duration.
        /// The duration is provided to your skill in a format based on the ISO-8601 duration format(PnYnMnDTnHnMnS). 
        /// The P indicates that this is a duration.The n is the numeric value, and the capital letter following the n designates the specific date or time element. 
        /// For example, P3D means 3 days.A T is used to indicate that the remaining values represent time elements rather than date elements.
        /// </summary>
        /// <returns>Converted value</returns>
        public TimeSpan ParseAs_AMAZON_DURATION()
        {
            if (string.IsNullOrWhiteSpace(Value))
                return TimeSpan.Zero;
            return XmlConvert.ToTimeSpan(Value);
        }
    }

}