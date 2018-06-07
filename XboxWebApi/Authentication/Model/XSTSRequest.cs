using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace XboxWebApi.Authentication.Model
{
    public class XSTSRequest
    {
		public string RelyingParty { get; internal set; }
        public string TokenType { get; internal set; }
		public Dictionary<string, object> Properties { get; internal set; }

		public XSTSRequest(UserToken userToken,
						   string relyingParty = "http://xboxlive.com",
						   string tokenType = "JWT",
						   string sandboxId = "RETAIL",
						   DeviceToken deviceToken = null,
						   TitleToken titleToken = null)
		{
			RelyingParty = relyingParty;
			TokenType = tokenType;
			Properties = new Dictionary<string, object>
			{
				{"UserTokens", new string[]{userToken.Jwt}},
				{"SandboxId", sandboxId}
			};
            
			if (deviceToken != null)
			{
				Properties.Add("DeviceToken", deviceToken.Jwt);
			}

			if (titleToken != null)
			{
				Properties.Add("TitleToken", titleToken.Jwt);
			}
        }
    }
}
