//  Copyright 2015 Stefan Negritoiu (FreeBusy). See LICENSE file for more information.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ra.AlexaSkillsKit.Helper
{
    public static class HttpHelpers
    {
        /// <summary>
        /// 
        /// </summary>
        public static string ToLogString(this HttpRequestMessage httpRequest)
        {
            var serializedRequest = AsyncHelpers.RunSync<string>(() =>
                httpRequest.Content.ReadAsStringAsync());
            return serializedRequest;
        }


        /// <summary>
        /// 
        /// </summary>
        public static string ToLogString(this HttpResponseMessage httpResponse)
        {
            var serializedRequest = AsyncHelpers.RunSync<string>(() =>
                httpResponse.Content.ReadAsStringAsync());
            return serializedRequest;
        }
    }
}