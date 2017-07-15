using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AlexaSkillsKit.Speechlet
{
    public class SpeechletSystem
    {
        public Application Application { get; private set; }
        public User User { get; set; }
        public virtual Device Device { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static SpeechletSystem FromJson(JObject json)
        {
            if (json == null)
                return null;

            return new SpeechletSystem()
            {
                Application = Application.FromJson(json.Value<JObject>("application")),
                User = User.FromJson(json.Value<JObject>("user")),
                Device = Device.FromJson(json.Value<JObject>("device"))
            };
        }

    }
}