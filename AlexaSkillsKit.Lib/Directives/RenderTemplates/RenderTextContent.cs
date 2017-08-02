using Newtonsoft.Json;

namespace Ra.AlexaSkillsKit.Directives.RenderTemplates
{
    /// <summary>
    /// Contains primaryText, secondaryText, and tertiaryText.
    /// </summary>
    public class RenderTextContent
    {
        /// <summary>
        /// Contains type (which is PlainText or RichText) and text (which is a string).
        /// </summary>
        [JsonProperty("primaryText")]
        public RenderText PrimaryText { get; set; }

        /// <summary>
        /// Contains type (which is PlainText or RichText) and text (which is a string).
        /// </summary>
        [JsonProperty("secondaryText")]
        public RenderText SecondaryText { get; set; }

        /// <summary>
        /// Contains type (which is PlainText or RichText) and text (which is a string).
        /// </summary>
        [JsonProperty("tertiaryText")]
        public RenderText TertiaryText { get; set; }

    }

}
