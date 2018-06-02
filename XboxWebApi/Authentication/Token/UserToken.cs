using System;
using XboxWebApi.Authentication.Model;

namespace XboxWebApi.Authentication
{
	public class UserToken : BaseAuthToken
    {
        public UserToken()
        {
        }

		public UserToken(XASResponse tokenResponse) : base(tokenResponse)
        {
        }
    }
}
