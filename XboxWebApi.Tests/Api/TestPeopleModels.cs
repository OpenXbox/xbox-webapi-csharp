using System;
using System.Collections.Specialized;
using NUnit.Framework;
using XboxWebApi.Common;
using XboxWebApi.Extensions;
using XboxWebApi.Services.Model;

namespace XboxWebApi.UnitTests.Api
{
    [TestFixture]
    public class TestPeopleModels : TestDataProvider
    {
        public TestPeopleModels()
            : base("ApiPeople")
        {
        }

        [Test]
        public void CreatePeopleBatchRequestBody()
        {
            ulong[] xuids = new ulong[]{2580478784034343, 2535771801919068, 2508565379774180};
            string expected = TestData["PeopleBatchRequest.json"];
            PeopleBatchRequest request = new PeopleBatchRequest(xuids);
            string body = NewtonsoftJsonSerializer.Create(JsonNamingStrategy.CamelCase)
                .Serialize(request);
            
            Assert.AreEqual(expected, body);
        }

        [Test]
        public void DeserializePeopleResponse()
        {
            string json = TestData["PeopleResponse.json"];
            PeopleResponse response = NewtonsoftJsonSerializer
                .Create(JsonNamingStrategy.CamelCase)
                .Deserialize<PeopleResponse>(json);

            DateTime expectedDate0 = new DateTime(2017,08,05,17,03,51).AddTicks(7279382);
            DateTime expectedDate1 = new DateTime(2017,07,22,12,55,43).AddTicks(7301922);
            DateTime expectedDate2 = new DateTime(2013,07,01,15,10,28).AddTicks(4600000);
            Assert.IsInstanceOf(typeof(IStringable), response);
            Assert.AreEqual(3, response.TotalCount);

            Assert.IsNotEmpty(response.People);
            Assert.AreEqual(3, response.People.Length);

            Assert.IsFalse(response.People[0].IsKnown);
            Assert.IsTrue(response.People[0].IsFavorite);
            Assert.IsTrue(response.People[0].IsFollowedByCaller);
            Assert.IsTrue(response.People[0].IsFollowingCaller);
            Assert.IsFalse(response.People[0].IsUnfollowingFeed);
            Assert.AreEqual(2580478784034343, response.People[0].Xuid);
            Assert.AreEqual(expectedDate0, response.People[0].AddedDateTimeUtc);
            Assert.IsNotNull(response.People[0].SocialNetworks);
            Assert.IsEmpty(response.People[0].SocialNetworks);

            Assert.IsFalse(response.People[1].IsKnown);
            Assert.IsTrue(response.People[1].IsFavorite);
            Assert.IsTrue(response.People[1].IsFollowedByCaller);
            Assert.IsTrue(response.People[1].IsFollowingCaller);
            Assert.IsFalse(response.People[1].IsUnfollowingFeed);
            Assert.AreEqual(2535771801919068, response.People[1].Xuid);
            Assert.AreEqual(expectedDate1, response.People[1].AddedDateTimeUtc);
            Assert.IsNotNull(response.People[1].SocialNetworks);
            Assert.IsEmpty(response.People[1].SocialNetworks);

            Assert.IsFalse(response.People[2].IsKnown);
            Assert.IsTrue(response.People[2].IsFavorite);
            Assert.IsTrue(response.People[2].IsFollowedByCaller);
            Assert.IsTrue(response.People[2].IsFollowingCaller);
            Assert.IsFalse(response.People[2].IsUnfollowingFeed);
            Assert.AreEqual(2508565379774180, response.People[2].Xuid);
            Assert.AreEqual(expectedDate2, response.People[2].AddedDateTimeUtc);
            Assert.IsNotNull(response.People[2].SocialNetworks);
            Assert.IsNotEmpty(response.People[2].SocialNetworks);
            Assert.AreEqual(1, response.People[2].SocialNetworks.Length);
            Assert.AreEqual("LegacyXboxLive", response.People[2].SocialNetworks[0]);
        }

        [Test]
        public void DeserializePeopleSummaryResponse()
        {
            string json = TestData["PeopleSummaryResponse.json"];
            PeopleSummaryResponse response = NewtonsoftJsonSerializer
                .Create(JsonNamingStrategy.CamelCase)
                .Deserialize<PeopleSummaryResponse>(json);

            Assert.IsInstanceOf(typeof(IStringable), response);
            Assert.IsFalse(response.IsTargetFollowingCaller);
            Assert.IsFalse(response.HasCallerMarkedTargetAsFavorite);
            Assert.IsTrue(response.IsCallerFollowingTarget);
            Assert.AreEqual("None", response.LegacyFriendStatus);
            Assert.IsFalse(response.HasCallerMarkedTargetAsKnown);
            Assert.AreEqual(94, response.TargetFollowingCount);
            Assert.AreEqual(3747535, response.TargetFollowerCount);
        }
    }
}