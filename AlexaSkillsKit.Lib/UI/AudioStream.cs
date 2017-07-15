using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexaSkillsKit.UI
{
    public class AudioStream
    {
        public string Token { get; set; }
        public string Url { get; set; }
        public long OffsetInMilliseconds { get; set; }
    }
}
