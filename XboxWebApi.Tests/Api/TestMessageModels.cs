using System;
using System.Collections.Generic;
using NUnit.Framework;
using XboxWebApi.Common;
using XboxWebApi.Services.Model;

namespace XboxWebApi.UnitTests.Api
{
    [TestFixture]
    public class TestMessageModels : TestDataProvider
    {
        public TestMessageModels()
            : base("ApiMessage")
        {
        }

        [Test]
        public void CreateMessageInboxRequestQuery()
        {
            MessageInboxRequestQuery query = new MessageInboxRequestQuery(10, 90);
            Dictionary<string,string> nv = query.GetQuery();

            Assert.IsNotEmpty(nv);
            Assert.AreEqual(2, nv.Count);
            Assert.AreEqual("10", nv["skipItems"]);
            Assert.AreEqual("90", nv["maxItems"]);
        }

        [Test]
        public void CreateMessageSendRequest()
        {
            string expectedXuid = TestData["MessageSendMessageXuidRequest.json"];
            string expectedGamertag = TestData["MessageSendMessageGtRequest.json"];

            ulong[] xuids = new ulong[]{2580478784034343, 2535771801919068, 2508565379774180};
            string[] gamertags = new string[]{"Gamertag1", "Gamertag2"};
            MessageSendRequest requestXuid = new MessageSendRequest("TestString", xuids);
            MessageSendRequest requestGt = new MessageSendRequest("TestString", gamertags);
            string bodyXuid = NewtonsoftJsonSerializer.Create(JsonNamingStrategy.CamelCase)
                .Serialize(requestXuid);
            string bodyGt = NewtonsoftJsonSerializer.Create(JsonNamingStrategy.CamelCase)
                .Serialize(requestGt);
            
            Assert.AreEqual(expectedXuid, bodyXuid);
            Assert.AreEqual(expectedGamertag, bodyGt);
        }

        [Test]
        public void DeserializeInboxResponse()
        {
            string json = TestData["MessageInboxResponse.json"];
            MessageInboxResponse response = NewtonsoftJsonSerializer
                .Create(JsonNamingStrategy.CamelCase)
                .Deserialize<MessageInboxResponse>(json);

            DateTime expectedDateSent0 = new DateTime(2018,05,20,20,08,27);
            DateTime expectedDateExpiration0 = new DateTime(2018,06,19,20,08,27);
            DateTime expectedDateSent1 = new DateTime(2018,05,08,01,07,35);
            DateTime expectedDateExpiration1 = new DateTime(2018,06,07,01,07,35);

            Assert.IsNotNull(response.PagingInfo);
            Assert.AreEqual(3, response.PagingInfo.TotalItems);

            Assert.IsNotEmpty(response.Results);
            Assert.AreEqual(3, response.Results.Length);
            
            Assert.IsNotNull(response.Results[0].Header);
            Assert.AreEqual(1191, response.Results[0].Header.Id);
            Assert.IsTrue(response.Results[0].Header.IsRead);
            Assert.AreEqual(2508565379774180, response.Results[0].Header.SenderXuid);
            Assert.AreEqual("Some Gamertag", response.Results[0].Header.Sender);
            Assert.AreEqual(expectedDateSent0, response.Results[0].Header.Sent);
            Assert.AreEqual(expectedDateExpiration0, response.Results[0].Header.Expiration);
            Assert.AreEqual("User", response.Results[0].Header.MessageType);
            Assert.IsTrue(response.Results[0].Header.HasText);
            Assert.IsFalse(response.Results[0].Header.HasPhoto);
            Assert.IsFalse(response.Results[0].Header.HasAudio);
            Assert.AreEqual("Inbox", response.Results[0].Header.MessageFolderType);

            Assert.AreEqual("Hey, how are you?" ,response.Results[0].MessageSummary);
            Assert.IsNull(response.Results[0].Actions);

            Assert.IsNotNull(response.Results[1].Header);
            Assert.AreEqual(1189, response.Results[1].Header.Id);
            Assert.IsTrue(response.Results[1].Header.IsRead);
            Assert.AreEqual(0, response.Results[1].Header.SenderXuid);
            Assert.AreEqual("Xbox Live", response.Results[1].Header.Sender);
            Assert.AreEqual(expectedDateSent1, response.Results[1].Header.Sent);
            Assert.AreEqual(expectedDateExpiration1, response.Results[1].Header.Expiration);
            Assert.AreEqual("Service", response.Results[1].Header.MessageType);
            Assert.IsTrue(response.Results[1].Header.HasText);
            Assert.IsFalse(response.Results[1].Header.HasPhoto);
            Assert.IsFalse(response.Results[1].Header.HasAudio);
            Assert.AreEqual("Inbox", response.Results[1].Header.MessageFolderType);

            Assert.AreEqual("Please remember: Xb" ,response.Results[1].MessageSummary);
            Assert.IsNotNull(response.Results[1].Actions);
            Assert.IsNotEmpty(response.Results[1].Actions);

            Assert.AreEqual(1, response.Results[2].Actions.Length);
            Assert.AreEqual(0, response.Results[2].Actions[0].Id);
            Assert.AreEqual("Redeem code", response.Results[2].Actions[0].VuiGui);
            Assert.IsNull(response.Results[2].Actions[0].VuiAlm);
            Assert.IsNull(response.Results[2].Actions[0].VuiPron);
            Assert.IsNull(response.Results[2].Actions[0].VuiConf);
            Assert.AreEqual("ABCDE-ABCDE-ABCDE-ABCDE-DCC9Z", response.Results[2].Actions[0].Launch);
            Assert.AreEqual(LaunchType.FiveByFive, response.Results[2].Actions[0].LaunchType);

            Assert.AreEqual("Thanks for parti..." ,response.Results[2].MessageSummary);

        }

        [Test]
        public void DeserializeMessageResponse()
        {
            string json = TestData["MessageResponse.json"];
            MessageResponse response = NewtonsoftJsonSerializer
                .Create(JsonNamingStrategy.CamelCase)
                .Deserialize<MessageResponse>(json);
            
            DateTime expectSent = new DateTime(2017, 6, 9, 19, 1, 59);
            DateTime expectExpiration = new DateTime(2017, 7, 9, 19, 1, 59);

            Assert.IsNotNull(response.Header);
            Assert.AreEqual("Here is some full message text, no cut-off text",
                response.MessageText);
            Assert.IsNull(response.AttachmentId);
            Assert.IsNull(response.Attachment);
            Assert.IsNotNull(response.Actions);
            Assert.IsNotEmpty(response.Actions);
            Assert.AreEqual(1, response.Actions.Length);

            Assert.AreEqual(0, response.Header.SenderXuid);
            Assert.AreEqual("Xbox Live", response.Header.Sender);
            Assert.AreEqual("Service", response.Header.MessageType);
            Assert.IsTrue(response.Header.HasText);
            Assert.IsFalse(response.Header.HasPhoto);
            Assert.IsFalse(response.Header.HasAudio);
            Assert.AreEqual("Inbox", response.Header.MessageFolderType);

            Assert.AreEqual(0, response.Actions[0].Id);
            Assert.AreEqual("Launch website", response.Actions[0].VuiGui);
            Assert.IsNull(response.Actions[0].VuiAlm);
            Assert.IsNull(response.Actions[0].VuiPron);
            Assert.IsNull(response.Actions[0].VuiConf);
            Assert.AreEqual("https://xbox.com", response.Actions[0].Launch);
            Assert.AreEqual(LaunchType.DeepLink, response.Actions[0].LaunchType);
        }

        [Test]
        public void DeserializeConversationResponse()
        {
            string json = TestData["MessageConversationResponse.json"];
            ConversationResponse response = NewtonsoftJsonSerializer
                .Create(JsonNamingStrategy.CamelCase)
                .Deserialize<ConversationResponse>(json);

            Assert.IsNotNull(response.Conversation);
            Assert.IsNotNull(response.Conversation.Summary);
            Assert.IsNull(response.Conversation.Summary.LastMessage);
            Assert.IsNotNull(response.Conversation.Messages);
            Assert.IsNotEmpty(response.Conversation.Messages);
            Assert.AreEqual(2, response.Conversation.Messages.Length);
        }

        [Test]
        public void DeserializeConversationsResponse()
        {
            string json = TestData["MessageConversationsResponse.json"];
            ConversationsResponse response = NewtonsoftJsonSerializer
                .Create(JsonNamingStrategy.CamelCase)
                .Deserialize<ConversationsResponse>(json);
            
            Assert.IsNotNull(response.Results);
            Assert.IsNotEmpty(response.Results);
            Assert.AreEqual(2, response.Results.Length);
            Assert.IsNotNull(response.Results[0].LastMessage);
            Assert.IsNotNull(response.Results[1].LastMessage);
            Assert.IsNotEmpty(response.Results[0].LastMessage.Actions);
            Assert.IsNull(response.Results[1].LastMessage.Actions);
        }
    }
}