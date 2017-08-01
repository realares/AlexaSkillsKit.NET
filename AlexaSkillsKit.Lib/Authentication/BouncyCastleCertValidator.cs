using System;
using System.Text;
using System.Collections;
using System.IO;

#if HasBouncyCastle

namespace Ra.AlexaSkillsKit.Authentication
{
    using Org.BouncyCastle.X509;

    public class BouncyCastleCertValidator : CertValidator
    {
        public X509Certificate Cert;


        protected override bool CreateCertificate(byte[] certbytes)
        {
            try
            {
                var pemReader = new Org.BouncyCastle.OpenSsl.PemReader(new StringReader(ASCIIEncoding.ASCII.GetString(certbytes)));
                Cert = (X509Certificate)pemReader.ReadObject();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected override bool VerifyCertificate()
        {
            if (!CheckCertSubjectNames(Cert))
                return false;

            return Cert.IsValidNow;
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

        protected override bool VerifyData()
        {
            byte[] expectedSig = null;
            try
            {
                expectedSig = Convert.FromBase64String(Header.Signature);
            }
            catch (FormatException)
            {
                return false;
            }

            var publicKey = (Org.BouncyCastle.Crypto.Parameters.RsaKeyParameters)Cert.GetPublicKey();
            var signer = Org.BouncyCastle.Security.SignerUtilities.GetSigner(Sdk.SIGNATURE_ALGORITHM);
            signer.Init(false, publicKey);
            signer.BlockUpdate(Header.RequestRawData, 0, Header.RequestRawData.Length);

            return signer.VerifySignature(expectedSig);
        }
    }

}

#endif


