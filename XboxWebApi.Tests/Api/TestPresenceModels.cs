using System;
using System.Collections.Generic;
using NUnit.Framework;
using XboxWebApi.Common;
using XboxWebApi.Services.Model;

namespace XboxWebApi.UnitTests.Api
{
    [TestFixture]
    public class TestPresenceModels : TestDataProvider
    {
        public TestPresenceModels()
            : base("ApiPresence")
        {
        }

        [Test]
        public void CreatePresenceQuery()
        {
            PresenceRequestQuery query = new PresenceRequestQuery(PresenceLevel.All);
            Dictionary<string,string> nv = query.GetQuery();

            Assert.IsNotEmpty(nv);
            Assert.AreEqual(1, nv.Count);
            Assert.AreEqual("all", nv["level"]);
        }

        [Test]
        public void CreatePresenceBatchRequestBody()
        {
            ulong[] xuids = new ulong[]{2580478784034343, 2535771801919068, 2508565379774180};
            string expected = TestData["PresenceBatchRequest.json"];
            PresenceBatchRequest request = new PresenceBatchRequest(
                xuids, PresenceLevel.All, onlineOnly: false);
            string body = NewtonsoftJsonSerializer.Create(JsonNamingStrategy.CamelCase)
                .Serialize(request);
            
            Assert.AreEqual(expected, body);
        }

        [Test]
        public void DeserializePresenceResponse()
        {
            string json = TestData["PresenceResponse.json"];
            PresenceResponse response = NewtonsoftJsonSerializer
                .Create(JsonNamingStrategy.CamelCase)
                .Deserialize<PresenceResponse>(json);

            DateTime expectedDate = new DateTime(2018,6,6,19,55,10).AddTicks(5125990);
            Assert.AreEqual(2580478784034343, response.Xuid);
            Assert.AreEqual(PresenceState.Online, response.State);
            Assert.IsNull(response.LastSeen);
            Assert.IsNotNull(response.Devices);
            Assert.AreEqual(1, response.Devices.Length);
            Assert.AreEqual(1, response.Devices[0].Titles.Length);
            Assert.AreEqual(DeviceType.WindowsOneCore, response.Devices[0].Type);
            Assert.AreEqual(328178078, response.Devices[0].Titles[0].Id);
            Assert.AreEqual(TitleViewState.Full, response.Devices[0].Titles[0].Placement);
            Assert.AreEqual("Xbox App", response.Devices[0].Titles[0].Name);
            Assert.AreEqual(expectedDate, response.Devices[0].Titles[0].LastModified);
            Assert.AreEqual(TitleState.Active, response.Devices[0].Titles[0].State);
            Assert.IsNull(response.Devices[0].Titles[0].Activity);
        }

        [Test]
        public void DeserializePresenceBatchResponse()
        {
            string json = TestData["PresenceBatchResponse.json"];
            PresenceBatchResponse response = NewtonsoftJsonSerializer
                .Create(JsonNamingStrategy.CamelCase)
                .Deserialize<PresenceBatchResponse>(json);
            
            DateTime expectedDate1 = new DateTime(2018,6,6,19,38,14).AddTicks(0770062);
            DateTime expectedDate2 = new DateTime(2018,5,30,20,38,4).AddTicks(3994143);

            Assert.AreEqual(3, response.Count);

            Assert.AreEqual(PresenceState.Offline, response[0].State);
            Assert.AreEqual(2580478784034343, response[0].Xuid);
            Assert.IsNull(response[0].Devices);
            Assert.IsNull(response[0].LastSeen);

            Assert.AreEqual(2535771801919068, response[1].Xuid);
            Assert.AreEqual(PresenceState.Online, response[1].State);
            Assert.IsNull(response[1].LastSeen);
            Assert.IsNotNull(response[1].Devices);
            Assert.AreEqual(1, response[1].Devices.Length);
            Assert.AreEqual(2, response[1].Devices[0].Titles.Length);

            Assert.AreEqual(DeviceType.XboxOne, response[1].Devices[0].Type);
            Assert.AreEqual(750323071, response[1].Devices[0].Titles[0].Id);
            Assert.AreEqual(TitleViewState.Background, response[1].Devices[0].Titles[0].Placement);
            Assert.AreEqual("Home", response[1].Devices[0].Titles[0].Name);
            Assert.AreEqual(expectedDate1, response[1].Devices[0].Titles[0].LastModified);
            Assert.AreEqual(TitleState.Active, response[1].Devices[0].Titles[0].State);
            Assert.IsNull(response[1].Devices[0].Titles[0].Activity);

            Assert.AreEqual(2508565379774180, response[2].Xuid);
            Assert.AreEqual(PresenceState.Offline, response[2].State);
            Assert.IsNotNull(response[2].LastSeen);
            Assert.AreEqual(DeviceType.WindowsOneCore, response[2].LastSeen.DeviceType);
            Assert.AreEqual(1653768775, response[2].LastSeen.TitleId);
            Assert.AreEqual("Cities: Skylines", response[2].LastSeen.TitleName);
            Assert.AreEqual(expectedDate2, response[2].LastSeen.Timestamp);
        }
    }
}