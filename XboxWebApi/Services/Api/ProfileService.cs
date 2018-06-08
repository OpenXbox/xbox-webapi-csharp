using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using RestSharp;
using XboxWebApi.Common;
using XboxWebApi.Extensions;
using XboxWebApi.Services.Model;

namespace XboxWebApi.Services.Api
{
    public class ProfileService : XblService
    {
        public ProfileService(XblConfiguration config)
            : base(config, "https://profile.xboxlive.com")
        {
            Headers = new NameValueCollection()
            {
                {"x-xbl-contract-version", "2"}
            };
        }

        public ProfileResponse GetProfilesBatch(ulong[] xuids, ProfileSetting[] settings = null)
        {
            RestRequestEx request = new RestRequestEx("users/batch/profile/settings",
                Method.POST);
            ProfileSetting[] profileSettings = settings != null ? settings : new ProfileSetting[]
            {
                ProfileSetting.AppDisplayName, ProfileSetting.Gamerscore,
                ProfileSetting.Gamertag, ProfileSetting.PublicGamerpic,
                ProfileSetting.XboxOneRep, ProfileSetting.RealName
            };
            ProfilesRequest body = new ProfilesRequest(xuids, profileSettings);
            request.AddHeaders(Headers);
            request.AddJsonBody(body, JsonNamingStrategy.CamelCase);
            IRestResponse<ProfileResponse> response = Client(JsonNamingStrategy.CamelCase)
                .Execute<ProfileResponse>(request);
            return response.Data;
        }

        private ProfileResponse _GetProfile(string resource, ProfileSetting[] settings = null)
        {
            RestRequestEx request = new RestRequestEx(resource, Method.GET);
            ProfileSetting[] profileSettings = settings != null ? settings : new ProfileSetting[]
            {
                ProfileSetting.AppDisplayName, ProfileSetting.Gamerscore,
                ProfileSetting.Gamertag, ProfileSetting.PublicGamerpic,
                ProfileSetting.XboxOneRep, ProfileSetting.RealName
            };
            ProfileRequestQuery query = new ProfileRequestQuery(profileSettings);
            request.AddHeaders(Headers);
            request.AddQueryParameters(query.GetQuery());
            IRestResponse<ProfileResponse> response = Client(JsonNamingStrategy.CamelCase)
                .Execute<ProfileResponse>(request);
            return response.Data;
        }

        public ProfileResponse GetProfile(ulong xuid, ProfileSetting[] settings = null)
        {
            return _GetProfile($"users/xuid({xuid})/profile/settings", settings);
        }

        public ProfileResponse GetProfile(string gamertag, ProfileSetting[] settings = null)
        {
            return _GetProfile($"users/gt({gamertag})/profile/settings", settings);
        }
    }
}