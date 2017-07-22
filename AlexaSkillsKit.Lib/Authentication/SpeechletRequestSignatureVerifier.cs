﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Security.Certificates;

namespace Ra.AlexaSkillsKit.Authentication
{
    public class SpeechletRequestSignatureVerifier
    {
        private static Func<string, string> _getCertCacheKey = (string url) => string.Format("{0}_{1}", Sdk.SIGNATURE_CERT_URL_REQUEST_HEADER, url);
        private static Dictionary<string, X509Certificate> _certCache = new Dictionary<string, X509Certificate>();
        private static DateTimeOffset _certCacheAbsoluteExpiration = DateTimeOffset.UtcNow.AddHours(24);


        /// <summary>
        /// Verifying the Signature Certificate URL per requirements documented at
        /// https://developer.amazon.com/public/solutions/alexa/alexa-skills-kit/docs/developing-an-alexa-skill-as-a-web-service
        /// </summary>
        public static bool VerifyCertificateUrl(string certChainUrl)
        {
            if (String.IsNullOrEmpty(certChainUrl))
            {
                return false;
            }

            Uri certChainUri;
            if (!Uri.TryCreate(certChainUrl, UriKind.Absolute, out certChainUri))
            {
                return false;
            }

            return
                certChainUri.Host.Equals(Sdk.SIGNATURE_CERT_URL_HOST, StringComparison.OrdinalIgnoreCase) &&
                certChainUri.PathAndQuery.StartsWith(Sdk.SIGNATURE_CERT_URL_PATH) &&
                certChainUri.Scheme.ToLowerInvariant() == "https" &&
                certChainUri.Port == 443;
        }


        /// <summary>
        /// Verifies request signature and manages the caching of the signature certificate
        /// </summary>
        public static bool VerifyRequestSignature(
            byte[] serializedSpeechletRequest, string expectedSignature, string certChainUrl)
        {

            X509Certificate cert = GetCachedCertificate(certChainUrl);
            if (cert == null ||
                !CheckRequestSignature(serializedSpeechletRequest, expectedSignature, cert))
            {

                // download the cert 
                // if we don't have it in cache or
                // if we have it but it's stale because the current request was signed with a newer cert
                // (signaled by signature check fail with cached cert)
                cert = RetrieveAndVerifyCertificate(certChainUrl);
                if (cert == null) return false;

                SetCachedCertificate(certChainUrl, cert);
            }

            return CheckRequestSignature(serializedSpeechletRequest, expectedSignature, cert);
        }


        /// <summary>
        /// Verifies request signature and manages the caching of the signature certificate
        /// </summary>
        public async static Task<bool> VerifyRequestSignatureAsync(
            byte[] serializedSpeechletRequest, string expectedSignature, string certChainUrl)
        {

            X509Certificate cert = GetCachedCertificate(certChainUrl);
            if (cert == null ||
                !CheckRequestSignature(serializedSpeechletRequest, expectedSignature, cert))
            {

                // download the cert 
                // if we don't have it in cache or 
                // if we have it but it's stale because the current request was signed with a newer cert
                // (signaled by signature check fail with cached cert)
                cert = await RetrieveAndVerifyCertificateAsync(certChainUrl);
                if (cert == null) return false;

                SetCachedCertificate(certChainUrl, cert);
            }

            return CheckRequestSignature(serializedSpeechletRequest, expectedSignature, cert);
        }


        /// <summary>
        /// 
        /// </summary>
        public static X509Certificate RetrieveAndVerifyCertificate(string certChainUrl)
        {
            // making requests to externally-supplied URLs is an open invitation to DoS
            // so restrict host to an Alexa controlled subdomain/path
            if (!VerifyCertificateUrl(certChainUrl)) return null;

            var httpClient = new HttpClient();
            var content = httpClient.GetStringAsync(certChainUrl).Result;

            var pemReader = new Org.BouncyCastle.OpenSsl.PemReader(new StringReader(content));
            var cert = (X509Certificate)pemReader.ReadObject();
            try
            {
                cert.CheckValidity();
                if (!CheckCertSubjectNames(cert)) return null;
            }
            catch (CertificateExpiredException)
            {
                return null;
            }
            catch (CertificateNotYetValidException)
            {
                return null;
            }

            return cert;
        }


        /// <summary>
        /// 
        /// </summary>
        public async static Task<X509Certificate> RetrieveAndVerifyCertificateAsync(string certChainUrl)
        {
            // making requests to externally-supplied URLs is an open invitation to DoS
            // so restrict host to an Alexa controlled subdomain/path
            if (!VerifyCertificateUrl(certChainUrl)) return null;

            var httpClient = new HttpClient();
            var httpResponse = await httpClient.GetAsync(certChainUrl);
            var content = await httpResponse.Content.ReadAsStringAsync();
            if (String.IsNullOrEmpty(content)) return null;

            var pemReader = new Org.BouncyCastle.OpenSsl.PemReader(new StringReader(content));
            var cert = (X509Certificate)pemReader.ReadObject();
            try
            {
                cert.CheckValidity();
                if (!CheckCertSubjectNames(cert)) return null;
            }
            catch (CertificateExpiredException)
            {
                return null;
            }
            catch (CertificateNotYetValidException)
            {
                return null;
            }

            return cert;
        }


        /// <summary>
        /// 
        /// </summary>
        public static bool CheckRequestSignature(
            byte[] serializedSpeechletRequest, string expectedSignature, Org.BouncyCastle.X509.X509Certificate cert)
        {

            byte[] expectedSig = null;
            try
            {
                expectedSig = Convert.FromBase64String(expectedSignature);
            }
            catch (FormatException)
            {
                return false;
            }

            var publicKey = (Org.BouncyCastle.Crypto.Parameters.RsaKeyParameters)cert.GetPublicKey();
            var signer = Org.BouncyCastle.Security.SignerUtilities.GetSigner(Sdk.SIGNATURE_ALGORITHM);
            signer.Init(false, publicKey);
            signer.BlockUpdate(serializedSpeechletRequest, 0, serializedSpeechletRequest.Length);

            return signer.VerifySignature(expectedSig);
        }


        /// <summary>
        /// 
        /// </summary>
        private static bool CheckCertSubjectNames(X509Certificate cert)
        {
            bool found = false;
            var subjectNamesList = (IList)cert.GetSubjectAlternativeNames();
            for (int i = 0; i < subjectNamesList.Count; i++)
            {
                var subjectNames = (IList)subjectNamesList[i];
                for (int j = 0; j < subjectNames.Count; j++)
                {
                    if (subjectNames[j] is String && subjectNames[j].Equals(Sdk.ECHO_API_DOMAIN_NAME))
                    {
                        found = true;
                        break;
                    }
                }
            }

            return found;
        }

        /// <summary>
        /// 
        /// </summary>
        private static X509Certificate GetCachedCertificate(string url)
        {
            var key = _getCertCacheKey(url);
            if (_certCacheAbsoluteExpiration > DateTimeOffset.UtcNow)
            {
                _certCache[key] = null;
            }

            return _certCache[key];
        }

        /// <summary>
        /// 
        /// </summary>
        private static void SetCachedCertificate(string url, X509Certificate certificate)
        {
            var key = _getCertCacheKey(url);
            _certCache[key] = certificate;
        }
    }
}