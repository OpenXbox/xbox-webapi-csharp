using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using XboxWebApi.Authentication.Model;
using XboxWebApi.Common;

namespace XboxWebApi.Authentication.Headless
{
    public class HeadlessAuthenticationService
    {
        static ILogger logger = Logging.Factory.CreateLogger<HeadlessAuthenticationService>();

        HttpClient _client;
        public string AuthenticationUrl { get; private set; }

        /// <summary>
        /// Callback to choose the index for the desired authentication strategy
        /// </summary>
        /// <value>Enumerable of available auth strategies</value>
        public Func<IEnumerable<string>, Task<int>> ChooseAuthStrategyCallback { get; set; }

        /// <summary>
        /// Callback to verify f.e. mobile phone number
        /// </summary>
        /// <value>Challenge to verify (f.e. maked mobile phone #)</value>
        public Func<string, Task<string>> VerifyPosessionCallback { get; set; }

        /// <summary>
        /// Callback to enter the OneTimeCode
        /// </summary>
        /// <value>User-facing prompt message</value>
        public Func<string, Task<string>> EnterOneTimeCodeCallback { get; set; }

        /// <summary>
        /// Creates a HttpClientHandler with disabled redirects
        /// and cookieHandler
        /// </summary>
        /// <returns>An instance of HttpClientHandler</returns>
        public static HttpClientHandler HttpClientHandlerFactory()
        {
            logger.LogTrace("HttpClientHandlerFactory() called");
            return new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = true,
                CookieContainer = new System.Net.CookieContainer()
            };
        }

        public HeadlessAuthenticationService(string authUrl)
        {
            AuthenticationUrl = authUrl;
        }

        private async Task<WindowsLiveResponse> DoTwoFactorAuthAsync(HttpClient client, Dictionary<string,object> serverData, string email, string flowToken)
        {
            var twoFactorAuth = new TwoFactorAuthenticationService(
                _client, serverData, email, flowToken);

            List<string> strategyList = new List<string>();
            foreach(var s in twoFactorAuth.AuthStragies)
            {
                var str = String.Format("[{}] {}", s.Type, s.Display);
                logger.LogTrace("DoTwoFactorAuthAsync: Adding {} to strategy-list", str);
                strategyList.Add(str);
            }
            logger.LogTrace("Calling ChooseAuthStrategyCallback");
            int chosenStrategyIdx = await ChooseAuthStrategyCallback?.Invoke(strategyList);
            if (chosenStrategyIdx < 0 || chosenStrategyIdx >= strategyList.Count)
            {
                logger.LogError("Invalid choice for AuthStrategy, choice: {}", chosenStrategyIdx);
                return null;
            }

            string proofConfirmation = null;
            string otc = null;

            var strategy = twoFactorAuth.AuthStragies[chosenStrategyIdx];
            var verificationPrompt = twoFactorAuth.GetMethodVerificationPrompt(chosenStrategyIdx);
            if (verificationPrompt != null)
            {
                proofConfirmation = await VerifyPosessionCallback(verificationPrompt);
            }

            bool otcNeeded = await twoFactorAuth.CheckOtc(chosenStrategyIdx, proofConfirmation);
            if (otcNeeded)
            {
                otc = await EnterOneTimeCodeCallback("Enter One-Time-Code (OTC)");
            }

            return await twoFactorAuth.Authenticate(chosenStrategyIdx, proofConfirmation, otc);
        }

        public async Task<WindowsLiveResponse> AuthenticateAsync(string email, string password)
        {
            var _clientHandler = HttpClientHandlerFactory();
            _client = new HttpClient(_clientHandler);
            HttpResponseMessage response = await _client.GetAsync(AuthenticationUrl);

            var htmlBody = await response.Content.ReadAsStringAsync();
            
            // Extract ServerData javascript-object via regex, convert it to proper JSON
            var serverData = Utils.ExtractJsObject(htmlBody, "var ServerData");
            
            // Extract PPFT value (flowtoken)
            string ppft = serverData["sFTTag"] as string;
            string ppftValue = Utils.ParseXmlNode(ppft).GetElementsByTagName("input")[0].Attributes["value"].Value;

            string credentialTypeUrl = null;
            foreach (var value in serverData.Values)
            {
                if (value as string != null &&
                    ((string)value).StartsWith("https://login.live.com/GetCredentialType.srf"))
                {
                    credentialTypeUrl = (string)value;
                }
            }

            if (credentialTypeUrl == null)
            {
                logger.LogError("Did not find GetCredentialType URL");
                return null;
            }

            var cookies = _clientHandler.CookieContainer.GetCookies(new Uri("https://login.live.com"));
            var credentialTypePostData = new Model.CredentialTypeRequest()
            {
                Username = email,
                Uaid = cookies["uaid"].Value,
                IsOtherIdpSupported = false,
                CheckPhones = false,
                IsRemoteNGCSupported = true,
                IsCookieBannerShown = false,
                IsFidoSupported = false,
                Forceotclogin = false,
                Otclogindisallowed = false,
                IsExternalFederationDisallowed = false,
                IsRemoteConnectSupported = false,
                FederationFlags = 3,
                IsSignup = false,
                FlowToken = ppftValue
            };

            var request = new HttpRequestMessage(HttpMethod.Post, credentialTypeUrl);
            request.Headers.Referrer = response.RequestMessage.RequestUri;
            request.Content = new JsonContent(credentialTypePostData, JsonNamingStrategy.CamelCase);

            response = (await _client.SendAsync(request)).EnsureSuccessStatusCode();
            var credentialTypeResponse =
                await response.Content.ReadAsJsonAsync<Model.CredentialTypeResponse>();

            var loginRequest = new Model.LoginRequest()
            {
                Login = email,
                Passwd = password,
                PPFT = ppftValue,
                PPSX = "Passpor",
                SI = "Sign in",
                Type = 11,
                NewUser = 1,
                LoginOptions = 1
            };

            if (credentialTypeResponse.Credentials == null)
            {
                throw new Exception("Did not find Credentials in CredentialType respose, auth likely failed!");
            }
            else if (credentialTypeResponse.Credentials.HasRemoteNGC == 1)
            {
                loginRequest.SetNeedsRemoteNGCParams(true);
                Model.RemoteNgcParams ngcParams = credentialTypeResponse.Credentials.RemoteNgcParams;
                loginRequest.Ps = 2;
                loginRequest.PsRNGCEntropy = ngcParams.SessionIdentifier;
                loginRequest.PsRNGCDefaultType = ngcParams.DefaultType;
            }

            string loginUrlPost = (string)serverData["urlPost"];
            request = new HttpRequestMessage(HttpMethod.Post, loginUrlPost);
            request.Content = new FormUrlEncodedContent(loginRequest.GetFormContent());

            response = await _client.SendAsync(request);

            try
            {
                // If auth flow is already done, return tokens
                return AuthenticationService.ParseWindowsLiveResponse(
                    response.Headers.Location.ToString()
                );
            }
            catch (Exception exc)
            {
                logger.LogWarning("Two Factor auth required! error: {}", exc);
            }

            return await DoTwoFactorAuthAsync(_client, serverData, email, ppftValue);
        }
    }
}