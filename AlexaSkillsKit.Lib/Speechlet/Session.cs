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
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace AlexaSkillsKit.Speechlet
{
    public class Session
    {
        public const string INTENT_SEQUENCE = "intentSequence";
        public const string SEPARATOR = ";";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        //public static Session FromJson(JObject json) {
        //    var attributes = new Dictionary<string, string>();
        //    var jsonAttributes = json.Value<JObject>("attributes");
        //    if (jsonAttributes != null) {
        //        foreach (var attrib in jsonAttributes.Children()) {
        //            attributes.Add(attrib.Value<JProperty>().Name, attrib.Value<JProperty>().Value.ToString());
        //        }
        //    }

        //    return new Session {
        //        SessionId = json.Value<string>("sessionId"),
        //        IsNew = json.Value<bool>("new"),
        //        User = User.FromJson(json.Value<JObject>("user")),
        //        Application = Application.FromJson(json.Value<JObject>("application")),
        //        Attributes = attributes
        //    };
        //}

        [JsonProperty("sessionId")]
        public virtual string SessionId { get;  set; }

        [JsonProperty("isNew")]
        public virtual bool IsNew { get; set; }

        [JsonProperty("application")]
        public virtual Application Application { get; set; }

        [JsonProperty("user")]
        public virtual User User { get; set; }

        [JsonProperty("attributes")]
        public virtual Dictionary<string, string> Attributes { get; set; }

        public virtual string[] IntentSequence {
            get {
                return String.IsNullOrEmpty(Attributes[INTENT_SEQUENCE]) ?
                    new string[0] : 
                    Attributes[INTENT_SEQUENCE].Split(
                        new string[1] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
            }
        }
    }
}