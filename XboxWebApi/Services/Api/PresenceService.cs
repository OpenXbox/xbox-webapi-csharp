using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using XboxWebApi.Common;
using XboxWebApi.Services.Model;

namespace XboxWebApi.Services.Api
{
    public class PresenceService : XblService
    {
        public PresenceService(IXblConfiguration config)
            : base(config, "https://userpresence.xboxlive.com/")
        {
            Headers = new Dictionary<string,string>()
            {
                {"x-xbl-contract-version", "3"}
            };
        }

        public async Task<PresenceResponse> GetPresenceAsync(PresenceLevel level = PresenceLevel.All)
        {
            PresenceRequestQuery query = new PresenceRequestQuery(level);
            var request = new HttpRequestMessage(HttpMethod.Get, "users/me");
            request.Headers.Add(Headers);
            request.AddQueryParameter(query.GetQuery());

            var response = await HttpClient.SendAsync(request);
            return await response.Content.ReadAsJsonAsync<PresenceResponse>();
        }

        public async Task<PresenceResponse> GetPresenceAsync(ulong xuid, PresenceLevel level = PresenceLevel.All)
        {
            PresenceRequestQuery query = new PresenceRequestQuery(level);
            var request = new HttpRequestMessage(HttpMethod.Get, $"users/xuid({xuid})");
            request.Headers.Add(Headers);
            request.AddQueryParameter(query.GetQuery());

            var response = await HttpClient.SendAsync(request);
            return await response.Content.ReadAsJsonAsync<PresenceResponse>();
        }

        public async Task<PresenceBatchResponse> GetPresenceBatchAsync(ulong[] xuids,
            PresenceLevel level = PresenceLevel.All,
            bool onlineOnly=false)
        {
            PresenceBatchRequest body = new PresenceBatchRequest(xuids, level, onlineOnly);
            var request = new HttpRequestMessage(HttpMethod.Post, "users/batch");
            request.Headers.Add(Headers);
            request.Content = new JsonContent(body, JsonNamingStrategy.CamelCase);

            var response = await HttpClient.SendAsync(request);
            return await response.Content.ReadAsJsonAsync<PresenceBatchResponse>();
        }
    }
}