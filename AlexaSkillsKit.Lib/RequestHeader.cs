using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Linq;

namespace Ra.AlexaSkillsKit
{
    public class RequestHeader
    {
        public string CertChainUrl { get; set; }
        public string Signature { get; set; }
        public byte[] RequestRawData { get; set; }

        public string RequestAsString => UTF8Encoding.UTF8.GetString(RequestRawData);

        
        public RequestHeader() { }

        public void Read(HttpRequestMessage httpRequest)
        {
            CertChainUrl = httpRequest.Headers.GetValues(Sdk.SIGNATURE_CERT_URL_REQUEST_HEADER).FirstOrDefault();
            Signature = httpRequest.Headers.GetValues(Sdk.SIGNATURE_REQUEST_HEADER).FirstOrDefault();
            RequestRawData = httpRequest.Content.ReadAsByteArrayAsync().Result;
        }

        public async void ReadAsync(HttpRequestMessage httpRequest)
        {
            CertChainUrl = httpRequest.Headers.GetValues(Sdk.SIGNATURE_CERT_URL_REQUEST_HEADER).FirstOrDefault();
            Signature = httpRequest.Headers.GetValues(Sdk.SIGNATURE_REQUEST_HEADER).FirstOrDefault();
            RequestRawData = await httpRequest.Content.ReadAsByteArrayAsync();
        }

        public override string ToString()
        {
            return 
$@"CertChainUrl: {CertChainUrl}
Signature: {Signature}
RequestRawData: {Convert.ToBase64String(RequestRawData)}
RequestString: {RequestAsString}";
        }
    }
}
