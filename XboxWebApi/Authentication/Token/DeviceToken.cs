using System;
using XboxWebApi.Authentication.Model;

namespace XboxWebApi.Authentication
{
	public class DeviceToken : BaseAuthToken
    {
        public DeviceToken()
        {
        }
        
        public DeviceToken(XASResponse tokenResponse) : base(tokenResponse)
        {
        }
    }
}
