using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Ra.AlexaSkillsKit.Directives.RenderTemplates
{
    public class RenderText
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RenderTextTypeEnum Type { get; set; }
    }

}
