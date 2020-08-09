using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using XboxWebApi.Common;
using XboxWebApi.Services.Model;

namespace XboxWebApi.Services.Api
{
    public class PeopleService : XblService
    {
        public PeopleService(IXblConfiguration config)
            : base(config, "https://social.xboxlive.com/")
        {
            Headers = new Dictionary<string,string>()
            {
                {"x-xbl-contract-version", "1"}
            };
        }

        public async Task<PeopleResponse> GetFriendsAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "users/me/people");
            request.Headers.Add(Headers);

            var response = await HttpClient.SendAsync(request);
            return await response.Content.ReadAsJsonAsync<PeopleResponse>();
        }

        public async Task<PeopleSummaryResponse> GetFriendsSummaryAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "users/me/summary");
            request.Headers.Add(Headers);

            var response = await HttpClient.SendAsync(request);
            return await response.Content.ReadAsJsonAsync<PeopleSummaryResponse>();
        }

        public async Task<PeopleSummaryResponse> GetFriendsSummaryAsync(ulong xuid)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"users/xuid({xuid})/summary");
            request.Headers.Add(Headers);

            var response = await HttpClient.SendAsync(request);
            return await response.Content.ReadAsJsonAsync<PeopleSummaryResponse>();
        }

        public async Task<PeopleSummaryResponse> GetFriendsSummaryAsync(string gamertag)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"users/gt({gamertag})/summary");
            request.Headers.Add(Headers);

            var response = await HttpClient.SendAsync(request);
            return await response.Content.ReadAsJsonAsync<PeopleSummaryResponse>();
        }

        public async Task<PeopleResponse> GetFriendsOwnBatchAsync(ulong[] xuids)
        {
            PeopleBatchRequest body = new PeopleBatchRequest(xuids);
            var request = new HttpRequestMessage(HttpMethod.Post, "users/me/people/xuids");
            request.Headers.Add(Headers);
            request.Content = new JsonContent(body, JsonNamingStrategy.CamelCase);

            var response = await HttpClient.SendAsync(request);
            return await response.Content.ReadAsJsonAsync<PeopleResponse>();
        }
    }
}