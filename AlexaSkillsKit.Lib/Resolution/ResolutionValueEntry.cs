using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ra.AlexaSkillsKit.Resolution
{
    /// <summary>
    /// An array of resolved values for the slot.
    /// </summary>
    public class ResolutionValueEntry
    {
        /// <summary>
        /// An object representing the resolved value for the slot, based on the user’s utterance and the slot type definition.
        /// </summary>
        [JsonProperty("value")]
        public ResolutionValue Value { get; set; }

    }
}
