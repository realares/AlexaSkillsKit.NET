using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Ra.AlexaSkillsKit.Directives.RenderTemplates;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ra.AlexaSkillsKit.Directives
{
    public class HintDirective : Directive
    {

        public override DirectiveTypesEnum Type { get => DirectiveTypesEnum.Hint; }

        [JsonProperty("hint")]
        public RenderHint Hint { get; set; }

    }
}
