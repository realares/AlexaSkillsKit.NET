using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AlexaSkillsKit.Json
{
    public class AlexaDateTimeConverter : Newtonsoft.Json.JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) return null;
            
            return ParseAlexaTimeStamp(reader.Value.ToString());
        }

        public static DateTime ParseAlexaTimeStamp(string ts)
        {
            bool isNum = true;
            foreach (var c in ts)
            {
                if (c < '0' || c > '9')
                {
                    isNum = false;
                    break;
                }
            }

            if (isNum)
            {
                long num = long.Parse(ts);
                return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(num);
            }

            return DateTime.Parse(ts);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}