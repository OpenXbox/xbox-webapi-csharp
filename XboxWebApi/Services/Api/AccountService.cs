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
        public AccountService(XblConfiguration config)
            : base(config, "https://accounts.xboxlive.com")
        {   
        }

        public AccountResponse GetAccount()
        {
            RestRequestEx request = new RestRequestEx("users/current/profile", Method.GET);
            // No headers
            IRestResponse<AccountResponse> response = ClientFactory(JsonNamingStrategy.CamelCase)
                .Execute<AccountResponse>(request);
            return response.Data;
        }

        public AccountResponse GetFamilyAccount(ulong xuid)
        {
            RestRequestEx request = new RestRequestEx($"family/memberXuid({xuid})", Method.GET);
            // No headers
            IRestResponse<AccountResponse> response = ClientFactory(JsonNamingStrategy.CamelCase)
                .Execute<AccountResponse>(request);
            return response.Data;
        }
    }
}