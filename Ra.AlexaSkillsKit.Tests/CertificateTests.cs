using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ra.AlexaSkillsKit.Authentication;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Ra.AlexaSkillsKit.Tests
{
    [TestClass]
    public class CertificateTests
    {
        [TestMethod]
        public void UrlCheckTest()
        {
            /*
             * The protocol is equal to https (case insensitive).
             * The hostname is equal to s3.amazonaws.com (case insensitive).
             * The path starts with /echo.api/ (case sensitive).
             * If a port is defined in the URL, the port is equal to 443.
             */

            string[] correctUrls =
                {
                    "https://s3.amazonaws.com/echo.api/echo-api-cert.pem",
                    "https://s3.amazonaws.com:443/echo.api/echo-api-cert.pem",
                    "https://s3.amazonaws.com/echo.api/../echo.api/echo-api-cert.pem"
                };


            var validator = new DotNetCertValidator()
            {
                Header = new RequestHeader()
            };

            foreach (var item in correctUrls)
            {
                validator.Header.CertChainUrl = item;
                Assert.IsTrue(validator.VerifyCertificateUrl());
            }

            string[] invalidUrls =
               {
                    null,
                    string.Empty,
                    "http://s3.amazonaws.com/echo.api/echo-api-cert.pem", // invalid protocol
                    "https://notamazon.com/echo.api/echo-api-cert.pem", // invalid hostname
                    "https://s3.amazonaws.com/EcHo.aPi/echo-api-cert.pem", //invalid path
                    "https://s3.amazonaws.com/invalid.path/echo-api-cert.pem", //invalid path
                    "https://s3.amazonaws.com:563/echo.api/echo-api-cert.pem", //invalid port
                };

            foreach (var item in invalidUrls)
            {
                validator.Header.CertChainUrl = item;
                Assert.IsFalse(validator.VerifyCertificateUrl());
            }

        }

        [TestMethod]
        public void TimestampToleranceTest()
        {

            var utcNow = DateTime.UtcNow;
            var envelop = new Json.SpeechletRequestEnvelope()
            {
                Request = new IntentRequest()
                {
                    Timestamp = utcNow
                }
            };


            Assert.IsTrue(SpeechletRequestTimestampVerifier.VerifyRequestTimestamp(envelop, utcNow));

            envelop.Request.Timestamp = utcNow.AddSeconds(+ (Sdk.TIMESTAMP_TOLERANCE_SEC - 1));
            Assert.IsTrue(SpeechletRequestTimestampVerifier.VerifyRequestTimestamp(envelop, utcNow));

            envelop.Request.Timestamp = utcNow.AddSeconds(- (Sdk.TIMESTAMP_TOLERANCE_SEC - 1));
            Assert.IsTrue(SpeechletRequestTimestampVerifier.VerifyRequestTimestamp(envelop, utcNow));

            envelop.Request.Timestamp = utcNow.AddSeconds(+(Sdk.TIMESTAMP_TOLERANCE_SEC + 1));
            Assert.IsFalse(SpeechletRequestTimestampVerifier.VerifyRequestTimestamp(envelop, utcNow));

            envelop.Request.Timestamp = utcNow.AddSeconds(-(Sdk.TIMESTAMP_TOLERANCE_SEC + 1));
            Assert.IsFalse(SpeechletRequestTimestampVerifier.VerifyRequestTimestamp(envelop, utcNow));
        }

        // this test is private
        [TestMethod]
        public void TestChain()
        {
            var signature = "MY SIGNATURE";
            var data = Convert.FromBase64String("REQUEST AS BASE64");

            var validator = new DotNetCertValidator();
            var header = new RequestHeader()
            {
                CertChainUrl = "https://s3.amazonaws.com/echo.api/echo-api-cert-3.pem",
                RequestRawData = data,
                Signature = signature
            };
            
            //Assert.AreEqual(validator.Verify(header), SpeechletRequestValidationResult.InvalidSignature);

            header.CertChainUrl = "https://s3.amazonaws.com/echo.api/echo-api-cert-4.pem";

            Assert.AreEqual(validator.Verify(header), SpeechletRequestValidationResult.OK);

        }
    }
}