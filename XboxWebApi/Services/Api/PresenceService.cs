using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using RestSharp;
using XboxWebApi.Common;
using XboxWebApi.Extensions;
using XboxWebApi.Services.Model;

namespace XboxWebApi.Services.Api
{
    public class PresenceService : XblService
    {
        public PresenceService(IXblConfiguration config, IRestSharpEx httpClient)
            : base(config, "https://userpresence.xboxlive.com", httpClient)
        {
            Headers = new NameValueCollection()
            {
                {"x-xbl-contract-version", "3"}
            };
        }

        public PresenceResponse GetPresence(PresenceLevel level = PresenceLevel.All)
        {
            PresenceRequestQuery query = new PresenceRequestQuery(level);
            RestRequestEx request = new RestRequestEx("users/me", Method.GET);
            request.AddHeaders(Headers);
            request.AddQueryParameters(query.GetQuery());

            IRestResponse<PresenceResponse> response = HttpClient.Execute<PresenceResponse>(request);
            return response.Data;
        }

        public PresenceResponse GetPresence(ulong xuid, PresenceLevel level = PresenceLevel.All)
        {
            PresenceRequestQuery query = new PresenceRequestQuery(level);
            RestRequestEx request = new RestRequestEx($"users/xuid({xuid})", Method.GET);
            request.AddHeaders(Headers);
            request.AddQueryParameters(query.GetQuery());

            IRestResponse<PresenceResponse> response = HttpClient.Execute<PresenceResponse>(request);
            return response.Data;
        }

        public PresenceBatchResponse GetPresenceBatch(ulong[] xuids,
            PresenceLevel level = PresenceLevel.All,
            bool onlineOnly=false)
        {
            PresenceBatchRequest body = new PresenceBatchRequest(xuids, level, onlineOnly);
            RestRequestEx request = new RestRequestEx("users/batch", Method.POST);
            request.AddHeaders(Headers);
            request.AddJsonBody(body, JsonNamingStrategy.CamelCase);

            IRestResponse<PresenceBatchResponse> response = HttpClient.Execute<PresenceBatchResponse>(request);
            return response.Data;
        }
    }
}