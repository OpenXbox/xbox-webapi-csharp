using System;
using Newtonsoft.Json;

namespace XboxWebApi.Authentication.Model
{
	public class XASDRequest
	{
        public string RelyingParty { get; internal set; }
        public string TokenType { get; internal set; }
        public XASDProperties Properties { get; internal set; }

		public XASDRequest(AccessToken accessToken,
		                   string relyingParty="http://auth.xboxlive.com",
		                   string tokenType="JWT",
		                   string authMethod="RPS",
		                   string siteName="user.auth.xboxlive.com")
		{
			RelyingParty = relyingParty;
			TokenType = tokenType;
			Properties = new XASDProperties()
			{
				AuthMethod = authMethod,
				SiteName = siteName,
				RpsTicket = accessToken.Jwt
			};
		}
	}

	public class XASDProperties
    {
		public string AuthMethod { get; internal set; }
		public string SiteName { get; internal set; }
		public string RpsTicket { get; internal set; }
    }
}