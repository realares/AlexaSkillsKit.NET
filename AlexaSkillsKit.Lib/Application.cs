//  Copyright 2015 Stefan Negritoiu (FreeBusy). See LICENSE file for more information.

using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Ra.AlexaSkillsKit
{
    public class Application
    {
        [JsonProperty("applicationId")]
        public virtual string Id { get; set; }
    }
}