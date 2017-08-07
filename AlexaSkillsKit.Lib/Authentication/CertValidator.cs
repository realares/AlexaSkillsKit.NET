
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Linq;
using System.Collections;
using System.IO;
using System.Threading.Tasks;

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
                cacheItem.CertBytes = null;

            if (cacheItem.CertBytes == null)
            {
                {
                    var certdata = RetrieveCertificate();
                    if (certdata == null)
                        return false;

                    cacheItem.CertBytes = certdata;
                    cacheItem.ValidUntil = DateTime.UtcNow.AddHours(24);
                }
            }

            if (!CreateCertificate(cacheItem.CertBytes))
                return false;

            if (!VerifyCertificate())
                return false;

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
            return RetrieveCertificateAsync().Result;
        }


        protected abstract bool CreateCertificate(byte[] certbytes);

        protected abstract bool VerifyCertificate();

        protected abstract bool VerifyData();


        public async Task<SpeechletRequestValidationResult> VerifyAsync(RequestHeader header)
        {
            this.Header = header;

            if (string.IsNullOrWhiteSpace(header.CertChainUrl))
                return SpeechletRequestValidationResult.NoCertHeader;

            if (!VerifyCertificateUrl())
                return SpeechletRequestValidationResult.InvalidSignature;


            if (string.IsNullOrWhiteSpace(header.Signature))
                return SpeechletRequestValidationResult.NoSignatureHeader;


            if (!await GetCertficateAsync())
                return SpeechletRequestValidationResult.InvalidSignature;


            if (!VerifyData())
            {
                if (Cache.TryRemove(header.CertChainUrl, out LocalCertCache tmp))
                {
                    if (!await GetCertficateAsync())
                        return SpeechletRequestValidationResult.InvalidSignature;

                    if (!VerifyData())
                        return SpeechletRequestValidationResult.InvalidSignature;
                }
                return SpeechletRequestValidationResult.InvalidSignature;
            }



            return SpeechletRequestValidationResult.OK;
        }

        protected async Task<bool> GetCertficateAsync()
        {

            // if not in Cache?
            var cacheItem = Cache.GetOrAdd(Header.CertChainUrl,
                // Add
                (key) => new LocalCertCache()
                {
                    ValidUntil = DateTime.UtcNow.AddHours(24)
                });

            if (cacheItem.ValidUntil < DateTime.UtcNow)
                cacheItem.CertBytes = null;

            if (cacheItem.CertBytes == null)
            {
                {
                    var certdata = await RetrieveCertificateAsync().ConfigureAwait(false);
                    if (certdata == null)
                        return false;

                    cacheItem.CertBytes = certdata;
                    cacheItem.ValidUntil = DateTime.UtcNow.AddHours(24);
                }
            }

            if (!CreateCertificate(cacheItem.CertBytes))
                return false;

            if (!VerifyCertificate())
                return false;

            return true;
        }

        protected async Task<byte[]> RetrieveCertificateAsync()
        {

            try
            {
                byte[] result = null;
                using (var httpClient = new HttpClient())
                {
                    result = await httpClient.GetByteArrayAsync(Header.CertChainUrl).ConfigureAwait(false);
                }
                return result;

            }
            catch { }
            return null;
        }

    }

}



