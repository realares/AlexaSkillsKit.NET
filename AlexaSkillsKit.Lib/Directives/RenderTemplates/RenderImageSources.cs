using Newtonsoft.Json;

namespace Ra.AlexaSkillsKit.Directives.RenderTemplates
{
    public class RenderImageSources
    {
        [JsonProperty("url")]
        public string Url;

        [JsonProperty("size")]
        public RenderImageSizeEnum Size;

        [JsonProperty("widthPixels", NullValueHandling = NullValueHandling.Ignore)]
        public int? WidthPixels;

        [JsonProperty("heightPixels", NullValueHandling = NullValueHandling.Ignore)]
        public int? HeightPixels;
    }

}
