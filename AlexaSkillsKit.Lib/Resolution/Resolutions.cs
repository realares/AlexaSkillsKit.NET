using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexaSkillsKit.Resolution
{
    public class Resolutions
    {
        /// <summary>
        /// An array of objects representing each possible authority for entity resolution. 
        /// An authority represents the source for the data provided for the slot. For a custom slot type, 
        /// the authority is the slot type you defined.
        /// </summary>
        [JsonProperty("resolutionsPerAuthority")]
        public List<ResolutionPerAuthority> ResolutionsPerAuthority { get; set; }
    }
}
