using System.IO;
using NUnit.Framework;
using XboxWebApi.Authentication;
using XboxWebApi.Authentication.Model;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace XboxWebApi.UnitTests.Authentication
{
    [TestFixture]
    public class TestAuthenticationDumpLoadTokens : TestDataProvider
    {
        public TestAuthenticationDumpLoadTokens()
            : base("Authentication")
        {
        }

        [Test]
        public void TestLoadTokens()
        {
            string tokens = TestData["TokenDump.json"];
            byte[] tokensBytes = System.Text.Encoding.UTF8.GetBytes(tokens);

            string tmpFilePath = Path.GetTempFileName();
            using(FileStream fs = new FileStream(tmpFilePath, FileMode.Create))
            {
                fs.Write(tokensBytes, 0, tokensBytes.Length);
            }

            AuthenticationService service = null;
            using (FileStream fs = new FileStream(tmpFilePath, FileMode.Open))
            {
                service = AuthenticationService.LoadFromFile(fs);
            }

            Assert.IsNotNull(service.AccessToken);
            Assert.IsNotNull(service.RefreshToken);
            Assert.IsNotNull(service.UserToken);
            Assert.IsNull(service.TitleToken);
            Assert.IsNull(service.DeviceToken);
            Assert.IsNotNull(service.XToken);
            Assert.IsNotNull(service.UserInformation);

            Assert.IsTrue(service.AccessToken.Valid);
            Assert.IsInstanceOf(typeof(string), service.AccessToken.Jwt);
            Assert.IsInstanceOf(typeof(System.DateTime), service.AccessToken.Issued);
            Assert.IsTrue(service.UserToken.Valid);
            Assert.IsFalse(service.XToken.Valid);
        }

        [Test]
        public void TestDumpTokens()
        {
            AuthenticationService service = new AuthenticationService(
                TestConstants.TestAccessToken, TestConstants.TestRefreshToken);

            string tmpFilePath = Path.GetTempFileName();
            using(FileStream fs = new FileStream(tmpFilePath, FileMode.Create))
            {
                service.DumpToFile(fs);
            }

            using(FileStream fs = new FileStream(tmpFilePath, FileMode.Open))
            {
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                string tokenDump = System.Text.Encoding.UTF8.GetString(bytes);
                Dictionary<string, object> deserialized = JsonConvert
                                                    .DeserializeObject<Dictionary<string, object>>(tokenDump);

                Assert.IsNotNull(deserialized["AccessToken"]);
                Assert.IsNotNull(deserialized["RefreshToken"]);
                Assert.IsNull(deserialized["UserToken"]);
                Assert.IsNull(deserialized["TitleToken"]);
                Assert.IsNull(deserialized["DeviceToken"]);
                Assert.IsNull(deserialized["XToken"]);
            }
        }
    }
}