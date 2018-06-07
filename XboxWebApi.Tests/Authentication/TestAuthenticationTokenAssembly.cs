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
    public class TestAuthenticationTokenAssembly : TestDataProvider
    {
        public TestAuthenticationTokenAssembly()
            : base("Authentication")
        {
        }

        [Test]
        public void AssembleToken()
        {
            string content = TestData["XTokenValid.json"];
            XASResponse xasResponse = NewtonsoftJsonSerializer.Default
                .Deserialize<XASResponse>(content);
            XToken token = new XToken(xasResponse);

            Assert.AreEqual(token.Issued, new DateTime(2014, 9, 20, 18, 41, 8).AddTicks(602402));
            Assert.AreEqual(token.Expires, new DateTime(2099, 9, 21, 18, 41, 8).AddTicks(602402));
            Assert.AreEqual(token.Jwt, "eyWTF/bdf+sd34ji234kasdf34asfs==");
            Assert.IsNotNull(token.UserInformation);

            Assert.AreEqual(token.UserInformation.AgeGroup, "Adult");
            Assert.AreEqual(token.UserInformation.Gamertag, "xboxWebapiGamertag");
            Assert.AreEqual(token.UserInformation.Privileges, "191 193 196 199 200 201");
            Assert.AreEqual(token.UserInformation.Userhash, "162358993400365622");
            Assert.AreEqual(token.UserInformation.UserSettingsRestrictions, "123");
            Assert.AreEqual(token.UserInformation.XboxUserId, 234568092345979);
        }
    }
}