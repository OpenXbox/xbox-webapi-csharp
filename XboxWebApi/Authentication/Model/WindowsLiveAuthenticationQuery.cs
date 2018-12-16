using System;
using System.Collections.Specialized;
using XboxWebApi.Authentication;

namespace XboxWebApi.Authentication.Model
{
    public class WindowsLiveAuthenticationQuery
    {
        public string ResponseType { get; internal set; }
        public string Scope { get; internal set; }
        public string RedirectUri { get; internal set; }
        public string ClientId { get; internal set; }
        public string Display { get; internal set; }
        public string Locale { get; internal set; }

        public WindowsLiveAuthenticationQuery(
            string responseType = "token",
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

        public NameValueCollection GetQuery()
        {
            return new NameValueCollection{
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
