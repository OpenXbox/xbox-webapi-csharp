using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using RestSharp;
using XboxWebApi.Common;
using XboxWebApi.Extensions;
using XboxWebApi.Services.Model;

namespace XboxWebApi.Services.Api
{
    public class PeopleService : XblService
    {
        public PeopleService(XblConfiguration config)
            : base(config, "https://social.xboxlive.com")
        {
            Headers = new NameValueCollection()
            {
                {"x-xbl-contract-version", "1"}
            };
        }

        public PeopleResponse GetFriends()
        {
            RestRequestEx request = new RestRequestEx("users/me/people", Method.GET);
            request.AddHeaders(Headers);
            IRestResponse<PeopleResponse> response = Client(JsonNamingStrategy.CamelCase)
                .Execute<PeopleResponse>(request);
            return response.Data;
        }

        public PeopleSummaryResponse GetFriendsSummary()
        {
            RestRequestEx request = new RestRequestEx("users/me/summary", Method.GET);
            request.AddHeaders(Headers);
            IRestResponse<PeopleSummaryResponse> response = Client(JsonNamingStrategy.CamelCase)
                .Execute<PeopleSummaryResponse>(request);
            return response.Data;
        }

        public PeopleSummaryResponse GetFriendsSummary(ulong xuid)
        {
            RestRequestEx request = new RestRequestEx($"users/xuid({xuid})/summary", Method.GET);
            request.AddHeaders(Headers);
            IRestResponse<PeopleSummaryResponse> response = Client(JsonNamingStrategy.CamelCase)
                .Execute<PeopleSummaryResponse>(request);
            return response.Data;
        }

        public PeopleSummaryResponse GetFriendsSummary(string gamertag)
        {
            RestRequestEx request = new RestRequestEx($"users/gt({gamertag})/summary", Method.GET);
            request.AddHeaders(Headers);
            IRestResponse<PeopleSummaryResponse> response = Client(JsonNamingStrategy.CamelCase)
                .Execute<PeopleSummaryResponse>(request);
            return response.Data;
        }

        public PeopleResponse GetFriendsOwnBatch(ulong[] xuids)
        {
            PeopleBatchRequest body = new PeopleBatchRequest(xuids);
            RestRequestEx request = new RestRequestEx("users/me/people/xuids", Method.POST);
            request.AddHeaders(Headers);
            request.AddJsonBody(body, JsonNamingStrategy.CamelCase);
            IRestResponse<PeopleResponse> response = Client(JsonNamingStrategy.CamelCase)
                .Execute<PeopleResponse>(request);
            return response.Data;
        }
    }
}