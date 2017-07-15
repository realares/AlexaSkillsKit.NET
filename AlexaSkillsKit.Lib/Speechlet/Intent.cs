﻿/* 
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
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace AlexaSkillsKit.Speechlet
{
    public class Intent
    {

        public virtual string Name { get; set; }

        public virtual Dictionary<string, Slot> Slots { get; set; }

        public virtual ConfirmationStatusEnum ConfirmationStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Intent FromJson(JObject json)
        {
            
            if (json == null)
                return null;

            var slots = new Dictionary<string, Slot>();

            if (json["slots"] != null && json.Value<JObject>("slots").HasValues)
            {
                foreach (var slot in json.Value<JObject>("slots").Children())
                {
                    slots.Add(slot.Value<JProperty>().Name, Slot.FromJson(slot.Value<JProperty>().Value as JObject));
                }
            }

            var intent = new Intent {
                Name = json.Value<string>("name"),
                Slots = slots
            };

            var stg_confirmstatus = json.Value<string>("confirmationStatus");
            if (!string.IsNullOrWhiteSpace(stg_confirmstatus))
                if (Enum.TryParse<ConfirmationStatusEnum>(stg_confirmstatus, out ConfirmationStatusEnum outenum))
                    intent.ConfirmationStatus = outenum;

            return intent;
        }


        public class Amazon_BuiltIns
        {
            public const string YesIntent = "AMAZON.YesIntent";
            public const string NoIntent = "AMAZON.NoIntent";

            public const string HelpIntent = "AMAZON.HelpIntent";
            public const string StopIntent = "AMAZON.StopIntent";
            public const string CancelIntent = "AMAZON.CancelIntent";
        }
    }
}