using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using XboxWebApi.Authentication.Model;
using XboxWebApi.Common;

namespace XboxWebApi.Authentication.Headless
{
    /// <summary>
    /// Auth flow:
    /// - Choose entry from available AuthStragies
    /// - If GetMethodVerificationPrompt returns != null, get proofConfirmation
    /// - If CheckOtc returns true, get OTC
    /// - Finish authentication by calling "Authenticate"
    /// </summary>
    public class TwoFactorAuthenticationService
    {
        static ILogger logger = Logging.Factory.CreateLogger<TwoFactorAuthenticationService>();

        /* Authenticator V2 GIF data */
        public static byte[] GifHeader {get; } =
        {
            (byte)'G',
            (byte)'I',
            (byte)'F',
            (byte)'8',
            (byte)'7',
            (byte)'a',
        };
        public static int MinimumGifSize { get; } = 35;

        public const int MaxTOTPv2WaitTimeSeconds = 120;

        readonly HttpClient _client;
        readonly Dictionary<string,object> _serverData;
        public string Email { get; }
        public List<Model.TwoFactorAuthStrategy> AuthStragies { get; }
        public string FlowToken { get; }
        public string PostUrl { get; }
        public string SessionLookupKey { get; private set; }

        /// <summary>
        /// Parses the list of supported authentication strategies
        /// Auth variants position changes from time to time, so instead of accessing a fixed,
        /// named field, heuristic detection is used
        /// Example node:
        /// [{
        ///    data:'<some data>', type:1, display:'pyxb-testing@outlook.com', otcEnabled:true, otcSent:false,
        ///    isLost:false, isSleeping:false, isSADef:true, isVoiceDef:false, isVoiceOnly:false, pushEnabled:false
        ///  },
        ///  {
        ///    data:'<some data>', type:3, clearDigits:'69', ctryISO:'DE', display:'*********69', otcEnabled:true,
        ///    otcSent:false, voiceEnabled:true, isLost:false, isSleeping:false, isSADef:false, isVoiceDef:false,
        ///    isVoiceOnly:false, pushEnabled:false
        ///  },
        ///  {
        ///    data:'2342352452523414114', type:14, display:'2342352452523414114', otcEnabled:false, otcSent:false,
        ///    isLost:false, isSleeping:false, isSADef:false, isVoiceDef:false, isVoiceOnly:false, pushEnabled:true
        /// }]
        /// </summary>
        /// <param name="serverData">Server data</param>
        /// <returns>Returns list of available TwoFactorAuthStrategy</returns>
        public static List<Model.TwoFactorAuthStrategy> ParseAuthStrategies(Dictionary<string,object> serverData)
        {
            var deserializer = NewtonsoftJsonSerializer.CamelCase;
            foreach(var val in serverData.Values)
            {
                try
                {
                    var deserialized = deserializer.Deserialize<List<Model.TwoFactorAuthStrategy>>((string)val);
                    if (deserialized != null
                        && deserialized.Count > 0
                        && !String.IsNullOrEmpty(deserialized[0].Data))
                    {
                        return deserialized;
                    }
                }
                catch (Exception)
                {
                    // pass
                }
            }
            return null;
        }

        /// <summary>
        /// Verify the AuthSessionState GIF-image, returned when polling the `Microsoft Authenticator v2`.
        /// At first, check if provided bytes really are a GIF image, then check image-dimensions to get Session State.
        ///
        /// Possible image dimensions:
        ///  - 2px x 2px - Rejected Authorization
        ///  - 1px x 1px - Pending Authorization (keep polling)
        ///  - 1px x 2px - Approved Authorization
        /// </summary>
        /// <param name="gifData">The GIF image, describing current Authentication Session State.</param>
        /// <returns>
        /// The current Authentication State. AuthSessionState.ERROR if provided bytes are not a GIF
        /// or the image-dimensions are unknown.
        /// </returns>
        public static Model.TwoFactorAuthSessionState VerifyAuthenticatorV2Gif(byte[] gifData)
        {
            if (gifData.Length < MinimumGifSize)
            {
                logger.LogError("Got GIF image smaller than expected! " +
                                "Got {} instead of min. {}", gifData.Length, MinimumGifSize);
                return Model.TwoFactorAuthSessionState.ERROR;
            }
            byte[] gifDataHeader = new byte[GifHeader.Length];
            Array.Copy(gifData, 0, gifDataHeader, 0, gifDataHeader.Length);

            if (!gifDataHeader.SequenceEqual(GifHeader))
            {
                logger.LogError("Returned image does not look like GIF");
                return Model.TwoFactorAuthSessionState.ERROR;
            }

            ushort gifWidth = BitConverter.ToUInt16(gifData, 6);
            ushort gifHeight = BitConverter.ToUInt16(gifData, 8);

            if (gifWidth == 1 && gifHeight == 2)
                return Model.TwoFactorAuthSessionState.APPROVED;
            else if (gifWidth == 1 && gifHeight == 1)
                return Model.TwoFactorAuthSessionState.PENDING;
            else if (gifWidth == 2 && gifHeight == 2)
                return Model.TwoFactorAuthSessionState.REJECTED;
            else
                logger.LogError("Unknown GIF dimensions! W: {}, H: {}", gifWidth, gifHeight);

            return Model.TwoFactorAuthSessionState.ERROR;
        }

        /// <summary>
        /// Initialize TwoFactorAuthenticationService
        /// </summary>
        /// <param name="client">HttpClient instance from HeadlessAuthenticationService</param>
        /// <param name="serverData">ServerData received in HeadlessAuthenticationService.Authenticate</param>
        /// <param name="email">Email address of Microsoft account</param>
        public TwoFactorAuthenticationService(
            HttpClient client,
            Dictionary<string, object> serverData,
            string email,
            string flowToken
        )
        {
            _client = client;
            _serverData = serverData;
            Email = email;
            FlowToken = flowToken;
            //FlowToken = (string)_serverData["sFT"];
            PostUrl = (string)_serverData["urlPost"];
            AuthStragies = ParseAuthStrategies(_serverData);
            SessionLookupKey = null;
        }

        /// <summary>
        /// Request OTC (One-Time-Code) if 2FA via Email, Mobile phone or MS Authenticator v2 is desired.
        /// </summary>
        /// <param name="authMethod">Auth method to use</param>
        /// <param name="proof">Proof Verification, used by mobile phone and email-method, for MS Authenticator provide `null`</param>
        /// <param name="authData">Authentication data for this provided, specific authorization method</param>
        /// <returns>Returns instance of OtcResponse</returns>
        private async Task<Model.OtcResponse> RequestOtc(
            Model.TwoFactorAuthMethod authMethod,
            string proof,
            string authData
        )
        {
            const string GetOnetimeCodeUrl = "https://login.live.com/pp1600/GetOneTimeCode.srf";

            var otcRequest = new Model.OtcRequest()
            {
                Login = Email,
                Flowtoken = FlowToken,
                Purpose = "eOTT_OneTimePassword",
                UiMode = "11"
            };

            otcRequest.SetAuthData(authData);

            if(!String.IsNullOrEmpty(proof))
            {
                otcRequest.ProofConfirmation = proof;
            }

            if (Model.TwoFactorAuthMethod.Email == authMethod)
            {
                otcRequest.Channel = "Email";
                otcRequest.SetPostFieldName("AltEmailE");
            }
            else if (Model.TwoFactorAuthMethod.SMS == authMethod)
            {
                otcRequest.Channel = "SMS";
                otcRequest.SetPostFieldName("MobileNumE");
            }
            else if (Model.TwoFactorAuthMethod.Voice == authMethod)
            {
                otcRequest.Channel = "Voice";
                otcRequest.SetPostFieldName("MobileNumE");
            }
            else if (Model.TwoFactorAuthMethod.TOTPAuthenticatorV2 == authMethod)
            {
                otcRequest.Channel = "PushNotifications";
                otcRequest.SetPostFieldName("SAPId");
            }
            else
            {
                logger.LogError("Unsupported TwoFactor Auth-Type: {}", authMethod);
                throw new Exception(String.Format("Unsupported TwoFactor Auth-Type: {}", authMethod));
            }

            var request = new HttpRequestMessage(HttpMethod.Post, GetOnetimeCodeUrl);
            request.Content = new FormUrlEncodedContent(otcRequest.GetFormContent());

            var response = await _client.SendAsync(request);
            return await response.Content.ReadAsJsonAsync<Model.OtcResponse>();
        }

        /// <summary>
        /// Finish the Two-Factor-Authentication. If it succeeds we are provided with Access and Refresh-Token.
        /// </summary>
        /// <param name="authMethod">Auth method to use</param>
        /// <param name="authData">Authentication data for this provided, specific authorization method</param>
        /// <param name="otc">One-Time-Code, required for every method except MS Authenticator v2</param>
        /// <param name="proofConfirmation">Confirmation of Email or mobile phone number, if that method was chosen</param>
        public async Task<HttpResponseMessage> FinishAuth(
            Model.TwoFactorAuthMethod authMethod,
            string authData,
            string otc,
            string proofConfirmation
        )
        {
            var finishAuthPostBody = new Model.FinishAuthRequest()
            {
                Login = Email,
                PPFT = FlowToken,
                SentProofIDE = authData,
                Sacxt = 1,
                Saav = 0,
                Purpose = "eOTT_OneTimePassword",
                I18 = "__DefaultSAStrings|1,__DefaultSA_Core|1,__DefaultSA_Wizard|1",

                Otc = otc,
                Slk = SessionLookupKey,
                ProofConfirmation = proofConfirmation
            };

            if (Model.TwoFactorAuthMethod.SMS == authMethod
               || Model.TwoFactorAuthMethod.Voice == authMethod
               || Model.TwoFactorAuthMethod.Email == authMethod)
            {
                finishAuthPostBody.Type = 18;
                finishAuthPostBody.GeneralVerify = "false";
            }
            else if (Model.TwoFactorAuthMethod.TOTPAuthenticator == authMethod)
            {
                finishAuthPostBody.Type = 19;
                finishAuthPostBody.GeneralVerify = "false";
            }
            else if (Model.TwoFactorAuthMethod.TOTPAuthenticatorV2 == authMethod)
            {
                finishAuthPostBody.Type = 22;
                finishAuthPostBody.GeneralVerify = "";
            }
            else
            {
                throw new Exception(String.Format("Unhandled case for submitting OTC, method: {}", authMethod));
            }

            var request = new HttpRequestMessage(HttpMethod.Post, PostUrl);
            request.Content = new FormUrlEncodedContent(finishAuthPostBody.GetFormContent());

            return await _client.SendAsync(request);
        }

        /// <summary>
        /// Poll MS Authenticator v2 SessionState.
        /// Polling happens for maximum of 120 seconds if Authorization is not approved by the Authenticator App.
        /// It will return earlier if request gets approved/rejected.
        /// </summary>
        /// <returns>Returns current session state</returns>
        private async Task<Model.TwoFactorAuthSessionState> PollSessionState()
        {
            string pollingUrl = null;
            foreach (var value in _serverData.Values)
            {
                string currentVal = value as string;
                if (!String.IsNullOrEmpty(currentVal)
                  && currentVal.StartsWith("https://login.live.com/GetSessionState.srf"))
                {
                    pollingUrl = currentVal;
                }
            }
            if (String.IsNullOrEmpty(pollingUrl))
            {
                throw new Exception("Cannot find polling URL for TOTPv2 session state");
            }

            DateTime maxWaitTime = DateTime.Now + TimeSpan.FromSeconds(MaxTOTPv2WaitTimeSeconds);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, pollingUrl);
            request.AddQueryParameter("slk", SessionLookupKey);

            logger.LogInformation("Polling Authenticator v2 Verification for {} seconds", MaxTOTPv2WaitTimeSeconds);

            Model.TwoFactorAuthSessionState sessionState = Model.TwoFactorAuthSessionState.PENDING;
            while (DateTime.Now < maxWaitTime)
            {
                var pollResponse = await _client.SendAsync(request);
                var gifData = await pollResponse.Content.ReadAsByteArrayAsync();
                sessionState = VerifyAuthenticatorV2Gif(gifData);
                if (sessionState != Model.TwoFactorAuthSessionState.PENDING)
                {
                    break;
                }
                await Task.Delay(1 * 1000);
            }

            return sessionState;
        }

        /// <summary>
        /// If auth strategy needs verification of method, get userprompt string
        /// For example:
        /// * Mobile phone verification (SMS, Voice) needs last 4 digits of mobile #
        /// * Email verification needs whole address
        /// </summary>
        /// <param name="strategyIndex">Index of chosen auth strategy</param>
        /// <returns>Userinput prompt string if proof is needed, `null` otherwise</returns>
        public string GetMethodVerificationPrompt(int strategyIndex)
        {
            var strategy = AuthStragies[strategyIndex];
            var authType = strategy.Type;
            var displayString = strategy.Display;

            if (Model.TwoFactorAuthMethod.SMS == authType
               || Model.TwoFactorAuthMethod.Voice == authType)
            {
                return String.Format("Enter last four digits of following phone number: {}", displayString);
            }
            else if (Model.TwoFactorAuthMethod.Email == authType)
            {
                return String.Format("Enter the full mail address '{}'", displayString);
            }

            logger.LogWarning("GetMethodVerificationPrompt: No prompt necessary");
            return String.Empty;
        }

        /// <summary>
        /// Check if OneTimeCode is required. If it's required, request it.
        /// </summary>
        /// <param name="strategyIndex">Index of chosen auth strategy</param>
        /// <param name="proofConfirmation">Verification / proof of chosen auth strategy</param>
        /// <returns>Returns true if OTC is required, false otherwise</returns>
        public async Task<bool> CheckOtc(int strategyIndex, string proofConfirmation)
        {
            var strategy = AuthStragies[strategyIndex];
            var authType = strategy.Type;
            var authData = strategy.Data;

            Model.OtcResponse response = null;

            if (authType != Model.TwoFactorAuthMethod.TOTPAuthenticator)
            {
                /*
                TOTPAuthenticator V1 works without requesting anything (offline OTC generation)
                TOTPAuthenticator V2 needs a cached `Session Lookup Key`, not OTC, we handle it here
                */
                try
                {
                    response = await RequestOtc(authType, proofConfirmation, authData);
                }
                catch (Exception exc)
                {
                    throw new Exception(String.Format("Error requesting OTC, error: %i", exc));
                }
                logger.LogDebug("State from Request OTC: {}", response.State);

                if (authType == Model.TwoFactorAuthMethod.TOTPAuthenticatorV2)
                {
                    // Smartphone push notification
                    SessionLookupKey = response.SessionLookupKey;
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Perform chain of Two-Factor-Authentication (2FA) with the Windows Live Server.
        /// </summary>
        /// <param name="strategyIndex">Index of chosen auth strategy</param>
        /// <param name="otc">One Time Code</param>
        /// <returns>If authentication succeeds, the WindowsLiveResponse</returns>
        public async Task<WindowsLiveResponse> Authenticate(
            int strategyIndex,
            string proofConfirmation,
            string otc
        )
        {
            var strategy = AuthStragies[strategyIndex];
            var authType = strategy.Type;
            var authData = strategy.Data;

            logger.LogDebug("Using Method: {}", authType);

            if (Model.TwoFactorAuthMethod.TOTPAuthenticatorV2 == authType)
            {
                if (this.SessionLookupKey == null)
                {
                    throw new Exception("Did not receive SessionLookupKey from Authenticator V2 request!");
                }

                var sessionState = await PollSessionState();
                if (sessionState != Model.TwoFactorAuthSessionState.APPROVED)
                {
                    throw new Exception(String.Format("Authentication by Authenticator V2 failed!" +
                                        " State: %s", sessionState));
                }


                // Do not send auth_data when finishing TOTPv2 authentication
                authData = null;
            }

            HttpResponseMessage response = await FinishAuth(authType, authData, otc, proofConfirmation);
            var location = response.Headers.Location;
            return AuthenticationService.ParseWindowsLiveResponse(location.ToString());
        }
    }
}