using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexaSkillsKit.Speechlet.Resolution
{
    public class ResolutionPerAuthority
    {
        [JsonProperty("authority")]
        public string Authority { get; set; }

        [JsonProperty("status")]
        public ResolutionStatus Status { get; set; }

        [JsonProperty("values")]
        public List<ResolutionValueEntry> Values { get; set; }

    }

    
}
