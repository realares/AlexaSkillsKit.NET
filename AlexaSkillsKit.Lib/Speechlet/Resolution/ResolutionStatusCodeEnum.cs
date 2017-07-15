using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AlexaSkillsKit.Speechlet.Resolution
{
  
    public enum ResolutionStatusCodeEnum
    {
        [EnumMember(Value = "ER_SUCCESS_MATCH")]
        ER_SUCCESS_MATCH,

        [EnumMember(Value = "ER_SUCCESS_NO_MATCH")]
        ER_SUCCESS_NO_MATCH
    }
}
