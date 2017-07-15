using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace AlexaSkillsKit.Speechlet
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ConfirmationStatusEnum
    {
        [EnumMember(Value ="NONE")]
        NONE,

        [EnumMember(Value = "DENIED")]
        DENIED,

        [EnumMember(Value = "CONFIRMED")]
        CONFIRMED
    }
}
