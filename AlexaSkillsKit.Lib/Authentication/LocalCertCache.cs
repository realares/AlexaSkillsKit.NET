
using System;

namespace Ra.AlexaSkillsKit.Authentication
{
    public class LocalCertCache
    {
        public byte[] CertBytes { get; set; }

        public DateTime ValidUntil { get; set; }
    }

}
