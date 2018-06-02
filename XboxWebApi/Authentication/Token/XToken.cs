using System;
using XboxWebApi.Authentication.Model;

namespace XboxWebApi.Authentication
{
	public class XToken : BaseAuthToken
    {
        public XToken()
        {
        }

		public XToken(XASResponse tokenResponse) : base(tokenResponse)
        {
        }
    }
}
