
#if HasSystemSecurityCryptography
#if !HasBouncyCastle

namespace Ra.AlexaSkillsKit.Authentication
{
    using System;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;

    public class DotNetCertValidator : CertValidator
    {

        protected X509Certificate2 Cert;

        protected override bool CreateCertificate(byte[] certbytes)
        {
            Cert = new X509Certificate2(certbytes);
            return true;
        }

        protected override bool VerifyCertificate()
        {
            var alternativeName = Cert.GetNameInfo(X509NameType.DnsFromAlternativeName, false);
            if (alternativeName != Sdk.ECHO_API_DOMAIN_NAME)
                return false;

            return Cert.Verify();
        }

        protected override bool VerifyData()
        {
            
            if (string.IsNullOrWhiteSpace(Header.Signature))
                return false;

            if (Cert.PublicKey == null || Cert.PublicKey.Key == null)
                return false;

            byte[] expectedSig = null;
            try
            {
                expectedSig = Convert.FromBase64String(Header.Signature);
            }
            catch (FormatException)
            {
                return false;
            }

            RSACryptoServiceProvider csp2 = (RSACryptoServiceProvider)Cert.PublicKey.Key;
            if (csp2.VerifyData(Header.RequestRawData, "SHA1", expectedSig))
            {
                return true;
            }
            return false;
        }
    }
}
#endif
#endif