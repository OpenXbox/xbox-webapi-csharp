using System;
using System.IO;
using System.Collections.Specialized;
using NUnit.Framework;
using XboxWebApi.Common;
using XboxWebApi.Authentication;
using XboxWebApi.Authentication.Model;

namespace XboxWebApi.UnitTests.Authentication
{
    [TestFixture]
    public class TestRefreshTokenWindowsLive : TestDataProvider
    {
        public TestRefreshTokenWindowsLive()
            : base("Authentication")
        {
        }

        [Test]
        public void CreateWindowsLiveTokenRefreshQuery()
        {
            RefreshToken dummyToken = new RefreshToken(){
                Jwt = "eyAbcdef.Jwt"
            };
            WindowsLiveRefreshQuery refreshQuery = new WindowsLiveRefreshQuery(dummyToken);
            
            Assert.AreEqual(refreshQuery.ClientId, "0000000048093EE3");
            Assert.AreEqual(refreshQuery.GrantType, "refresh_token");
            Assert.AreEqual(refreshQuery.RefreshToken, "eyAbcdef.Jwt");
            Assert.AreEqual(refreshQuery.Scope, "service::user.auth.xboxlive.com::MBI_SSL");
        }

        [Test]
        public void BuildWindowsLiveTokenRefreshQuery()
        {
            RefreshToken dummyToken = new RefreshToken(){
                Jwt = "eyAbcdef.Jwt"
            };
            WindowsLiveRefreshQuery refreshQuery = new WindowsLiveRefreshQuery(dummyToken);
            NameValueCollection queryParams = refreshQuery.GetQuery();

            Assert.AreEqual(queryParams.Count, 4);
            Assert.AreEqual(queryParams["client_id"], "0000000048093EE3");
            Assert.AreEqual(queryParams["grant_type"], "refresh_token");
            Assert.AreEqual(queryParams["refresh_token"], "eyAbcdef.Jwt");
            Assert.AreEqual(queryParams["scope"], "service::user.auth.xboxlive.com::MBI_SSL");
            Assert.IsNull(queryParams["ClientId"]);
            Assert.IsNull(queryParams["GrantType"]);
            Assert.IsNull(queryParams["RefreshToken"]);
        }

        [Test]
        public void ParseValidRefreshToken()
        {
            string content = TestData["RefreshToken.json"];
            WindowsLiveResponse response = NewtonsoftJsonSerializer.Create(JsonNamingStrategy.SnakeCase)
                .Deserialize<WindowsLiveResponse>(content);

            Assert.AreEqual(response.TokenType, "bearer");
            Assert.AreEqual(response.ExpiresIn, 86400);
            Assert.AreEqual(response.Scope, "service::user.auth.xboxlive.com::MBI_SSL");
            Assert.AreEqual(response.AccessToken, "EWCAA/bdf+sd34ji234kasdf34asfs==");
            Assert.AreEqual(response.RefreshToken, "CuZ*4TX7!SAF33cW*kzdFLPRcz0DtU$$");
            Assert.AreEqual(response.UserId, "a42bdc501731723e");
        }

        [Test]
        public void ParseInvalidRefreshToken()
        {
            string content = TestData["InvalidData.json"];
            WindowsLiveResponse response = NewtonsoftJsonSerializer.Create(JsonNamingStrategy.SnakeCase)
                .Deserialize<WindowsLiveResponse>(content);

            Assert.AreEqual(response.ExpiresIn, 0);
            Assert.IsNull(response.TokenType);
            Assert.IsNull(response.Scope);
            Assert.IsNull(response.AccessToken);
            Assert.IsNull(response.RefreshToken);
            Assert.IsNull(response.UserId);
        }
    }
}