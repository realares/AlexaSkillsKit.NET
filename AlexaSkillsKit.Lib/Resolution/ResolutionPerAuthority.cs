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
    public class ResolutionPerAuthority
    {
        /// <summary>
        /// The name of the authority for the slot values. For custom slot types, 
        /// this authority label incorporates your skill ID and the slot type name.
        /// </summary>
        [JsonProperty("authority")]
        public string Authority { get; set; }

        /// <summary>
        /// An object representing the status of entity resolution for the slot.
        /// </summary>
        [JsonProperty("status")]
        public ResolutionStatus Status { get; set; }

        /// <summary>
        /// An array of resolved values for the slot.
        /// </summary>
        [JsonProperty("values")]
        public List<ResolutionValueEntry> Values { get; set; }

    }

    
}
