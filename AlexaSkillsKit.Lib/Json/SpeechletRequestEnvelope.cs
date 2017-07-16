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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AlexaSkillsKit.Speechlet;
using AlexaSkillsKit.Slu;

namespace AlexaSkillsKit.Json
{
    public class SpeechletRequestEnvelope
    {

        public virtual SpeechletRequest Request { get; set; }

        public virtual Session Session { get; set; }

        public virtual string Version { get; set; }

        public virtual Context Context { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static SpeechletRequestEnvelope FromJson(string content)
        {
            if (String.IsNullOrEmpty(content))
            {
                throw new SpeechletException("Request content is empty");
            }

            JObject json = JsonConvert.DeserializeObject<JObject>(content, Sdk.DeserializationSettings);
            return FromJson(json);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static SpeechletRequestEnvelope FromJson(JObject json)
        {
            if (json["version"] != null && json.Value<string>("version") != Sdk.VERSION)
            {
                throw new SpeechletException($"Request must conform to {Sdk.VERSION} schema.");
            }

            SpeechletRequest request;

            JObject requestJson = json.Value<JObject>("request");

            string locale = requestJson.Value<string>("locale");
            Properties.Resource.Culture = new System.Globalization.CultureInfo(locale);

        
            string requestType = requestJson.Value<string>("type");
            string requestId = requestJson.Value<string>("requestId");

            // Web-Supportinterface send Timestamp as Unix TS e.g. 1499714531579
            // Alexa Devices send Timestamp as DT String e.g. 2017-07-10T19:09:57Z
            DateTime timestamp = ParseAlexaTimeStamp(requestJson);

            switch (requestType)
            {

                case "LaunchRequest":
                    request = new LaunchRequest(requestId, timestamp);
                    break;

                case "IntentRequest":
                    request = new IntentRequest(requestJson, requestId, timestamp);
                    break;

                case "SessionStartedRequest":
                    request = new SessionStartedRequest(requestId, timestamp);
                    break;

                case "SessionEndedRequest":
                    SessionEndedRequest.ReasonEnum reason;
                    Enum.TryParse<SessionEndedRequest.ReasonEnum>(requestJson.Value<string>("reason"), out reason);
                    request = new SessionEndedRequest(requestId, timestamp, reason);
                    break;

                default:
                    {
                        if (requestType.StartsWith("PlaybackController") || requestType.StartsWith("AudioPlayer"))
                        {
                            string token = requestJson.Value<string>("token");
                            long offset = requestJson.Value<long>("offsetInMilliseconds");
                            string type = requestJson.Value<string>("type");
                            request = new AudioPlayerRequest(requestId, timestamp, token, offset, type);
                        }
                        else if (requestType == "System.ExceptionEncountered")
                            request = null;

                    }
                    throw new ArgumentException("json");
            }

            return new SpeechletRequestEnvelope
            {
                Request = request,
                Session = JsonConvert.DeserializeObject<Session>(json["session"].ToString()),// Session.FromJson(json.Value<JObject>("session")),
                Version = json.Value<string>("version"),
                Context = JsonConvert.DeserializeObject<Context>(json["context"].ToString()) // Context.FromJson(json.Value<JObject>("context"))
            };
        }

        public static DateTime ParseAlexaTimeStamp(JObject requestJson)
        {
            string ts = requestJson.Value<string>("timestamp");

            bool isNum = true;

            foreach (var c in ts)
            {
                if (c < '0' || c > '9')
                {
                    isNum = false;
                    break;
                }
            }

            if (isNum)
            {
                long num = long.Parse(ts);
                return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(num);
            }

            return requestJson.Value<DateTime>("timestamp");
        }
    }
}