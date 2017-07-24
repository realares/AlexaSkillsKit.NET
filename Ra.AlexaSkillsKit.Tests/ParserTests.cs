using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Ra.AlexaSkillsKit.Json;
using Ra.AlexaSkillsKit.Directives;

namespace Ra.AlexaSkillsKit.Tests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void Parse_AlexaRequest()
        {
            var requestContent = File.ReadAllText(@"request.json");
            var requestEnvelope = SpeechletRequestEnvelope.FromJson(requestContent);

            Assert.IsNotNull(requestEnvelope);
            Assert.IsNotNull(requestEnvelope.Context);
            Assert.IsNotNull(requestEnvelope.Request);
            Assert.IsNotNull(requestEnvelope.Session);
            Assert.IsNotNull(requestEnvelope.Version);
            Assert.AreEqual(requestEnvelope.Version, Sdk.VERSION);

          
            // intentRequest
            Assert.IsInstanceOfType(requestEnvelope.Request, typeof(IntentRequest));
            var intentRequest = requestEnvelope.Request as IntentRequest;

                    Assert.AreEqual(intentRequest.RequestId, "amzn1.echo-api.request.00000000-0000-0000-0000-000000000000");
            Assert.AreEqual(intentRequest.DialogState, DialogStateEnum.IN_PROGRESS);
            Assert.AreEqual(intentRequest.Timestamp, new DateTime(2017, 1, 2, 3, 4, 5, DateTimeKind.Utc));

            // Intent
            Assert.IsNotNull(intentRequest.Intent);
            Assert.AreEqual(intentRequest.Intent.ConfirmationStatus, ConfirmationStatusEnum.CONFIRMED);
            Assert.AreEqual(intentRequest.Intent.Name, "test");

            // Slots
            Assert.IsNotNull(intentRequest.Intent.Slots);
            Assert.AreEqual(intentRequest.Intent.Slots.Count, 1);
            Assert.IsTrue(intentRequest.Intent.Slots.ContainsKey("Testslot"));

            var slot = intentRequest.Intent.Slots["Testslot"];
            Assert.IsNotNull(slot);

            Assert.AreEqual(slot.Name, "Testslot");
            Assert.AreEqual(slot.Value, "Testvalue");
            Assert.AreEqual(slot.ConfirmationStatus,  ConfirmationStatusEnum.CONFIRMED);

            // resolutions
            Assert.IsNotNull(slot.Resolutions);
            Assert.IsNotNull(slot.Resolutions.ResolutionsPerAuthority);
            Assert.AreEqual(slot.Resolutions.ResolutionsPerAuthority.Count, 1);
            Assert.AreEqual(slot.Resolutions.ResolutionsPerAuthority[0].Authority, "amzn1.er-authority.echo-sdk.amzn1.ask.skill.00000000-0000-0000-0000-000000000000.Testslot");
            Assert.IsNotNull(slot.Resolutions.ResolutionsPerAuthority[0].Status);
            Assert.AreEqual(slot.Resolutions.ResolutionsPerAuthority[0].Status.Code, Resolution.ResolutionStatusCodeEnum.ER_SUCCESS_MATCH);
            Assert.AreEqual(slot.Resolutions.ResolutionsPerAuthority[0].Values.Count, 1);
            Assert.AreEqual(slot.Resolutions.ResolutionsPerAuthority[0].Values[0].Value.Id,  "1");
            Assert.AreEqual(slot.Resolutions.ResolutionsPerAuthority[0].Values[0].Value.Name, "Testvalue");

 
        }
    }
}
