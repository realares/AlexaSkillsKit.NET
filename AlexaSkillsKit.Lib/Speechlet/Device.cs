using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AlexaSkillsKit.Speechlet
{
    public class Device
    {
        public SupportedInterfaces SupportedInterfaces { get; set; }

        public virtual string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Device FromJson(JObject json)
        {
            return new Device
            {
                Id = json.Value<string>("deviceId"),
                SupportedInterfaces = SupportedInterfaces.FromJson(json.Value<JObject>("supportedInterfaces"))
            };
        }


    }
}