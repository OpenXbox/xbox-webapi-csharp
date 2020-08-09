using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using XboxWebApi.Common;
using XboxWebApi.Services.Model;

namespace XboxWebApi.Services.Api
{
    public class ProfileService : XblService
    {
        public ProfileService(IXblConfiguration config)
            : base(config, "https://profile.xboxlive.com/")
        {
            Headers = new Dictionary<string,string>()
            {
                {"x-xbl-contract-version", "2"}
            };
        }

        public async Task<ProfileResponse> GetProfilesBatchAsync(ulong[] xuids, ProfileSetting[] settings = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "users/batch/profile/settings");
            ProfileSetting[] profileSettings = settings != null ? settings : new ProfileSetting[]
            {
                ProfileSetting.AppDisplayName, ProfileSetting.Gamerscore,
                ProfileSetting.Gamertag, ProfileSetting.PublicGamerpic,
                ProfileSetting.XboxOneRep, ProfileSetting.RealName
            };
            ProfilesRequest body = new ProfilesRequest(xuids, profileSettings);
            request.Headers.Add(Headers);
            request.Content = new JsonContent(body, JsonNamingStrategy.CamelCase);

            var response = await HttpClient.SendAsync(request);
            return await response.Content.ReadAsJsonAsync<ProfileResponse>();
        }

        private async Task<ProfileResponse> _GetProfileAsync(string resource, ProfileSetting[] settings = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, resource);
            ProfileSetting[] profileSettings = settings != null ? settings : new ProfileSetting[]
            {
                ProfileSetting.AppDisplayName, ProfileSetting.Gamerscore,
                ProfileSetting.Gamertag, ProfileSetting.PublicGamerpic,
                ProfileSetting.XboxOneRep, ProfileSetting.RealName
            };
            ProfileRequestQuery query = new ProfileRequestQuery(profileSettings);
            request.Headers.Add(Headers);
            request.AddQueryParameter(query.GetQuery());

            var response = await HttpClient.SendAsync(request);
            return await response.Content.ReadAsJsonAsync<ProfileResponse>();
        }

        public async Task<ProfileResponse> GetProfileAsync(ulong xuid, ProfileSetting[] settings = null)
        {
            return await _GetProfileAsync($"users/xuid({xuid})/profile/settings", settings);
        }

        public async Task<ProfileResponse> GetProfileAsync(string gamertag, ProfileSetting[] settings = null)
        {
            return await _GetProfileAsync($"users/gt({gamertag})/profile/settings", settings);
        }
    }
}