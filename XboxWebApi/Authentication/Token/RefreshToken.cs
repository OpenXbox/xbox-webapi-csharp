using System;

namespace XboxWebApi.Authentication
{
	public class RefreshToken : BaseAuthToken
    {
        public RefreshToken()
        {
        }

        public RefreshToken(Model.WindowsLiveResponse response)
        {
                Issued = response.CreationTime;
				Expires = response.CreationTime + TimeSpan.FromDays(14);
                Jwt = response.RefreshToken;
        }
    }
}
