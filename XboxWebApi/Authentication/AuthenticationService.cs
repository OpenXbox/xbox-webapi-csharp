using System;
using System.IO;
using System.Collections.Specialized;
using RestSharp;
using Newtonsoft.Json;
using XboxWebApi.Common;
using XboxWebApi.Authentication.Model;

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
			RestClient client = new RestClient("https://login.live.com");
			RestRequest request = new RestRequest("oauth20_token.srf", Method.GET);
			NameValueCollection nv = new Model.WindowsLiveRefreshQuery(refreshToken).GetQuery();

			foreach (string key in nv)
            {
				request.AddQueryParameter(key, nv[key]);            
            }
			IRestResponse response = client.Execute(request);
			if (!response.IsSuccessful)
			{
				throw new ApiException("RefreshLiveToken failed", response);
			}

			return WindowsLiveResponse.FromJson(response.Content);
		}

		public static UserToken AuthenticateXASU(AccessToken accessToken)
		{
			RestClient client = new RestClient("https://user.auth.xboxlive.com");
			RestRequest request = new RestRequest("user/authenticate", Method.POST);
			request.AddHeader("x-xbl-contract-version", "1");
			request.AddJsonBody(new XASURequest(accessToken));
			IRestResponse response = client.Execute(request);

			if (!response.IsSuccessful)
            {
                throw new ApiException("AuthenticateXASU failed", response);
            }
			XASResponse xasResp = XASResponse.FromJson(response.Content);
			return new UserToken(xasResp);
		}

		public static DeviceToken AuthenticateXASD(AccessToken accessToken)
		{
			RestClient client = new RestClient("https://device.auth.xboxlive.com");
			RestRequest request = new RestRequest("device/authenticate", Method.POST);
            request.AddHeader("x-xbl-contract-version", "1");
            request.AddJsonBody(new XASDRequest(accessToken));
            IRestResponse response = client.Execute(request);
			if (!response.IsSuccessful)
            {
                throw new ApiException("AuthenticateXASD failed", response);
            }

			XASResponse xasResp = XASResponse.FromJson(response.Content);
            return new DeviceToken(xasResp);
		}
        
		public static TitleToken AuthenticateXAST(AccessToken accessToken,
		                                          DeviceToken deviceToken)
		{
			RestClient client = new RestClient("https://title.auth.xboxlive.com");
            RestRequest request = new RestRequest("title/authenticate", Method.POST);
            request.AddHeader("x-xbl-contract-version", "1");
			request.AddJsonBody(new XASTRequest(accessToken, deviceToken));
            IRestResponse response = client.Execute(request);
			if (!response.IsSuccessful)
            {
                throw new ApiException("AuthenticateXAST failed", response);
            }

			XASResponse xasResp = XASResponse.FromJson(response.Content);
            return new TitleToken(xasResp);
		}

		public static XToken AuthenticateXSTS(UserToken userToken,
		                                      DeviceToken deviceToken=null,
		                                      TitleToken titleToken=null)
		{
			RestClient client = new RestClient("https://xsts.auth.xboxlive.com");
			RestRequest request = new RestRequest("xsts/authorize", Method.POST);
            request.AddHeader("x-xbl-contract-version", "1");
			request.AddJsonBody(new XSTSRequest(userToken,
                                                deviceToken: deviceToken,
                                                titleToken: titleToken));
            IRestResponse response = client.Execute(request);
			if (!response.IsSuccessful)
            {
                throw new ApiException("AuthenticateXSTS failed", response);
            }

			XASResponse xasResp = XASResponse.FromJson(response.Content);
            return new XToken(xasResp);
		}
        
		public static string GetWindowsLiveAuthenticationUrl()
		{
			var client = new RestClient("https://login.live.com");
			var request = new RestRequest("oauth20_authorize.srf", Method.GET);
			var nv = new Model.WindowsLiveAuthenticationQuery().GetQuery();
			foreach (string key in nv)
            {
                request.AddQueryParameter(key, nv[key]);
            }
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
    }
}
