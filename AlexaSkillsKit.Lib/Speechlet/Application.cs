//  Copyright 2015 Stefan Negritoiu (FreeBusy). See LICENSE file for more information.

using System;
using Newtonsoft.Json.Linq;

namespace AlexaSkillsKit.Speechlet
{
    public class Application
    {
        public virtual string Id
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Application FromJson(JObject json)
        {

            if (json == null)
                return null;

            return new Application {
                Id = json.Value<string>("applicationId")
            };
        }


    }
}