using NUnit.Framework;
using XboxWebApi.Authentication;
using XboxWebApi.Authentication.Model;

namespace XboxWebApi.UnitTests.Authentication
{
    [TestFixture]
    public class TestAuthenticationXASRequests
    {
        public TestAuthenticationXASRequests()
        {
        }

        [Test]
        public void CreateRequestXASU()
        {
            AccessToken token = new AccessToken()
            {
                Jwt = "eWaoksdijsdfeefes"
            };
            XASURequest request = new XASURequest(token);
            string body = request.ToJson();

            Assert.AreEqual(body, "{\"RelyingParty\":\"http://auth.xboxlive.com\"," +
                                  "\"TokenType\":\"JWT\"," +
                                  "\"Properties\":{" +
                                  "\"AuthMethod\":\"RPS\"," +
                                  "\"SiteName\":\"user.auth.xboxlive.com\"," +
                                  "\"RpsTicket\":\"eWaoksdijsdfeefes\"}}");
        }

        [Test]
        public void CreateRequestXASD()
        {
            AccessToken token = new AccessToken()
            {
                Jwt = "eWaoksdijsdfeefes"
            };
            XASDRequest request = new XASDRequest(token);
            string body = request.ToJson();

            Assert.AreEqual(body, "{\"RelyingParty\":\"http://auth.xboxlive.com\"," +
                                  "\"TokenType\":\"JWT\"," +
                                  "\"Properties\":{" +
                                  "\"AuthMethod\":\"RPS\"," +
                                  "\"SiteName\":\"user.auth.xboxlive.com\"," +
                                  "\"RpsTicket\":\"eWaoksdijsdfeefes\"}}");
        }

        [Test]
        public void CreateRequestXAST()
        {
            AccessToken accessToken = new AccessToken()
            {
                Jwt = "eWaoksdijsdfeefes"
            };
            DeviceToken deviceToken = new DeviceToken()
            {
                Jwt = "eyajiwjafiassssaw"
            };
            XASTRequest request = new XASTRequest(accessToken, deviceToken);
            string body = request.ToJson();

            Assert.AreEqual(body, "{\"RelyingParty\":\"http://auth.xboxlive.com\"," +
                                  "\"TokenType\":\"JWT\"," +
                                  "\"Properties\":{" +
                                  "\"AuthMethod\":\"RPS\"," +
                                  "\"DeviceToken\":\"eyajiwjafiassssaw\"," +
                                  "\"SiteName\":\"user.auth.xboxlive.com\"," +
                                  "\"RpsTicket\":\"eWaoksdijsdfeefes\"}}");
        }

        [Test]
        public void CreateRequestXSTS()
        {
            UserToken token = new UserToken()
            {
                Jwt = "eWaoksdijsdfeefes"
            };
            XSTSRequest request = new XSTSRequest(token);
            string body = request.ToJson();

            Assert.AreEqual(body, "{\"RelyingParty\":\"http://xboxlive.com\"," +
                                  "\"TokenType\":\"JWT\"," +
                                  "\"Properties\":{" +
                                  "\"UserTokens\":[\"eWaoksdijsdfeefes\"]," +
                                  "\"SandboxId\":\"RETAIL\"}}");
        }
    }
}