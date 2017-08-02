using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ra.AlexaSkillsKit.Directives.RenderTemplates
{

    public class ListTemplate
    {
        [JsonProperty("type")]
        public ListTemplateTypeEnum Type { get; set; }

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

        [JsonProperty("listItems")]
        public List<RenderListItem> ListItems { get;set;}
        /*
        {
         "type": "ListTemplate1",
         "token": "string",
         "backButton": "VISIBLE"(default) | "HIDDEN",
         "backgroundImage": "Image",
         "title": "string",
         "listItems": [
           {
             "token": "string",
             "image": Image,
             "textContent": TextContent
           },
           ...
           ...
           {
             "token": "string",
             "image": Image,
             "textContent": TextContent
           }
         ]
       }
       */
    }

    public enum ListTemplateTypeEnum
    {
        /// <summary>
        /// <para>Vertical list, Maximum 88 Height x 88 Width</para>
        /// <para>Title(optional),
        /// Skill icon,
        /// Rich or Plain text,
        /// Ordinals,
        /// Background image(optional),
        /// Thumbnail images(optional),
        /// Primary, secondary(optional), and tertiary(optional) text,
        /// Static or dynamically-generated search results, menu selection, lists, instructions, directions,
        /// Text and optional imagesm
        /// Scrollable by touch and voice,
        /// >Non-expandable text area</para>
        /// <see cref="https://images-na.ssl-images-amazon.com/images/G/01/mobile-apps/dex/ask-customskills/Show_ListTemplate_01_50._TTH_.png"/>
        /// </summary>
        ListTemplate1,
        /// <summary>
        /// <para>Horizontal list with text under image</para>
        /// <para>Title(optional),
        /// Skill icon,
        /// Rich or Plain text,
        /// Ordinals,
        /// One or more images(optional),
        /// Background image(optional),
        /// Static or dynamically-generated search results, menu selection, lists,
        /// Images required,
        /// Scrollable by touch and voicem
        /// >Non-expandable text area</para>
        /// <see cref="https://images-na.ssl-images-amazon.com/images/G/01/mobile-apps/dex/ask-customskills/Show_ListTemplate_02_50._TTH_.png"/>
        /// </summary>
        ListTemplate2,
        ListTemplate3
    }
}
