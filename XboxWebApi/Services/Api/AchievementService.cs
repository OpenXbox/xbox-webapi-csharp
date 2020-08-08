using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using XboxWebApi.Common;
using XboxWebApi.Services.Model.Achievement;

namespace XboxWebApi.Services.Api
{
    public class AchievementService : XblService
    {
        private Dictionary<string,string> Headers_XONE;
        private Dictionary<string,string> Headers_X360;

        public AchievementService(IXblConfiguration config)
            : base(config, "https://achievements.xboxlive.com/")
        {
            Headers_X360 = new Dictionary<string,string>(){
                {"x-xbl-contract-version", "1"}
            };

            Headers_XONE = new Dictionary<string,string>(){
                {"x-xbl-contract-version", "2"}
            };
        }

        public async Task<HttpResponseMessage> GetAchievementDetailAsync(ulong xuid, string scid, string achievementId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"users/xuid({xuid})/achievements/{scid}/{achievementId}");
            request.Headers.Add(Headers_XONE);
            
            var response = await HttpClient.SendAsync(request);
            return response;
        }

        public async Task<HttpResponseMessage> GetAchievementGameprogressAsync(ulong xuid, ulong titleId)
        {
            AchievementRequestQuery query = new AchievementRequestQuery(titleId);
            var request = new HttpRequestMessage(HttpMethod.Get, $"users/xuid({xuid})/achievements");
            request.Headers.Add(Headers_XONE);
            request.AddQueryParameter(query.GetQuery());
            
            var response = await HttpClient.SendAsync(request);
            return response;
        }

        public async Task<HttpResponseMessage> GetAchievementsRecentProgressAsync(ulong xuid)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"users/xuid({xuid})/history/titles");
            request.Headers.Add(Headers_XONE);

            var response = await HttpClient.SendAsync(request);
            return response;
        }

        public async Task<HttpResponseMessage> GetAchievementsXbox360AllAsync(ulong xuid, ulong titleId)
        {
            AchievementRequestQuery query = new AchievementRequestQuery(titleId);
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"users/xuid({xuid})/titleachievements");
            request.Headers.Add(Headers_X360);
            request.AddQueryParameter(query.GetQuery());
            
            var response = await HttpClient.SendAsync(request);
            return response;
        }

        public async Task<HttpResponseMessage> GetAchievementsXbox360EarnedAsync(ulong xuid, ulong titleId)
        {
            AchievementRequestQuery query = new AchievementRequestQuery(titleId);
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"users/xuid({xuid})/achievements");
            request.Headers.Add(Headers_X360);
            request.AddQueryParameter(query.GetQuery());

            var response = await HttpClient.SendAsync(request);
            return response;
        }

        public async Task<HttpResponseMessage> GetAchievementsXbox360RecentProgressAsync(ulong xuid)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"users/xuid({xuid})/history/titles");
            request.Headers.Add(Headers_X360);

            var response = await HttpClient.SendAsync(request);
            return response;
        }
    }
}