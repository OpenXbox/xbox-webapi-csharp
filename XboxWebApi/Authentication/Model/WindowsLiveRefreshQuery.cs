using System;
using System.Collections.Generic;

namespace XboxWebApi.Authentication.Model
{
    public class WindowsLiveRefreshQuery : Common.IHttpRequestQuery
    {
        public string GrantType { get; internal set; }
        public string ClientId { get; internal set; }
        public string Scope { get; internal set; }
        public string RefreshToken { get; internal set; }

        public WindowsLiveRefreshQuery(
            RefreshToken refreshToken,
            string grantType = "refresh_token",
            string clientId = WindowsLiveConstants.ClientId,
            string scope = WindowsLiveConstants.Scope)
        {
            GrantType = grantType;
            ClientId = clientId;
            Scope = scope;
            RefreshToken = refreshToken.Jwt;
        }

        public Dictionary<string,string> GetQuery()
        {
            return new Dictionary<string,string>{
                {"grant_type", GrantType},
                {"client_id", ClientId},
                {"scope", Scope},
                {"refresh_token", RefreshToken}
            };
        }
    }
}
