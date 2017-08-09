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

namespace Ra.AlexaSkillsKit
{
    public class Session
    {
        public const string INTENT_SEQUENCE = "intentSequence";
        public const string SEPARATOR = ";";


        [JsonProperty("sessionId")]
        public virtual string SessionId { get;  set; }


        /// <summary>
        /// Return whether this is a new session, meaning the current request is the initial request starting the speechlet.
        /// true if this is a new session; otherwise, false if this is a subsequent request within an existing session
        /// </summary>
        [JsonProperty("new")]
        public virtual bool IsNew { get; set; }

        /// <summary>
        /// Returns the application.
        /// </summary>
        [JsonProperty("application")]
        public virtual Application Application { get; set; }

        /// <summary>
        /// Returns the user associated with this session.
        /// </summary>
        [JsonProperty("user")]
        public virtual User User { get; set; }

        private Dictionary<string, string> _Attributes;

        /// <summary>
        /// Returns all the session attributes.
        /// </summary>
        [JsonProperty("attributes")]
        public virtual Dictionary<string, string> Attributes
        {
            get
            {
                if (_Attributes == null)
                    _Attributes = new Dictionary<string, string>();
                return _Attributes;
            }
            set
            {
                _Attributes = value;
            }
        }        


        public virtual string[] IntentSequence {
            get {
                return !Attributes.ContainsKey(INTENT_SEQUENCE) ?
                    new string[0] : 
                    Attributes[INTENT_SEQUENCE].Split(
                        new string[1] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
            }
        }
    }
}