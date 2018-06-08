using System;
using System.Collections.Specialized;
using NUnit.Framework;
using XboxWebApi.Common;
using XboxWebApi.Extensions;
using XboxWebApi.Services.Model;

namespace XboxWebApi.UnitTests.Api
{
    [TestFixture]
    public class TestProfileModels : TestDataProvider
    {
        public TestProfileModels()
            : base("ApiProfile")
        {
        }

        [Test]
        public void CreateProfileRequestQuery()
        {
            ProfileRequestQuery query = new ProfileRequestQuery(new ProfileSetting[]
            {
                ProfileSetting.AccountTier,
                ProfileSetting.GameDisplayName,
                ProfileSetting.PublicGamerpic
            });
            NameValueCollection nv = query.GetQuery();

            Assert.IsNotEmpty(nv);
            Assert.AreEqual(1, nv.Count);
            Assert.AreEqual("AccountTier,GameDisplayName,PublicGamerpic", nv["settings"]);
        }

        [Test]
        public void DeserializeProfileResponse()
        {
            string json = TestData["ProfileResponse.json"];
            ProfileResponse response = NewtonsoftJsonSerializer
                .Create(JsonNamingStrategy.CamelCase)
                .Deserialize<ProfileResponse>(json);
            
            Assert.IsInstanceOf(typeof(IStringable), response);

            Assert.IsNotNull(response.ProfileUsers);
            Assert.IsNotEmpty(response.ProfileUsers);
            Assert.AreEqual(1, response.ProfileUsers.Length);

            Assert.AreEqual(2580478784034343, response.ProfileUsers[0].Id);
            Assert.AreEqual(2580478784034343, response.ProfileUsers[0].HostId);
            Assert.IsFalse(response.ProfileUsers[0].IsSponsoredUser);
            Assert.IsNotNull(response.ProfileUsers[0].Settings);
            Assert.IsNotEmpty(response.ProfileUsers[0].Settings);
            Assert.AreEqual(6, response.ProfileUsers[0].Settings.Length);

            Assert.AreEqual(ProfileSetting.AppDisplayName, response.ProfileUsers[0].Settings[0].Id);
            Assert.AreEqual("Some Gamertag", response.ProfileUsers[0].Settings[0].Value);
            Assert.AreEqual(ProfileSetting.Gamerscore, response.ProfileUsers[0].Settings[1].Id);
            Assert.AreEqual("1337000", response.ProfileUsers[0].Settings[1].Value);
            Assert.AreEqual(ProfileSetting.Gamertag, response.ProfileUsers[0].Settings[2].Id);
            Assert.AreEqual("Some Gamertag", response.ProfileUsers[0].Settings[2].Value);
            Assert.AreEqual(ProfileSetting.PublicGamerpic, response.ProfileUsers[0].Settings[3].Id);
            Assert.AreEqual("http://images-eds.xboxlive.com/image?url=abcdef",
                response.ProfileUsers[0].Settings[3].Value);
            Assert.AreEqual(ProfileSetting.XboxOneRep, response.ProfileUsers[0].Settings[4].Id);
            Assert.AreEqual("Superstar", response.ProfileUsers[0].Settings[4].Value);
            Assert.AreEqual(ProfileSetting.RealName, response.ProfileUsers[0].Settings[5].Id);
            Assert.AreEqual("John Doe", response.ProfileUsers[0].Settings[5].Value);
        }
    }
}