using System;
using System.IO;
using System.Collections.Generic;
using NUnit.Framework;
using XboxWebApi.Common;
using XboxWebApi.Authentication;
using XboxWebApi.Authentication.Model;

namespace XboxWebApi.UnitTests.Authentication
{
    [TestFixture]
    public class TestAuthenticationTokenParsing : TestDataProvider
    {
        public TestAuthenticationTokenParsing()
            : base("Authentication")
        {
        }

        [Test]
        public void ParseXASToken()
        {
            string content = TestData["XTokenValid.json"];
            XASResponse token = NewtonsoftJsonSerializer.Default
                .Deserialize<XASResponse>(content);

            Assert.AreEqual(token.IssueInstant, new DateTime(2014, 9, 20, 18, 41, 8).AddTicks(602402));
            Assert.AreEqual(token.NotAfter, new DateTime(2099, 9, 21, 18, 41, 8).AddTicks(602402));
            Assert.AreEqual(token.Token, "eyWTF/bdf+sd34ji234kasdf34asfs==");
            Assert.IsNotNull(token.DisplayClaims);

            Assert.AreEqual(token.DisplayClaims.Count, 1);
            Assert.IsInstanceOf(typeof(Dictionary<string, List<XboxUserInformation>>), token.DisplayClaims);
            Assert.IsNotNull(token.DisplayClaims["xui"]);
            Assert.IsInstanceOf(typeof(List<XboxUserInformation>), token.DisplayClaims["xui"]);
            Assert.AreEqual(token.DisplayClaims["xui"].Count, 1);

            Assert.AreEqual(token.DisplayClaims["xui"][0].AgeGroup, "Adult");
            Assert.AreEqual(token.DisplayClaims["xui"][0].Gamertag, "xboxWebapiGamertag");
            Assert.AreEqual(token.DisplayClaims["xui"][0].Privileges, "191 193 196 199 200 201");
            Assert.AreEqual(token.DisplayClaims["xui"][0].Userhash, "162358993400365622");
            Assert.AreEqual(token.DisplayClaims["xui"][0].UserSettingsRestrictions, "123");
            Assert.AreEqual(token.DisplayClaims["xui"][0].XboxUserId, 234568092345979);
        }

        [Test]
        public void ParseXASTokenNoUserinfo()
        {
            string content = TestData["TokenNoUserinfo.json"];
            XASResponse token = NewtonsoftJsonSerializer.Default
                .Deserialize<XASResponse>(content);
            Assert.AreEqual(token.IssueInstant, new DateTime(2014, 9, 20, 18, 41, 8).AddTicks(602402));
            Assert.AreEqual(token.NotAfter, new DateTime(2014, 9, 21, 18, 41, 8).AddTicks(602402));
            Assert.AreEqual(token.Token, "eyWTF/bdf+sd34ji234kasdf34asfs==");
            Assert.IsNull(token.DisplayClaims);

        }

        [Test]
        public void ParseXASInvalidContent()
        {
            string content = TestData["InvalidData.json"];
            XASResponse token = NewtonsoftJsonSerializer.Default
                .Deserialize<XASResponse>(content);

            Assert.IsNull(token.Token);
            Assert.AreEqual(token.IssueInstant, new DateTime());
            Assert.AreEqual(token.NotAfter,  new DateTime());
            Assert.IsNull(token.DisplayClaims);            
        }
    }
}