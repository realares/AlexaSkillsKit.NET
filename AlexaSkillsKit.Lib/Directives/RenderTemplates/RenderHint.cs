using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ra.AlexaSkillsKit.Directives.RenderTemplates
{
    public class RenderHint
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RenderTextTypeEnum Type { get; } = RenderTextTypeEnum.PlainText;

        [JsonProperty("text")]
        public string Text { get; set; }

    }
}
