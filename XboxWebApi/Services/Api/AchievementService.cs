using System;
using System.Collections.Specialized;
using RestSharp;
using XboxWebApi.Extensions;
using XboxWebApi.Services.Model.Achievement;

namespace XboxWebApi.Services.Api
{
    public class AchievementService : XblService
    {
        private NameValueCollection Headers_XONE;
        private NameValueCollection Headers_X360;

        public AchievementService(IXblConfiguration config, IRestSharpEx httpClient)
            : base(config, "https://achievements.xboxlive.com", httpClient)
        {
            Headers_X360 = new NameValueCollection(){
                {"x-xbl-contract-version", "1"}
            };

            Headers_XONE = new NameValueCollection(){
                {"x-xbl-contract-version", "2"}
            };
        }

        public void GetAchievementDetail(ulong xuid, string scid, string achievementId)
        {
            RestRequestEx request = new RestRequestEx(
                $"users/xuid({xuid})/achievements/{scid}/{achievementId}", Method.GET);
            request.AddHeaders(Headers_XONE);
            
            IRestResponse response = HttpClient.Execute(request);
            Console.WriteLine(response.Content);
        }

        public void GetAchievementGameprogress(ulong xuid, ulong titleId)
        {
            AchievementRequestQuery query = new AchievementRequestQuery(titleId);
            RestRequestEx request = new RestRequestEx(
                $"users/xuid({xuid})/achievements", Method.GET);
            request.AddHeaders(Headers_XONE);
            request.AddQueryParameters(query.GetQuery());
            
            IRestResponse response = HttpClient.Execute(request);
            Console.WriteLine(response.Content);
        }

        public void GetAchievementsRecentProgress(ulong xuid)
        {
            RestRequestEx request = new RestRequestEx(
                $"users/xuid({xuid})/history/titles", Method.GET);
            request.AddHeaders(Headers_XONE);

            IRestResponse response = HttpClient.Execute(request);
            Console.WriteLine(response.Content);
        }

        public void GetAchievementsXbox360All(ulong xuid, ulong titleId)
        {
            AchievementRequestQuery query = new AchievementRequestQuery(titleId);
            RestRequestEx request = new RestRequestEx(
                $"users/xuid({xuid})/titleachievements", Method.GET);
            request.AddHeaders(Headers_X360);
            request.AddQueryParameters(query.GetQuery());
            
            IRestResponse response = HttpClient.Execute(request);
            Console.WriteLine(response.Content);
        }

        public void GetAchievementsXbox360Earned(ulong xuid, ulong titleId)
        {
            AchievementRequestQuery query = new AchievementRequestQuery(titleId);
            RestRequestEx request = new RestRequestEx(
                $"users/xuid({xuid})/achievements", Method.GET);
            request.AddHeaders(Headers_X360);
            request.AddQueryParameters(query.GetQuery());

            IRestResponse response = HttpClient.Execute(request);
            Console.WriteLine(response.Content);
        }

        public void GetAchievementsXbox360RecentProgress(ulong xuid)
        {
            RestRequestEx request = new RestRequestEx(
                $"users/xuid({xuid})/history/titles", Method.GET);
            request.AddHeaders(Headers_X360);

            IRestResponse response = HttpClient.Execute(request);
            Console.WriteLine(response.Content);
        }
    }
}