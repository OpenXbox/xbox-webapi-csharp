using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using XboxWebApi.Common;
using XboxWebApi.Services.Model;

namespace XboxWebApi.Services.Api
{
    public class MessageService : XblService
    {
        public MessageService(IXblConfiguration config)
            : base(config, "https://msg.xboxlive.com/")
        {
            Headers = new Dictionary<string,string>()
            {
                {"x-xbl-contract-version", "1"}
            };
        }

        public async Task<MessageInboxResponse> GetMessagesAsync(int skipItems=0, int maxItems=100)
        {
            MessageInboxRequestQuery query = new MessageInboxRequestQuery(
                skipItems, maxItems);
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"users/xuid({Config.XboxUserId})/inbox");
            request.Headers.Add(Headers);
            request.AddQueryParameter(query.GetQuery());

            var response = await HttpClient.SendAsync(request);
            return await response.Content.ReadAsJsonAsync<MessageInboxResponse>();
        }

        public async Task<MessageResponse> GetMessageAsync(int messageId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"users/xuid({Config.XboxUserId})/inbox/{messageId}");
            request.Headers.Add(Headers);

            var response = await HttpClient.SendAsync(request);
            return await response.Content.ReadAsJsonAsync<MessageResponse>();
        }

        public async Task<ConversationsResponse> GetConversationsAsync(int skipItems=0, int maxItems=100)
        {
            MessageInboxRequestQuery query = new MessageInboxRequestQuery(
                skipItems, maxItems);
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"users/xuid({Config.XboxUserId})/inbox/conversations");
            request.Headers.Add(Headers);
            request.AddQueryParameter(query.GetQuery());

            var response = await HttpClient.SendAsync(request);
            return await response.Content.ReadAsJsonAsync<ConversationsResponse>();
        }

        public async Task<ConversationResponse> GetConversationAsync(ulong xuid)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"users/xuid({Config.XboxUserId})/inbox/conversations/xuid({xuid})");
            request.Headers.Add(Headers);

            var response = await HttpClient.SendAsync(request);
            return await response.Content.ReadAsJsonAsync<ConversationResponse>();
        }

        public async Task DeleteMessageAsync(int messageId)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete,
                $"users/xuid({Config.XboxUserId})/inbox/{messageId}");
            request.Headers.Add(Headers);

            var response = await HttpClient.SendAsync(request);
        }

        private async Task SendMessageAsync(MessageSendRequest postData)
        {
            var request = new HttpRequestMessage(HttpMethod.Post,
                $"users/xuid({Config.XboxUserId})/outbox");
            request.Headers.Add(Headers);
            request.Content = new JsonContent(postData, JsonNamingStrategy.CamelCase);

            var response = await HttpClient.SendAsync(request);
        }

        public async Task SendMessageAsync(string messageText, ulong[] xuids)
        {
            MessageSendRequest postData = new MessageSendRequest(messageText, xuids);
            await SendMessageAsync(postData);
        }

        public async Task SendMessageAsync(string messageText, string[] gamertags)
        {
            MessageSendRequest postData = new MessageSendRequest(messageText, gamertags);
            await SendMessageAsync(postData);
        }

        public async Task SendMessageAsync(string messageText, string gamertag)
        {
            await SendMessageAsync(messageText, new string[]{gamertag});
        }

        public async Task SendMessageAsync(string messageText, ulong xuid)
        {
            await SendMessageAsync(messageText, new ulong[]{xuid});
        }
    }
}