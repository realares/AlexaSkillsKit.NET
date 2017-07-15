//  Copyright 2015 Stefan Negritoiu (FreeBusy). See LICENSE file for more information.

using System;
using Newtonsoft.Json.Linq;

namespace AlexaSkillsKit.Speechlet
{
    public class User
    {
        public virtual string Id { get; set; }

        public virtual string AccessToken { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static User FromJson(JObject json)
        {
            if (json == null)
                return null;

            return new User {
                Id = json.Value<string>("userId"),
                AccessToken = json.Value<string>("accessToken")
            };
        }


    }
}