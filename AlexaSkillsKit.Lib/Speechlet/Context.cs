using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using AlexaSkillsKit.Slu;

namespace AlexaSkillsKit.Speechlet
{
    public class Context
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Context FromJson(JObject json)
        {
            if (json == null)
                return null;

            return new Context
            {
                System = SpeechletSystem.FromJson(json.Value<JObject>("System")),
                AudioPlayer = AudioPlayer.FromJson(json.Value<JObject>("AudioPlayer"))
            };
        }

        public AudioPlayer AudioPlayer { get; set; }

        public virtual SpeechletSystem System
        {
            get;
            set;
        }

    }

}