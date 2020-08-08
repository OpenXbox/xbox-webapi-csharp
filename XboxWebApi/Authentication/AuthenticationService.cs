using System;
using System.IO;
using System.Collections.Specialized;
using RestSharp;
using Newtonsoft.Json;
using XboxWebApi.Common;
using XboxWebApi.Extensions;
using XboxWebApi.Authentication.Model;
using System.Text;
using System.Threading.Tasks;

namespace XboxWebApi.Authentication
{
    public class AuthenticationService
    {
        public AccessToken AccessToken { get; set; }
        public RefreshToken RefreshToken { get; set; }
        public UserToken UserToken { get; set; }
        public DeviceToken DeviceToken { get; set; }
        public TitleToken TitleToken { get; set; }
        public XToken XToken { get; set; }
        public XboxUserInformation UserInformation { get; set; }

        public AuthenticationService()
        {
        }

        public AuthenticationService(WindowsLiveResponse wlResponse)
        {
            AccessToken = new AccessToken(wlResponse);
            RefreshToken = new RefreshToken(wlResponse);
        }

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
            return AuthenticateAsync().GetAwaiter().GetResult();
        }

        public async Task<bool> AuthenticateAsync()
        {
            WindowsLiveResponse windowsLiveTokens = await RefreshLiveTokenAsync(RefreshToken);
            //make sure that the access token is actually present in the token refresh before trying to change it
            if (!string.IsNullOrWhiteSpace(windowsLiveTokens.AccessToken))
            {
                AccessToken = new AccessToken(windowsLiveTokens);
            }
            //make sure that the refresh token is actually present in the token refresh before trying to change it
            if (!string.IsNullOrWhiteSpace(windowsLiveTokens.RefreshToken))
            {
                RefreshToken = new RefreshToken(windowsLiveTokens);
            }
            UserToken = await AuthenticateXASUAsync(AccessToken);
            XToken = await AuthenticateXSTSAsync(UserToken, DeviceToken, TitleToken);
            UserInformation = XToken.UserInformation;
            return true;
        }

        public static WindowsLiveResponse RefreshLiveToken(RefreshToken token)
        {
            return RefreshLiveTokenAsync(token).GetAwaiter().GetResult();
        }

        public static async Task<WindowsLiveResponse> RefreshLiveTokenAsync(
            RefreshToken refreshToken)
        {
            RestClientEx client = ClientFactory("https://login.live.com",
                JsonNamingStrategy.SnakeCase);
            RestRequestEx request = new RestRequestEx("oauth20_token.srf", Method.GET);
            NameValueCollection nv = new Model.WindowsLiveRefreshQuery(refreshToken).GetQuery();
            request.AddQueryParameters(nv);
            IRestResponse<WindowsLiveResponse> response = await client.ExecuteTaskAsync<WindowsLiveResponse>(request);
            return response.Data;
        }

        public static UserToken AuthenticateXASU(AccessToken accessToken)
        {
            return AuthenticateXASUAsync(accessToken).GetAwaiter().GetResult();
        }

        public static async Task<UserToken> AuthenticateXASUAsync(AccessToken accessToken)
        {
            RestClientEx client = ClientFactory("https://user.auth.xboxlive.com");
            RestRequestEx request = new RestRequestEx("user/authenticate", Method.POST);
            request.AddHeader("x-xbl-contract-version", "1");
            request.AddJsonBody(new XASURequest(accessToken));
            IRestResponse<XASResponse> response = await client.ExecuteTaskAsync<XASResponse>(request);
            return new UserToken(response.Data);
        }

        public DeviceToken AuthenticateXASD(AccessToken accessToken)
        {
            return AuthenticateXASDAsync(accessToken).GetAwaiter().GetResult();
        }

        public static async Task<DeviceToken> AuthenticateXASDAsync(AccessToken accessToken)
        {
            RestClientEx client = ClientFactory("https://device.auth.xboxlive.com");
            RestRequestEx request = new RestRequestEx("device/authenticate", Method.POST);
            request.AddHeader("x-xbl-contract-version", "1");
            request.AddJsonBody(new XASDRequest(accessToken));
            IRestResponse<XASResponse> response = await client.ExecuteTaskAsync<XASResponse>(request);
            return new DeviceToken(response.Data);
        }

        public static TitleToken AuthenticateXAST(AccessToken accessToken, DeviceToken deviceToken)
        {
            return AuthenticateXASTAsync(accessToken, deviceToken).GetAwaiter().GetResult();
        }

        public static async Task<TitleToken> AuthenticateXASTAsync(AccessToken accessToken,
                                                  DeviceToken deviceToken)
        {
            RestClientEx client = ClientFactory("https://title.auth.xboxlive.com");
            RestRequestEx request = new RestRequestEx("title/authenticate", Method.POST);
            request.AddHeader("x-xbl-contract-version", "1");
            request.AddJsonBody(new XASTRequest(accessToken, deviceToken));
            IRestResponse<XASResponse> response = await client.ExecuteTaskAsync<XASResponse>(request);
            return new TitleToken(response.Data);
        }

        public static XToken AuthenticateXSTS(UserToken userToken,
                                              DeviceToken deviceToken = null,
                                              TitleToken titleToken = null)
        {
            return AuthenticateXSTSAsync(userToken, deviceToken, titleToken).GetAwaiter().GetResult();
        }

        public static async Task<XToken> AuthenticateXSTSAsync(UserToken userToken,
                                              DeviceToken deviceToken = null,
                                              TitleToken titleToken = null)
        {
            RestClientEx client = ClientFactory("https://xsts.auth.xboxlive.com");
            RestRequestEx request = new RestRequestEx("xsts/authorize", Method.POST);
            request.AddHeader("x-xbl-contract-version", "1");
            request.AddJsonBody(new XSTSRequest(userToken,
                                                deviceToken: deviceToken,
                                                titleToken: titleToken));
            IRestResponse<XASResponse> response = await client.ExecuteTaskAsync<XASResponse>(request);
            return new XToken(response.Data);
        }

        public static string GetWindowsLiveAuthenticationUrl()
        {
            RestClientEx client = ClientFactory("https://login.live.com");
            RestRequestEx request = new RestRequestEx("oauth20_authorize.srf", Method.GET);
            NameValueCollection nv = new Model.WindowsLiveAuthenticationQuery().GetQuery();
            request.AddQueryParameters(nv);
            string url = client.BuildUri(request).ToString();

            return System.Web.HttpUtility.UrlDecode(url);
        }

        public static WindowsLiveResponse ParseWindowsLiveResponse(string url)
        {
            if (!url.StartsWith(WindowsLiveConstants.RedirectUrl))
            {
                throw new InvalidDataException(String.Format("Invalid URL to parse: {0}", url));
            }

            string urlFragment = new Uri(url).Fragment;
            if (String.IsNullOrEmpty(urlFragment) || !urlFragment.StartsWith("#access_token"))
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
            return JsonConvert.DeserializeObject<AuthenticationService>(s);
        }

        public void DumpToFile(FileStream fs)
        {
            string s = JsonConvert.SerializeObject(this, Formatting.Indented);
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            fs.Write(bytes, 0, bytes.Length);
        }
    }
}
