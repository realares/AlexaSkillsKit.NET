using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexaSkillsKit.Resolution
{
    public class ResolutionStatus
    {
        /// <summary>
        /// A code indicating the results of attempting to resolve the user utterance against the defined slot types.
        /// </summary>
        [JsonProperty("code")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ResolutionStatusCodeEnum Code { get; set; }

    }
}
