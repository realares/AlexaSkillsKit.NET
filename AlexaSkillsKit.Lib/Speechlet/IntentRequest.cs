//  Copyright 2015 Stefan Negritoiu (FreeBusy). See LICENSE file for more information.

using System;
using AlexaSkillsKit.Slu;
using static AlexaSkillsKit.UI.Dialog;
using Newtonsoft.Json.Linq;

namespace AlexaSkillsKit.Speechlet
{
    public class IntentRequest : SpeechletRequest
    {

        public virtual Intent Intent { get; private set; }

        public DialogStateEnum DialogState { get; set; }

        public IntentRequest(JObject requestJson, string requestId, DateTime timestamp)  
            : base(requestId, timestamp)
        {

            Intent = Intent.FromJson(requestJson.Value<JObject>("intent"));
            if (Enum.TryParse<DialogStateEnum>(requestJson.Value<string>("dialogState"), out DialogStateEnum tmp))
                DialogState = tmp;
        }

    }


}