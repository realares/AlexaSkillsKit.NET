//  Copyright 2015 Stefan Negritoiu (FreeBusy). See LICENSE file for more information.

using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AlexaSkillsKit.Speechlet
{
    public class Slot
    {
        public virtual string Name { get; set; }

        public virtual string Value { get; set; }
        
        public virtual ConfirmationStatusEnum ConfirmationStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Slot FromJson(JObject json)
        {

            if (json == null)
                return null;


            var slot = new Slot
            {
                Name = json.Value<string>("name"),
                Value = json.Value<string>("value"),
            };

            if (Enum.TryParse<ConfirmationStatusEnum>(json.Value<string>("confirmationStatus"), out ConfirmationStatusEnum tmp))
                slot.ConfirmationStatus = tmp;

            return slot;
        }
        

    }
}