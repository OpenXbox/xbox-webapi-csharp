using System;
using System.IO;
using System.Collections.Generic;
using NUnit.Framework;
using XboxWebApi.Authentication;
using XboxWebApi.Authentication.Model;

namespace XboxWebApi.UnitTests.Authentication
{
    [TestFixture]
    public class TestAuthenticationTokenParsing
    {
        public TestAuthenticationTokenParsing()
        {
        }

        [Test]
        public void ParseXASToken()
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
            XASResponse token = XASResponse.FromJson(content);

            Assert.AreEqual(token.IssueInstant, new DateTime(2014, 9, 20, 18, 41, 8).AddTicks(602402));
            Assert.AreEqual(token.NotAfter, new DateTime(2014, 9, 21, 18, 41, 8).AddTicks(602402));
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
            string content = "{\"IssueInstant\":\"2014-09-20T18:41:08.0602402Z\"," +
                              "\"NotAfter\":\"2014-09-21T18:41:08.0602402Z\"," +
                              "\"Token\":\"eyWTF/bdf+sd34ji234kasdf34asfs==\"}";

            XASResponse token = XASResponse.FromJson(content);
            Assert.AreEqual(token.IssueInstant, new DateTime(2014, 9, 20, 18, 41, 8).AddTicks(602402));
            Assert.AreEqual(token.NotAfter, new DateTime(2014, 9, 21, 18, 41, 8).AddTicks(602402));
            Assert.AreEqual(token.Token, "eyWTF/bdf+sd34ji234kasdf34asfs==");
            Assert.IsNull(token.DisplayClaims);

        }

        [Test]
        public void ParseXASInvalidContent()
        {
            string content = "{\"error\":\"Some error occured\"}";
            XASResponse token = XASResponse.FromJson(content);

            Assert.IsNull(token.Token);
            Assert.AreEqual(token.IssueInstant, new DateTime());
            Assert.AreEqual(token.NotAfter,  new DateTime());
            Assert.IsNull(token.DisplayClaims);            
        }
    }
}