using System;
using Newtonsoft.Json;

namespace XboxWebApi.Authentication.Headless.Model
{
	public class CredentialTypeResponse
	{
        public string Username { get; set; }
        public string Display { get; set; }
        public string Location { get; set; }
        public int IfExistsResult { get; set; }
        
        // IsSignupDisallowed: Only returned when non-Microsoft-Account email is supplied
        public bool IsSignupDisallowed { get; set; }
        public CredentialType Credentials { get; set; }
	}

    public class CredentialType
    {
        public int PrefCredential { get; set; }
        public int HasPassword { get; set; }
        public int HasRemoteNGC { get; set; }
        public int HasFido { get; set; }
        public int HasPhone { get; set; }
        public int HasGithubFed { get; set; }
        public int HasGoogleFed { get; set; }
        public int HasLinkedInFed { get; set; }
        public RemoteNgcParams RemoteNgcParams { get; set; }
    }

    public class RemoteNgcParams
    {
        public string SessionIdentifier { get; set; }
        public string Entropy { get; set; }
        public int DefaultType { get; set; }
    }
}