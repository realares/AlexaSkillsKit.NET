/* 
Original work Copyright (c) 2015 Stefan Negritoiu (FreeBusy) 
Modified work Copyright 2017 Frank Kuchta

The MIT License (MIT)
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using Ra.AlexaSkillsKit.Json;

namespace Ra.AlexaSkillsKit
{
    public static class Sdk
    {
        public const string VERSION = "1.0";
        public const string CHARACTER_ENCODING = "UTF-8";
        public const string ECHO_API_DOMAIN_NAME = "echo-api.amazon.com";
        public const string SIGNATURE_CERT_URL_REQUEST_HEADER = "SignatureCertChainUrl";
        public const string SIGNATURE_CERT_URL_HOST = "s3.amazonaws.com";
        public const string SIGNATURE_CERT_URL_PATH = "/echo.api/";
        public const string SIGNATURE_CERT_TYPE = "X.509";
        public const string SIGNATURE_REQUEST_HEADER = "Signature";
        public const string SIGNATURE_ALGORITHM = "SHA1withRSA";
        public const string SIGNATURE_KEY_TYPE = "RSA";
        public const int TIMESTAMP_TOLERANCE_SEC = 150;

        public static JsonSerializerSettings DeserializationSettings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Ignore,
            ContractResolver = new CamelCaseExceptDictionaryKeysResolver(),
            Converters = new List<JsonConverter>
            {
                new Newtonsoft.Json.Converters.StringEnumConverter(true)
            }
        };
    }
}