using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexaSkillsKit.UI.Speech
{

    /// <summary>
    /// The audio tag lets you provide the URL for an MP3 file that the Alexa service can play while rendering a response. 
    /// You can use this to embed short, pre-recorded audio within your service’s response. 
    /// For example, you could include sound effects alongside your text-to-speech responses, 
    /// or provide responses using a voice associated with your brand. For more information, see Including Short Pre-Recorded Audio in your Response.
    /// </summary>
    public class SSML_Audio
    {
        /// <summary>
        /// Specifies the URL for the MP3 file. Note the following requirements and limitations:
        /// The MP3 must be hosted at an Internet-accessible HTTPS endpoint.HTTPS is required, 
        /// and the domain hosting the MP3 file must present a valid, trusted SSL certificate.Self-signed certificates cannot be used.
        /// The MP3 must not contain any customer-specific or other sensitive information.
        /// The MP3 must be a valid MP3 file(MPEG version 2).
        /// The audio file cannot be longer than ninety(90) seconds.
        /// The bit rate must be 48 kbps.Note that this bit rate gives a good result when used with spoken content, but is generally not a high enough quality for music.
        /// The sample rate must be 16000 Hz.
        /// </summary>
        [JsonProperty("src")]
        public string Src { get; set; }

        /// <summary>
        /// Create new Instance of <see cref="AlexaSkillsKit.UI.SSML.SSML_audio"/> with empty Src.
        /// </summary>
        public SSML_Audio()
        {
            Src = string.Empty;
        }

        /// <summary>
        /// Create new Instance of <see cref="AlexaSkillsKit.UI.SSML.SSML_audio"/>
        /// </summary>
        /// <param name="src">Specifies the URL for the MP3 file. Note the following requirements and limitations</param>
        public SSML_Audio(string src)
        {
            Src = src;
        }

        public override string ToString()
        {
            return $"<audio src=\"{Src}\">";
        }
       
    }
}
