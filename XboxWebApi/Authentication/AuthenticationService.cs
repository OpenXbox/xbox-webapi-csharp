using System;
using System.IO;
using System.Collections.Specialized;
using Newtonsoft.Json;
using XboxWebApi.Common;
using XboxWebApi.Authentication.Model;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Logging;

namespace XboxWebApi.Authentication
{
    public class AuthenticationService
    {
        static ILogger logger = Logging.Factory.CreateLogger<AuthenticationService>();
        public AccessToken AccessToken { get; set; }
        public RefreshToken RefreshToken { get; set; }
        public UserToken UserToken { get; set; }
        public DeviceToken DeviceToken { get; set; }
        public TitleToken TitleToken { get; set; }
        public XToken XToken { get; set; }
        public XboxUserInformation UserInformation { get; set; }

        /// <summary>
        /// Creates a HttpClient with provided baseUrl
        /// </summary>
        /// <param name="baseUrl">Base URL to use</param>
        /// <returns>An instance of HttpClient</returns>
        public static HttpClient ClientFactory(string baseUrl)
        {
            logger.LogTrace("ClientFactory(string baseUrl) called with baseUrl: {}", baseUrl);
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl.EndsWith("/") ? baseUrl : baseUrl + "/");
            logger.LogTrace("ClientFactory: client.BaseAddress set to: {}", client.BaseAddress);
            return client;
        }

        /// <summary>
        /// Initializes a new instance of AuthenticationService
        /// </summary>
        public AuthenticationService()
        {
            logger.LogTrace("AuthenticationService() ctor called");
        }

        /// <summary>
        /// Initializes a new instance of AuthenticationService via WindowsLiveResponse.
        /// </summary>
        /// <param name="wlResponse">
        /// Windows Live response, either constructed from redirect URI
        /// or by refreshing token
        /// </param>
        public AuthenticationService(WindowsLiveResponse wlResponse)
        {
            logger.LogTrace("AuthenticationService(WindowsLiveResponse wlResponse) ctor called");
            AccessToken = new AccessToken(wlResponse);
            RefreshToken = new RefreshToken(wlResponse);
        }

        /// <summary>
        /// Initializes a new instance of AuthenticationService via Access- and RefreshToken.
        /// </summary>
        /// <param name="accessToken">Windows Live Access token</param>
        /// <param name="refreshToken">Windows Live Refresh token</param>
        public AuthenticationService(AccessToken accessToken, RefreshToken refreshToken)
        {
            logger.LogTrace("AuthenticationService(AccessToken accessToken, RefreshToken refreshToken) ctor called");
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        /// <summary>
        /// Authenticate to Xbox Live.
        /// First it refreshes the Windows Live token, then it authenticates to XASU and XSTS
        /// </summary>
        /// <returns>Returns true on success, false otherwise</returns>
        public async Task<bool> AuthenticateAsync()
        {
            try
            {
                logger.LogTrace("AuthenticateAsync() called");
                logger.LogTrace("AuthenticateAsync: Calling RefreshLiveTokenAsync");
                try
                {
                    WindowsLiveResponse windowsLiveTokens = await RefreshLiveTokenAsync(RefreshToken);
                    logger.LogTrace("AuthenticateAsync: Constructing AccessToken");
                    if (!string.IsNullOrWhiteSpace(windowsLiveTokens.AccessToken))
                    {
                        AccessToken = new AccessToken(windowsLiveTokens);
                    }
                    logger.LogTrace("AuthenticateAsync: Constructing RefreshToken");
                    if (!string.IsNullOrWhiteSpace(windowsLiveTokens.RefreshToken))
                    {
                        RefreshToken = new RefreshToken(windowsLiveTokens);
                    }
                }
                catch {}
                logger.LogTrace("AuthenticateAsync: Calling AuthenticateXASUAsync");
                UserToken = await AuthenticateXASUAsync(AccessToken);
                logger.LogTrace("AuthenticateAsync: Calling AuthenticateXSTSAsync");
                XToken = await AuthenticateXSTSAsync(UserToken, DeviceToken, TitleToken);
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, "AuthenticateAsync failed due to HTTP error: {}", ex.Message);
                return false;
            }

            logger.LogTrace("AuthenticateAsync: Setting UserInformation");
            UserInformation = XToken.UserInformation;
            return true;
        }

        /// <summary>
        /// Dump tokens and userinformation to a JSON file
        /// Shorthand class method to call DumpToJsonFileAsync in current instance-context.
        /// </summary>
        /// <param name="targetFilePath">Target JSON filepath</param>
        /// <returns>Returns true on success, false otherwise</returns>
        public async Task<bool> DumpToJsonFileAsync(string targetFilePath)
            => await DumpToJsonFileAsync(this, targetFilePath);
        
        /// <summary>
        /// Update tokens and userinformation from a JSON file
        /// </summary>
        /// <param name="sourceFilePath">Source JSON filepath</param>
        /// <returns>Returns true on success, false otherwise</returns>
        public async Task<bool> UpdateFromJsonFileAsync(string sourceFilePath)
        {
            try
            {
                AuthenticationService tempObj = await LoadFromJsonFileAsync(sourceFilePath);
                this.RefreshToken = tempObj.RefreshToken;
                this.AccessToken = tempObj.AccessToken;
                this.UserToken = tempObj.UserToken;
                this.XToken = tempObj.XToken;
                this.DeviceToken = tempObj.DeviceToken;
                this.TitleToken = tempObj.TitleToken;
                this.UserInformation = tempObj.UserInformation;
            }
            catch (Exception exc)
            {
                logger.LogError("UpdateFromJsonFileAsync failed, error: {}", exc);
                return false;
            }

            return true;
        }

        /*
         * Static methods
         */

        /// <summary>
        /// Refreshes Access- and RefreshToken via the provided RefreshToken 
        /// </summary>
        /// <param name="refreshToken">Windows Live refresh token</param>
        /// <returns>The gathered WindowsLiveResponse.</returns>
        public static async Task<WindowsLiveResponse> RefreshLiveTokenAsync(
            RefreshToken refreshToken)
        {
            logger.LogTrace("RefreshLiveTokenAsync() called");
            HttpClient client = ClientFactory("https://login.live.com/");

            var request = new HttpRequestMessage(HttpMethod.Get, "oauth20_token.srf");
            var parameters = new Model.WindowsLiveRefreshQuery(refreshToken);
            request.AddQueryParameter(parameters.GetQuery());
            
            var response = (await client.SendAsync(request)).EnsureSuccessStatusCode();
            return await response.Content.ReadAsJsonAsync<WindowsLiveResponse>(JsonNamingStrategy.SnakeCase);
        }

        /// <summary>
        /// Authenticates to XASU (user.auth.xboxlive.com) via Windows Live AccessToken.
        /// </summary>
        /// <param name="accessToken">Windows Live access token</param>
        /// <returns>Returns the Xbox Live user token</returns>
        public static async Task<UserToken> AuthenticateXASUAsync(AccessToken accessToken)
        {
            logger.LogTrace("AuthenticateXASUAsync() called");
            HttpClient client = ClientFactory("https://user.auth.xboxlive.com/");
            var request = new HttpRequestMessage(HttpMethod.Post, "user/authenticate");
            var requestBody = new XASURequest(accessToken);
            request.Content = new JsonContent(requestBody);
            request.Headers.Add("x-xbl-contract-version", "1");

            var response = (await client.SendAsync(request)).EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsJsonAsync<XASResponse>();
            return new UserToken(data);
        }

        /// <summary>
        /// Authenticates to XASD (device.auth.xboxlive.com) via Windows Live AccessToken.
        /// NOTE: BROKEN
        /// </summary>
        /// <param name="accessToken">Windows Live access token</param>
        /// <returns>Returns the Xbox Live device token</returns>
        public static async Task<DeviceToken> AuthenticateXASDAsync(AccessToken accessToken)
        {
            logger.LogTrace("AuthenticateXASDAsync() called");
            HttpClient client = ClientFactory("https://device.auth.xboxlive.com/");
            var request = new HttpRequestMessage(HttpMethod.Post, "device/authenticate");
            var requestBody = new XASDRequest(accessToken);
            request.Headers.Add("x-xbl-contract-version", "1");
            request.Content = new JsonContent(requestBody);

            var response = (await client.SendAsync(request)).EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsJsonAsync<XASResponse>();
            return new DeviceToken(data);
        }

        /// <summary>
        /// Authenticates to XAST (title.auth.xboxlive.com) via Access- and DeviceToken.
        /// </summary>
        /// <param name="accessToken">Windows Live access token</param>
        /// <param name="deviceToken">Xbox Live device token</param>
        /// <returns>Returns the Xbox Live title token</returns>
        public static async Task<TitleToken> AuthenticateXASTAsync(AccessToken accessToken,
                                                  DeviceToken deviceToken)
        {
            logger.LogTrace("AuthenticateXASTAsync() called");
            HttpClient client = ClientFactory("https://title.auth.xboxlive.com/");
            var request = new HttpRequestMessage(HttpMethod.Post, "title/authenticate");
            var requestBody = new XASTRequest(accessToken, deviceToken);
            request.Headers.Add("x-xbl-contract-version", "1");
            request.Content = new JsonContent(requestBody);

            var response = (await client.SendAsync(request)).EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsJsonAsync<XASResponse>();
            return new TitleToken(data);
        }

        /// <summary>
        /// Authenticates to XAST (title.auth.xboxlive.com) via Access- and DeviceToken.
        /// </summary>
        /// <param name="userToken">Xbox Live user token</param>
        /// <param name="deviceToken">Xbox Live device token</param>
        /// <param name="titleToken">Xbox Live title token</param>
        /// <returns>Returns the Xbox Live X-token</returns>
        public static async Task<XToken> AuthenticateXSTSAsync(UserToken userToken,
                                              DeviceToken deviceToken = null,
                                              TitleToken titleToken = null)
        {
            logger.LogTrace("AuthenticateXSTSAsync() called");
            HttpClient client = ClientFactory("https://xsts.auth.xboxlive.com/");
            var request = new HttpRequestMessage(HttpMethod.Post, "xsts/authorize");
            var requestBody = new XSTSRequest(userToken,
                                              deviceToken: deviceToken,
                                              titleToken: titleToken);
            request.Headers.Add("x-xbl-contract-version", "1");
            request.Content = new JsonContent(requestBody);

            var response = await client.SendAsync(request);
            var data = await response.Content.ReadAsJsonAsync<XASResponse>();
            return new XToken(data);
        }

        /// <summary>
        /// Gets the OAUTH2 Windows Live authentication URL
        /// </summary>
        /// <returns>The ready-to-call authentication URL</returns>
        public static string GetWindowsLiveAuthenticationUrl()
        {
            logger.LogTrace("GetWindowsLiveAuthenticationUrl() called");
            var parameters = new Model.WindowsLiveAuthenticationQuery();
            var url = QueryHelpers.AddQueryString(
                "https://login.live.com/oauth20_authorize.srf",
                parameters.GetQuery());

            url = System.Web.HttpUtility.UrlDecode(url);
            logger.LogDebug("GetWindowsLiveAuthenticationUrl: Returning URL: {}", url);
            return url;
        }

        /// <summary>
        /// Parses the Windows Live authentication URL received from a successful authentication flow.
        /// </summary>
        /// <param name="url">Windows Live redirect URL</param>
        /// <returns>The parsed Windows Live response, containing access- and refresh token</returns>
        public static WindowsLiveResponse ParseWindowsLiveResponse(string url)
        {
            logger.LogTrace("ParseWindowsLiveResponse() called with URL: {}", url);
            if (!url.StartsWith(WindowsLiveConstants.RedirectUrl))
            {
                logger.LogError("ParseWindowsLiveResponse: URL {} did not start with {}", url, WindowsLiveConstants.RedirectUrl);
                throw new InvalidDataException(String.Format("Invalid URL to parse: {0}", url));
            }

            string urlFragment = new Uri(url).Fragment;
            if (String.IsNullOrEmpty(urlFragment) || !urlFragment.StartsWith("#access_token"))
            {
                logger.LogError("ParseWindowsLiveResponse: URL fragment is invalid: {}", urlFragment);
                throw new InvalidDataException(String.Format("Invalid URL fragment: {0}", urlFragment));
            }

            // Cut off leading '#'
            logger.LogTrace("ParseWindowsLiveResponse: URL fragment before substring(1): {}", urlFragment);
            urlFragment = urlFragment.Substring(1);
            logger.LogTrace("ParseWindowsLiveResponse: URL fragment after substring(1): {}", urlFragment);

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

        /// <summary>
        /// Instantiates AuthenticationService from JSON string
        /// </summary>
        /// <param name="json">JSON string</param>
        /// <returns>Instance of AuthenticationService</returns>
        public static AuthenticationService LoadFromJson(string json)
        {
            return JsonConvert.DeserializeObject<AuthenticationService>(json);
        }

        /// <summary>
        /// Dumps AuthenticationService instance to JSON string
        /// </summary>
        /// <param name="obj">Instance of AuthenticationService</param>
        /// <returns>Returns a JSON string</returns>
        public static string DumpToJson(AuthenticationService obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        /// <summary>
        /// Instantiates AuthenticationService from a JSON file
        /// </summary>
        /// <param name="sourceFilePath">JSON filepath</param>
        /// <returns>Instance of AuthenticationService</returns>
        public async static Task<AuthenticationService> LoadFromJsonFileAsync(string sourceFilePath)
        {
            string json = await File.ReadAllTextAsync(sourceFilePath, System.Text.Encoding.UTF8);
            return LoadFromJson(json);
        }

        /// <summary>
        /// Dumps AuthenticationService instance to JSON file
        /// </summary>
        /// <param name="obj">Instance of AuthenticationService</param>
        /// <param name="targetFilePath">JSON filepath</param>
        /// <returns></returns>
        public async static Task<bool> DumpToJsonFileAsync(AuthenticationService obj, string targetFilePath)
        {
            string json = DumpToJson(obj);
            try
            {
                await File.WriteAllTextAsync(targetFilePath, json, System.Text.Encoding.UTF8);
                return true;
            }
            catch (Exception exc)
            {
                logger.LogError("DumpToJsonFile: Failed File.WriteAllTextAsync, error: {}", exc);
            }
            return false;
        }

        /// <summary>
        /// Initializes an instance of AuthenticationService from a JSON filestream.
        /// </summary>
        /// <param name="fs">Filestream to read from</param>
        /// <returns>Instance of AuthenticationService</returns>
        public async static Task<AuthenticationService> LoadFromJsonFileStream(FileStream fs)
        {
            logger.LogTrace("LoadFromJsonFileStream() called");
            byte[] buf = new byte[fs.Length];
            await fs.ReadAsync(buf, 0, buf.Length);
            string s = Encoding.UTF8.GetString(buf);
            return LoadFromJson(s);
        }

        /// <summary>
        /// Dumps the instance of AuthenticationService to a JSON filestream
        /// </summary>
        /// <param name="fs">Filestream to write to</param>
        public async static Task DumpToJsonFileStream(AuthenticationService obj, FileStream fs)
        {
            logger.LogTrace("DumpToJsonFileStream() called");
            string s = DumpToJson(obj);
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            await fs.WriteAsync(bytes, 0, bytes.Length);
        }
    }
}
