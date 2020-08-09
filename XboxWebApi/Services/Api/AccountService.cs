using System;
using System.Net.Http;
using System.Threading.Tasks;
using XboxWebApi.Common;
using XboxWebApi.Services.Model.Account;

namespace XboxWebApi.Services.Api
{
    public class AccountService : XblService
    {
        public AccountService(IXblConfiguration config)
            : base(config, "https://accounts.xboxlive.com/")
        {
        }

        public async Task<AccountResponse> GetAccountAsync()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "users/current/profile");
            // No headers
            var response = await HttpClient.SendAsync(request);
            return await response.Content.ReadAsJsonAsync<AccountResponse>();
        }

        public async Task<AccountResponse> GetFamilyAccountAsync(ulong xuid)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"family/memberXuid({xuid})");
            // No headers
            var response = await HttpClient.SendAsync(request);
            return await response.Content.ReadAsJsonAsync<AccountResponse>();
        }
    }
}