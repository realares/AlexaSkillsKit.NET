using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ra.AlexaSkillsKit
{
    public class Permissions
    {
        [JsonProperty("consentToken")]
        public string ConsentToken { get; set; }
    }
}
