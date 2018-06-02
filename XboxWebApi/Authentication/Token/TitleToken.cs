using System;
using XboxWebApi.Authentication.Model;

namespace XboxWebApi.Authentication
{
	public class TitleToken : BaseAuthToken
    {
        public TitleToken()
        {
        }

		public TitleToken(XASResponse tokenResponse) : base(tokenResponse)
        {
        }
    }
}
