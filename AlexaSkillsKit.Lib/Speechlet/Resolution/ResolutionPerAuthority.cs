using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexaSkillsKit.Speechlet.Resolution
{
    public class ResolutionPerAuthority
    {
        public string Authority { get; set; }

        public ResolutionStatus Status { get; set; }

        public List<ResolutionValueEntry> Values { get; set; }
    }
}
