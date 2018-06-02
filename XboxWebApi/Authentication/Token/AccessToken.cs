using System;

namespace XboxWebApi.Authentication
{
	public class AccessToken : BaseAuthToken
    {
        public AccessToken()
        {
        }

        public AccessToken(Model.WindowsLiveResponse response)
        {
                Issued = response.CreationTime;
				Expires = response.CreationTime + TimeSpan.FromSeconds(response.ExpiresIn);
                Jwt = response.AccessToken;
        }
    }
}
