using System;
using System.Collections.Specialized;

namespace XboxWebApi.Authentication.Model
{
	public class WindowsLiveRefreshQuery
    {
		public string GrantType { get; internal set; }
		public string ClientId { get; internal set; }
		public string Scope { get; internal set; }
		public string RefreshToken { get; internal set; }
        
		public WindowsLiveRefreshQuery(
			RefreshToken refreshToken,
			string grantType="refresh_token",
			string clientId="0000000048093EE3",
			string scope="service::user.auth.xboxlive.com::MBI_SSL")
		{
			GrantType = grantType;
			ClientId = clientId;
			Scope = scope;
			RefreshToken = refreshToken.Jwt;
		}

		public NameValueCollection GetQuery()
		{
			return new NameValueCollection{
			    {"grant_type", GrantType},
				{"client_id", ClientId},
				{"scope", Scope},
				{"refresh_token", RefreshToken}
			};
		}
	}
}
