using System;
using Newtonsoft.Json;

namespace XboxWebApi.Authentication.Headless.Model
{
	public class CredentialTypeRequest
	{
                public string Username { get; set; }
                public string Uaid { get; set; }
                public bool IsOtherIdpSupported { get; set; }
                public bool CheckPhones { get; set; }
                public bool IsRemoteNGCSupported { get; set; }
                public bool IsCookieBannerShown { get; set; }
                public bool IsFidoSupported { get; set; }
                public bool Forceotclogin { get; set; }
                public bool Otclogindisallowed { get; set; }
                public bool IsExternalFederationDisallowed { get; set; }
                public bool IsRemoteConnectSupported { get; set; }
                public int FederationFlags { get; set; }
                public bool IsSignup { get; set; }
                public string FlowToken { get; set; }
	}
}