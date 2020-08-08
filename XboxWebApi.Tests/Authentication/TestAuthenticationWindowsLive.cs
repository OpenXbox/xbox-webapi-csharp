using System.IO;
using NUnit.Framework;
using XboxWebApi.Authentication;
using XboxWebApi.Authentication.Model;

namespace XboxWebApi.UnitTests.Authentication
{
    [TestFixture]
    public class TestAuthenticationWindowsLive : TestDataProvider
    {
        public TestAuthenticationWindowsLive()
            : base("Authentication")
        {
        }

        [Test]
        public void GenerateWindowsLiveAuthenticationUrl()
        {
            string expectedUrl = TestData["WindowsLiveAuthUrl.url"];
            string authenticationUrl = AuthenticationService.GetWindowsLiveAuthenticationUrl();

            Assert.AreEqual(authenticationUrl, expectedUrl);
        }

        [Test]
        public void ParseWindowsLiveResponseSuccess()
        {
            string responseUrl = TestData["WindowsLiveRedirectionUrl.url"];
            System.DateTime dateBeforeParsing = System.DateTime.Now;
            WindowsLiveResponse response = AuthenticationService.ParseWindowsLiveResponse(responseUrl);

            Assert.IsNotNull(response);

            AccessToken accessToken = new AccessToken(response);
            RefreshToken refreshToken = new RefreshToken(response);

            Assert.IsFalse(accessToken.HasUserInformation);
            Assert.IsNull(accessToken.UserInformation);
            Assert.IsTrue(accessToken.Valid);
            Assert.AreEqual(accessToken.Jwt, "EwAAA+pvBAAUKods63Ys1fGlwiccIFJ+9u");
            Assert.GreaterOrEqual(accessToken.Issued, dateBeforeParsing);
            Assert.GreaterOrEqual(accessToken.Expires, accessToken.Issued);
            
            Assert.IsFalse(refreshToken.HasUserInformation);
            Assert.IsNull(refreshToken.UserInformation);
            Assert.IsTrue(refreshToken.Valid);
            Assert.AreEqual(refreshToken.Jwt, "MCdhvIzN1f!FoKyCigwGbM$$");
            Assert.GreaterOrEqual(refreshToken.Issued, dateBeforeParsing);
            Assert.GreaterOrEqual(refreshToken.Expires, refreshToken.Issued);

            Assert.Greater(refreshToken.Expires, accessToken.Expires);
        }

        [Test]
        public void ParseWindowsLiveResponseInvalidUrl()
        {
            string invalidUrl = "https://login.live.com/oauth20_invalid.srf?param=value";

            InvalidDataException ex = Assert.Throws<InvalidDataException>(
                delegate {AuthenticationService.ParseWindowsLiveResponse(invalidUrl);});
            
            Assert.IsTrue(ex.Message.StartsWith("Invalid URL to parse"));
        }

        [Test]
        public void ParseWindowsLiveResponseNoFragment()
        {
            string invalidUrlFragment = "https://login.live.com/oauth20_desktop.srf?lc=1337";

            InvalidDataException ex = Assert.Throws<InvalidDataException>(
                delegate {AuthenticationService.ParseWindowsLiveResponse(invalidUrlFragment);});
            
            Assert.IsTrue(ex.Message.StartsWith("Invalid URL fragment"));
        }

        [Test]
        public void ParseWindowsLiveResponseIncompleteQuery()
        {
            string responseUrl = "https://login.live.com/oauth20_desktop.srf?" +
                                 "lc=1033" +
                                 "#access_token=EwAAA%2bpvBAAUKods63Ys1fGlwiccIFJ%2b9u";

            InvalidDataException ex = Assert.Throws<InvalidDataException>(
                delegate {AuthenticationService.ParseWindowsLiveResponse(responseUrl);});
            Assert.IsTrue(ex.Message.StartsWith("Key not found"));
        }
    }
}