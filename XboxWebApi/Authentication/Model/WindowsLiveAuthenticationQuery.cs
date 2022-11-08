using System;
using System.Collections.Generic;

namespace XboxWebApi.Authentication.Model
{
    public class WindowsLiveAuthenticationQuery : Common.IHttpRequestQuery
    {
        public string ResponseType { get; internal set; }
        public string Scope { get; internal set; }
        public string RedirectUri { get; internal set; }
        public string ClientId { get; internal set; }
        public string Display { get; internal set; }
        public string Locale { get; internal set; }

        public WindowsLiveAuthenticationQuery(
            string responseType = WindowsLiveConstants.ResponseType,
            string scope = WindowsLiveConstants.Scope,
            string redirectUri = WindowsLiveConstants.RedirectUrl,
            string clientId = WindowsLiveConstants.ClientId,
            string display = "touch", string locale = "en")
        {
            ResponseType = responseType;
            Scope = scope;
            RedirectUri = redirectUri;
            ClientId = clientId;
            Display = display;
            Locale = locale;
        }

        public Dictionary<string,string> GetQuery()
        {
            return new Dictionary<string,string>{
                {"response_type", ResponseType},
                {"scope", Scope},
                {"redirect_uri", RedirectUri},
                {"client_id", ClientId},
                {"display", Display},
                {"locale", Locale}
            };
        }
    }
}
