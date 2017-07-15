using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexaSkillsKit.Speechlet.Resolution
{
    public class ResolutionValueEntry
    {
        [JsonProperty("value")]
        public ResolutionValue Value { get; set; }

    }
}
