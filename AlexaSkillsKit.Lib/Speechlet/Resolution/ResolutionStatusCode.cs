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
    public class ResolutionStatus
    {
        [JsonProperty("code")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ResolutionStatusCodeEnum Code { get; set; }

    }
}
