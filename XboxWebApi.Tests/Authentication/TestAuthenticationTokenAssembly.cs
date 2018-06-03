using System;
using System.IO;
using System.Collections.Generic;
using NUnit.Framework;
using XboxWebApi.Authentication;
using XboxWebApi.Authentication.Model;

namespace XboxWebApi.UnitTests.Authentication
{
    [TestFixture]
    public class TestAuthenticationTokenAssembly
    {
        public TestAuthenticationTokenAssembly()
        {
        }

        [Test]
        public void AssembleToken()
        {
            string content = "{\"IssueInstant\":\"2014-09-20T18:41:08.0602402Z\"," +
                              "\"NotAfter\":\"2014-09-21T18:41:08.0602402Z\"," +
                              "\"Token\":\"eyWTF/bdf+sd34ji234kasdf34asfs==\"," +
                              "\"DisplayClaims\":{\"xui\":[{\"agg\":\"Adult\"," +
                              "\"gtg\":\"xboxWebapiGamertag\"," +
                              "\"prv\":\"191 193 196 199 200 201\"," +
                              "\"xid\":\"234568092345979\"," +
                              "\"uhs\":\"162358993400365622\"," +
                              "\"usr\":\"123\"}]}}";
            XASResponse xasResponse = XASResponse.FromJson(content);
            XToken token = new XToken(xasResponse);

            Assert.AreEqual(token.Issued, new DateTime(2014, 9, 20, 18, 41, 8).AddTicks(602402));
            Assert.AreEqual(token.Expires, new DateTime(2014, 9, 21, 18, 41, 8).AddTicks(602402));
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