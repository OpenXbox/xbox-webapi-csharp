using NUnit.Framework;
using XboxWebApi.Common;
using XboxWebApi.Authentication;
using XboxWebApi.Authentication.Model;

namespace XboxWebApi.UnitTests.Authentication
{
    [TestFixture]
    public class TestAuthenticationXASRequests : TestDataProvider
    {
        public TestAuthenticationXASRequests()
            : base("Authentication")
        {
        }

        [Test]
        public void CreateRequestXASU()
        {
            string expect = TestData["XASURequestBody.json"];
            AccessToken token = new AccessToken()
            {
                Jwt = "eWaoksdijsdfeefes"
            };
            XASURequest request = new XASURequest(token);
            string body = NewtonsoftJsonSerializer.Default.Serialize(request);

            Assert.AreEqual(body, expect);
        }

        [Test]
        public void CreateRequestXASD()
        {
            string expect = TestData["XASDRequestBody.json"];
            AccessToken token = new AccessToken()
            {
                Jwt = "eWaoksdijsdfeefes"
            };
            XASDRequest request = new XASDRequest(token);
            string body = NewtonsoftJsonSerializer.Default.Serialize(request);

            Assert.AreEqual(body, expect);
        }

        [Test]
        public void CreateRequestXAST()
        {
            string expect = TestData["XASTRequestBody.json"];
            AccessToken accessToken = new AccessToken()
            {
                Jwt = "eWaoksdijsdfeefes"
            };
            DeviceToken deviceToken = new DeviceToken()
            {
                Jwt = "eyajiwjafiassssaw"
            };
            XASTRequest request = new XASTRequest(accessToken, deviceToken);
            string body = NewtonsoftJsonSerializer.Default.Serialize(request);

            Assert.AreEqual(body, expect);
        }

        [Test]
        public void CreateRequestXSTS()
        {
            string expect = TestData["XSTSRequestBody.json"];
            UserToken token = new UserToken()
            {
                Jwt = "eWaoksdijsdfeefes"
            };
            XSTSRequest request = new XSTSRequest(token);
            string body = NewtonsoftJsonSerializer.Default.Serialize(request);

            Assert.AreEqual(body, expect);
        }
    }
}