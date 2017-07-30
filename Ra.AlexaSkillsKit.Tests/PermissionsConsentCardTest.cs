using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ra.AlexaSkillsKit.UI.Cards;

namespace Ra.AlexaSkillsKit.Tests
{
    [TestClass]
    public class PermissionsConsentCardTest
    {
        [TestMethod]
        public void Test_PermissionsConsentCard()
        {
            var ask = new AskForPermissionsConsentCard(new PermissionTypeEnum[] { PermissionTypeEnum.FullAddress });

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(ask, Sdk.DeserializationSettings);

            string assetstg = "{\"type\":\"askForPermissionsConsent\",\"permissions\":[\"read::alexa:device:all:address\"]}";

            Assert.AreEqual(json, assetstg);
        }
    }
}
