
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Linq;
using System.Collections;
using System.IO;

namespace Ra.AlexaSkillsKit.Authentication
{

    public abstract class CertValidator
    {
        public static System.Collections.Concurrent.ConcurrentDictionary<string, LocalCertCache> Cache =
            new System.Collections.Concurrent.ConcurrentDictionary<string, LocalCertCache>();


        public RequestHeader Header { get; set; }

        public SpeechletRequestValidationResult Verify(RequestHeader header)
        {
            this.Header = header;

            if (string.IsNullOrWhiteSpace(header.CertChainUrl))
                return SpeechletRequestValidationResult.NoCertHeader;

            if (!VerifyCertificateUrl())
                return SpeechletRequestValidationResult.InvalidSignature;


            if (string.IsNullOrWhiteSpace(header.Signature))
                return SpeechletRequestValidationResult.NoSignatureHeader;


            if (!GetCertficate())
                return SpeechletRequestValidationResult.InvalidSignature;


            if (!VerifyData())
            {
                if (Cache.TryRemove(header.CertChainUrl, out LocalCertCache tmp))
                {
                    if (!GetCertficate())
                        return SpeechletRequestValidationResult.InvalidSignature;

                    if (!VerifyData())
                        return SpeechletRequestValidationResult.InvalidSignature;
                }
                return SpeechletRequestValidationResult.InvalidSignature;
            }



            return SpeechletRequestValidationResult.OK;
        }

        protected bool GetCertficate()
        {

            // if not in Cache?
            var cacheItem = Cache.GetOrAdd(Header.CertChainUrl, 
                // Add
                (key) => new LocalCertCache()
                {
                    ValidUntil = DateTime.UtcNow.AddHours(24)
                });

            if (cacheItem.ValidUntil < DateTime.UtcNow)
                cacheItem = null;

            if (cacheItem.CertBytes == null)
            {
                {
                    var certdata = RetrieveCertificate();
                    if (certdata == null)
                        return false;

                    if (!CreateCertificate(certdata))
                        return false;

                    if (!VerifyCertificate())
                        return false;

                    // Store in Cache
                    cacheItem.CertBytes = certdata;
                    cacheItem.ValidUntil = DateTime.UtcNow.AddHours(24);
                }
            }

            return true;
        }

        public bool VerifyCertificateUrl()
        {
            if (String.IsNullOrEmpty(Header.CertChainUrl))
            {
                return false;
            }

            if (!Uri.TryCreate(Header.CertChainUrl, UriKind.Absolute, out Uri certChainUri))
            {
                return false;
            }

            return
                certChainUri.Host.Equals(Sdk.SIGNATURE_CERT_URL_HOST, StringComparison.OrdinalIgnoreCase) &&
                certChainUri.PathAndQuery.StartsWith(Sdk.SIGNATURE_CERT_URL_PATH) &&
                certChainUri.Scheme.ToLowerInvariant() == "https" &&
                certChainUri.Port == 443;
        }

        protected byte[] RetrieveCertificate()
        {

            try
            {
                byte[] result = null;
                using (var httpClient = new HttpClient())
                {
                    result = httpClient.GetByteArrayAsync(Header.CertChainUrl).Result;
                }
                return result;

            }
            catch { }
            return null;
        }

        protected abstract bool CreateCertificate(byte[] certbytes);

        protected abstract bool VerifyCertificate();

        protected abstract bool VerifyData();
    }

}



