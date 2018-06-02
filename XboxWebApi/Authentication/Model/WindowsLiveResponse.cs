using System;
using System.Collections.Specialized;
using Newtonsoft.Json;

namespace XboxWebApi.Authentication.Model
{
	public partial class WindowsLiveResponse
	{        
		public string AccessToken;
		public int ExpiresIn;
		public string RefreshToken;
		public string UserId;
		public string Scope;
		public string TokenType;

		// Not part of actual response data
		public DateTime CreationTime { get; private set; }

		public WindowsLiveResponse()
		{
			CreationTime = DateTime.Now;
		}
		public WindowsLiveResponse(NameValueCollection queryParams)
		{
			CreationTime = DateTime.Now;

			ExpiresIn = int.Parse(queryParams["expires_in"]);
			AccessToken = queryParams["access_token"];
			TokenType = queryParams["token_type"];
			Scope = queryParams["scope"];
			RefreshToken = queryParams["refresh_token"];
			UserId = queryParams["user_id"];         
		}
	}

	public partial class WindowsLiveResponse
	{
		public static WindowsLiveResponse FromJson(string json) =>
		    JsonConvert.DeserializeObject<WindowsLiveResponse>(
			    json, Common.JsonSetting.SnakeCaseSetting());
	}
}
