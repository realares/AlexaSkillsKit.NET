using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ra.AlexaSkillsKit.Directives.RenderTemplates
{

    public class BodyTemplate
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public BodyTemplateTypeEnum Type { get; set; }

        /// <summary>
        /// Used to track selectable elements in the skill service code. The value can be any user-defined string.
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }

        /// <summary>
        /// Used to place the back button on a display template.
        /// </summary>
        [JsonProperty("backButton")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RenderVisibilityEnum BackButton { get; set; }

        /// <summary>
        /// Used to include a background image on a display template.
        /// </summary>
        [JsonProperty("backgroundImage")]
        public RenderImage BackgroundImage { get; set; }

        /// <summary>
        /// Used for title for a display template. The value can be any user-defined string.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Contains primaryText, secondaryText, and tertiaryText.
        /// </summary>
        [JsonProperty("textContent")]
        public RenderTextContent TextContent { get; set; }

    }

}
