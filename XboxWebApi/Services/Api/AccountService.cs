using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using RestSharp;
using XboxWebApi.Common;
using XboxWebApi.Extensions;
using XboxWebApi.Services.Model.Account;

namespace XboxWebApi.Services.Api
{
    public class AccountService : XblService
    {
        public AccountService(IXblConfiguration config, IRestSharpEx httpClient)
            : base(config, "https://accounts.xboxlive.com", httpClient)
        {
        }

        public AccountResponse GetAccount()
        {
            RestRequestEx request = new RestRequestEx("users/current/profile", Method.GET);
            // No headers
            IRestResponse<AccountResponse> response = HttpClient.Execute<AccountResponse>(request);

            return response.Data;
        }

        public AccountResponse GetFamilyAccount(ulong xuid)
        {
            RestRequestEx request = new RestRequestEx($"family/memberXuid({xuid})", Method.GET);
            // No headers
            IRestResponse<AccountResponse> response = HttpClient.Execute<AccountResponse>(request);

            return response.Data;
        }
    }
}