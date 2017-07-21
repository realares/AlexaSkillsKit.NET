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
using AlexaSkillsKit.Directives;
using System.Collections.Generic;
using AlexaSkillsKit.UI;
using Newtonsoft.Json;

namespace AlexaSkillsKit
{
    public class IntentRequest : SpeechletRequest
    {
        [JsonProperty("intent")]
        public virtual Intent Intent { get; private set; }

        [JsonProperty("dialogState")]
        public DialogStateEnum DialogState { get; set; }

        [JsonProperty("confirmationStatus")]
        public ConfirmationStatusEnum ConfirmationStatus { get; set; }

        public IntentRequest(JObject requestJson, string requestId, DateTime timestamp)  
            : base(requestId, timestamp)
        {

            Intent = JsonConvert.DeserializeObject<Intent>(requestJson["intent"].ToString());
 
            if (Enum.TryParse<DialogStateEnum>(requestJson.Value<string>("dialogState"), out DialogStateEnum tmp1))
                DialogState = tmp1;

            if (Enum.TryParse<ConfirmationStatusEnum>(requestJson.Value<string>("confirmationStatus"), out ConfirmationStatusEnum tmp2))
                ConfirmationStatus = tmp2;
        }



    }


}