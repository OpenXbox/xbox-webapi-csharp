using System;
using System.IO;
using System.Collections.Specialized;
using RestSharp;
using Newtonsoft.Json;
using XboxWebApi.Common;
using XboxWebApi.Extensions;
using XboxWebApi.Authentication.Model;
using System.Text;

namespace XboxWebApi.Authentication
{
	public class AuthenticationService
	{
		public AccessToken AccessToken { get; internal set; }
		public RefreshToken RefreshToken { get; internal set; }
		public UserToken UserToken { get; internal set; }
		public DeviceToken DeviceToken { get; internal set; }
		public TitleToken TitleToken { get; internal set; }
		public XToken XToken { get; internal set; }
		public XboxUserInformation UserInformation { get; internal set; }

		public AuthenticationService(AccessToken accessToken, RefreshToken refreshToken)
		{
			AccessToken = accessToken;
			RefreshToken = refreshToken;
		}

		public static RestClientEx ClientFactory(string baseUrl,
			JsonNamingStrategy naming = JsonNamingStrategy.Default)
		{
			return new RestClientEx(baseUrl, naming);
		}

		public bool Authenticate()
		{
			WindowsLiveResponse windowsLiveTokens = RefreshLiveToken(RefreshToken);
            AccessToken = new AccessToken(windowsLiveTokens);
            RefreshToken = new RefreshToken(windowsLiveTokens);
            UserToken = AuthenticateXASU(AccessToken);
			XToken = AuthenticateXSTS(UserToken, DeviceToken, TitleToken);
			UserInformation = XToken.UserInformation;
			return true;
		}

		public static WindowsLiveResponse RefreshLiveToken(
			RefreshToken refreshToken)
		{
			RestClientEx client = ClientFactory("https://login.live.com",
				JsonNamingStrategy.SnakeCase);
			RestRequestEx request = new RestRequestEx("oauth20_token.srf", Method.GET);
			NameValueCollection nv = new Model.WindowsLiveRefreshQuery(refreshToken).GetQuery();
			request.AddQueryParameters(nv);
			IRestResponse<WindowsLiveResponse> response = client.Execute<WindowsLiveResponse>(request);
			return response.Data;
		}

		public static UserToken AuthenticateXASU(AccessToken accessToken)
		{
			RestClientEx client = ClientFactory("https://user.auth.xboxlive.com");
			RestRequestEx request = new RestRequestEx("user/authenticate", Method.POST);
			request.AddHeader("x-xbl-contract-version", "1");
			request.AddJsonBody(new XASURequest(accessToken));
			IRestResponse<XASResponse> response = client.Execute<XASResponse>(request);
			return new UserToken(response.Data);
		}

		public static DeviceToken AuthenticateXASD(AccessToken accessToken)
		{
			RestClientEx client = ClientFactory("https://device.auth.xboxlive.com");
			RestRequestEx request = new RestRequestEx("device/authenticate", Method.POST);
            request.AddHeader("x-xbl-contract-version", "1");
            request.AddJsonBody(new XASDRequest(accessToken));
			IRestResponse<XASResponse> response = client.Execute<XASResponse>(request);
            return new DeviceToken(response.Data);
		}
        
		public static TitleToken AuthenticateXAST(AccessToken accessToken,
		                                          DeviceToken deviceToken)
		{
			RestClientEx client = ClientFactory("https://title.auth.xboxlive.com");
            RestRequestEx request = new RestRequestEx("title/authenticate", Method.POST);
            request.AddHeader("x-xbl-contract-version", "1");
			request.AddJsonBody(new XASTRequest(accessToken, deviceToken));
			IRestResponse<XASResponse> response = client.Execute<XASResponse>(request);
            return new TitleToken(response.Data);
		}

		public static XToken AuthenticateXSTS(UserToken userToken,
		                                      DeviceToken deviceToken=null,
		                                      TitleToken titleToken=null)
		{
			RestClientEx client = ClientFactory("https://xsts.auth.xboxlive.com");
			RestRequestEx request = new RestRequestEx("xsts/authorize", Method.POST);
            request.AddHeader("x-xbl-contract-version", "1");
			request.AddJsonBody(new XSTSRequest(userToken,
                                                deviceToken: deviceToken,
                                                titleToken: titleToken));
			IRestResponse<XASResponse> response = client.Execute<XASResponse>(request);
            return new XToken(response.Data);
		}
        
		public static string GetWindowsLiveAuthenticationUrl()
		{
			RestClientEx client = ClientFactory("https://login.live.com");
			RestRequestEx request = new RestRequestEx("oauth20_authorize.srf", Method.GET);
			NameValueCollection nv = new Model.WindowsLiveAuthenticationQuery().GetQuery();
			request.AddQueryParameters(nv);
			return client.BuildUri(request).ToString();
		}

		public static WindowsLiveResponse ParseWindowsLiveResponse(string url)
		{
			if (!url.StartsWith("https://login.live.com/oauth20_desktop.srf"))
			{
				throw new InvalidDataException(String.Format("Invalid URL to parse: {0}", url));
			}

			string urlFragment = new Uri(url).Fragment;
			if (String.IsNullOrEmpty(urlFragment) || !urlFragment.StartsWith("#access_token"))
			{
				throw new InvalidDataException(String.Format("Invalid URL fragment: {0}", urlFragment));
			}

			// Cut off leading '#'
			urlFragment = urlFragment.Substring(1);

			NameValueCollection queryParams = System.Web.HttpUtility.ParseQueryString(
				urlFragment, System.Text.Encoding.UTF8);

			string[] expectedKeys = {
				"expires_in", "access_token", "token_type",
				"scope", "refresh_token", "user_id"};

			foreach (string key in expectedKeys)
			{
				string val = queryParams[key];
				if (String.IsNullOrEmpty(val))
					throw new InvalidDataException(
						String.Format("Key not found: {0} || Invalid value: {1}", key, val));
			}

			return new WindowsLiveResponse(queryParams);
		}

		public static AuthenticationService LoadFromFile(FileStream fs)
		{
			byte[] buf = new byte[fs.Length];
			fs.Read(buf, 0, buf.Length);
			string s = Encoding.UTF8.GetString(buf);
			return (AuthenticationService)JsonConvert.DeserializeObject(s);
		}

		public void DumpToFile(FileStream fs)
		{
			string s = JsonConvert.SerializeObject(this, Formatting.Indented);
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			fs.Write(bytes, 0, bytes.Length);
		}
    }
}
