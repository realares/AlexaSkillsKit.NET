using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ra.AlexaSkillsKit.Resolution
{

    /// <summary>
    /// An object representing the resolved value for the slot, based on the user’s utterance and the slot type definition.
    /// </summary>
    public class ResolutionValue
    {
        /// <summary>
        /// The string for the resolved slot value
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The unique ID defined for the resolved slot value. This is based on the IDs defined in the slot type definition.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
    }

}
