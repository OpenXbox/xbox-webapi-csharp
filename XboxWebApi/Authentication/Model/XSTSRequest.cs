using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace XboxWebApi.Authentication.Model
{
    public partial class XSTSRequest
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
				{"UserTokens", new List<string>(){userToken.Jwt}},
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

	public partial class XSTSRequest
    {
        public static XSTSRequest FromJson(string json) =>
            JsonConvert.DeserializeObject<XSTSRequest>(
                json, Common.JsonSetting.StandardSetting());
    }

	public static class XSTSSerialize
	{
		public static string ToJson(this XSTSRequest self) =>
			JsonConvert.SerializeObject(
			self, Common.JsonSetting.StandardSetting());
    }
}
