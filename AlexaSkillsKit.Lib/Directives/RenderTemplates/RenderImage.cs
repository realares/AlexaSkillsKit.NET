using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ra.AlexaSkillsKit.Directives.RenderTemplates
{
    /// <summary>
    /// <para>The image object in the display templates takes the following format. 
    /// Note that the image format used for images on cards is different.</para>
    /// <para>The contentDescription property is text used to describe the image for a screen reader.
    /// The fields size, widthPixels, and heightPixels are optional.By default, for Echo Show, 
    /// size takes the value X_SMALL.If the other size values are included, 
    /// then the order of precedence for displaying images begins with X_LARGE and proceeds downward, 
    /// which means that larger images will be downscaled for display on Echo Show if provided.
    /// For the best user experience, include the appropriately sized image, and do not include larger images.</para>
    /// <para>Do not include the widthPixels and heightPixels integer values, which are optional, unless they are exactly correct.</para>
    /// </summary>
    public class RenderImage
    {
        [JsonProperty("contentDescription")]
        public string ContentDescription { get; set; }

        [JsonProperty("sources")]
        public List<RenderImageSources> Sources { get; set; }

    }

}
