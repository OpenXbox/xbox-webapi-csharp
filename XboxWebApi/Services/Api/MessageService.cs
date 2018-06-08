using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using RestSharp;
using XboxWebApi.Common;
using XboxWebApi.Extensions;
using XboxWebApi.Services.Model;

namespace XboxWebApi.Services.Api
{
    public class MessageService : XblService
    {
        public MessageService(XblConfiguration config)
            : base(config, "https://msg.xboxlive.com")
        {
            Headers = new NameValueCollection()
            {
                {"x-xbl-contract-version", "1"}
            };
        }

        public MessageInboxResponse GetMessages(int skipItems=0, int maxItems=100)
        {
            MessageInboxRequestQuery query = new MessageInboxRequestQuery(
                skipItems, maxItems);
            RestRequestEx request = new RestRequestEx(
                $"users/xuid({Config.XboxUserId})/inbox", Method.GET);
            request.AddHeaders(Headers);
            request.AddQueryParameters(query.GetQuery());
            IRestResponse<MessageInboxResponse> response = ClientFactory(JsonNamingStrategy.CamelCase)
                .Execute<MessageInboxResponse>(request);
            return response.Data;
        }

        public MessageResponse GetMessage(int messageId)
        {
            RestRequestEx request = new RestRequestEx(
                $"users/xuid({Config.XboxUserId})/inbox/{messageId}", Method.GET);
            request.AddHeaders(Headers);
            IRestResponse<MessageResponse> response = ClientFactory(JsonNamingStrategy.CamelCase)
                .Execute<MessageResponse>(request);
            return response.Data;
        }

        public ConversationsResponse GetConversations(int skipItems=0, int maxItems=100)
        {
            MessageInboxRequestQuery query = new MessageInboxRequestQuery(
                skipItems, maxItems);
            RestRequestEx request = new RestRequestEx(
                $"users/xuid({Config.XboxUserId})/inbox/conversations", Method.GET);
            request.AddHeaders(Headers);
            request.AddQueryParameters(query.GetQuery());
            IRestResponse<ConversationsResponse> response = ClientFactory(JsonNamingStrategy.CamelCase)
                .Execute<ConversationsResponse>(request);
            return response.Data;
        }

        public ConversationResponse GetConversation(ulong xuid)
        {
            RestRequestEx request = new RestRequestEx(
                $"users/xuid({Config.XboxUserId})/inbox/conversations/xuid({xuid})", Method.GET);
            request.AddHeaders(Headers);
            IRestResponse<ConversationResponse> response = ClientFactory(JsonNamingStrategy.CamelCase)
                .Execute<ConversationResponse>(request);
            return response.Data;
        }

        public void DeleteMessage(int messageId)
        {
            RestRequestEx request = new RestRequestEx(
                $"users/xuid({Config.XboxUserId})/inbox/{messageId}", Method.DELETE);
            request.AddHeaders(Headers);
            IRestResponse response = ClientFactory(JsonNamingStrategy.CamelCase)
                .Execute(request);
        }

        private void SendMessage(MessageSendRequest postData)
        {
            RestRequestEx request = new RestRequestEx(
                $"users/xuid({Config.XboxUserId})/outbox", Method.POST);
            request.AddHeaders(Headers);
            request.AddJsonBody(postData, JsonNamingStrategy.CamelCase);
            IRestResponse response = ClientFactory(JsonNamingStrategy.CamelCase)
                .Execute(request);
        }

        public void SendMessage(string messageText, ulong[] xuids)
        {
            MessageSendRequest postData = new MessageSendRequest(messageText, xuids);
            SendMessage(postData);
        }

        public void SendMessage(string messageText, string[] gamertags)
        {
            MessageSendRequest postData = new MessageSendRequest(messageText, gamertags);
            SendMessage(postData);
        }

        public void SendMessage(string messageText, string gamertag)
        {
            SendMessage(messageText, new string[]{gamertag});
        }

        public void SendMessage(string messageText, ulong xuid)
        {
            SendMessage(messageText, new ulong[]{xuid});
        }
    }
}