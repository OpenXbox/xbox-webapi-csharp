using System;
using Newtonsoft.Json;

namespace XboxWebApi.Authentication.Model
{
    public class XASURequest
    {
		public string RelyingParty { get; internal set; }
        public string TokenType { get; internal set; }
        public XASUProperties Properties { get; internal set; }

		public XASURequest(AccessToken accessToken,
		                   string relyingParty="http://auth.xboxlive.com",
		                   string tokenType="JWT",
		                   string authMethod="RPS",
		                   string siteName="user.auth.xboxlive.com")
        {
			RelyingParty = relyingParty;
			TokenType = tokenType;
			Properties = new XASUProperties()
			{
				AuthMethod = authMethod,
                SiteName = siteName,
				RpsTicket = accessToken.Jwt
			};
        }
    }

	public class XASUProperties
    {
        public string AuthMethod { get; internal set; }
        public string SiteName { get; internal set; }
        public string RpsTicket { get; internal set; }
    }
}
