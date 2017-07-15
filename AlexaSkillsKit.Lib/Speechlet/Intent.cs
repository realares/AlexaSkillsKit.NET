//  Copyright 2015 Stefan Negritoiu (FreeBusy). See LICENSE file for more information.

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