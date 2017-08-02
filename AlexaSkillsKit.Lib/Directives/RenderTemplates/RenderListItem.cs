using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ra.AlexaSkillsKit.Directives.RenderTemplates
{
    public class RenderListItem
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("image")]
        public RenderImage Image { get; set; }

        [JsonProperty("textContent")]
        public RenderTextContent TextContent { get; set; }

    }
}
