using System.IO;
using NUnit.Framework;
using XboxWebApi.Authentication;
using XboxWebApi.Authentication.Model;

namespace XboxWebApi.UnitTests.Authentication
{
    [TestFixture]
    public class TestAuthenticationWindowsLive
    {
        public TestAuthenticationWindowsLive()
        {
        }

        [Test]
        public void GenerateWindowsLiveAuthenticationUrl()
        {
            string expectedUrl = "https://login.live.com/oauth20_authorize.srf?" +
                                 "response_type=token" +
                                 "&scope=service::user.auth.xboxlive.com::MBI_SSL" +
                                 "&redirect_uri=https:%2f%2flogin.live.com%2foauth20_desktop.srf" +
                                 "&client_id=0000000048093EE3" +
                                 "&display=touch" +
                                 "&locale=en";
            string authenticationUrl = AuthenticationService.GetWindowsLiveAuthenticationUrl();

            Assert.AreEqual(authenticationUrl, expectedUrl);
        }

        [Test]
        public void ParseWindowsLiveResponseSuccess()
        {
            System.DateTime dateBeforeParsing = System.DateTime.Now;
            string responseUrl = "https://login.live.com/oauth20_desktop.srf?" +
                                 "lc=1033" +
                                 "#access_token=EwAAA%2bpvBAAUKods63Ys1fGlwiccIFJ%2b9u" +
                                 "&token_type=bearer" +
                                 "&expires_in=86400" +
                                 "&scope=service::user.auth.xboxlive.com::MBI_SSL" +
                                 "&refresh_token=MCdhvIzN1f!FoKyCigwGbM%24%24" +
                                 "&user_id=100abefbdea232";
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