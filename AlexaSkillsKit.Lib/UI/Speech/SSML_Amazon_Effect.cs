using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AlexaSkillsKit.UI.Speech
{
    /// <summary>
    /// Applies Amazon-specific effects to the speech.
    /// </summary>
    public class SSML_Amazon_Effect
    {
        public enum SSML_AmazonEffectEnum
        {
            /// <summary>
            /// Applies a whispering effect to the speech.
            /// </summary>
            [EnumMember(Value = "whispered")]
            whispered
        }

        public SSML_AmazonEffectEnum Name { get; set; }

        public string Text { get; set; }

        public SSML_Amazon_Effect()
        {
            Name = SSML_AmazonEffectEnum.whispered;
            Text = string.Empty;
        }

        public SSML_Amazon_Effect(SSML_AmazonEffectEnum name)
        {
            Name = name;
            Text = string.Empty;
        }

        public SSML_Amazon_Effect(SSML_AmazonEffectEnum name, string text)
        {
            Name = name;
            Text = text;
        }

        public override string ToString()
        {
            return $"{Open}{Text}{Close}";
        }

        public string Open => $"<amazon:effect name=\"{Name.ToString()}\">";
        public string Close => $"</amazon:effect>";
    }
}
