using System;
using Newtonsoft.Json;
using XboxWebApi.Extensions;
using XboxWebApi.Authentication.Model;

namespace XboxWebApi.Authentication
{
	public class BaseAuthToken : IStringable
	{
		public string Jwt { get; set; }
		public DateTime Issued { get; set; }
		public DateTime Expires { get; set; }
		public XboxUserInformation UserInformation { get; set; }

		public bool Valid => 
			(Expires != null && Expires > DateTime.Now);
		
		public bool HasUserInformation =>
			(UserInformation != null);

		public BaseAuthToken()
		{
		}

        public BaseAuthToken(XASResponse tokenResponse)
        {
			Jwt = tokenResponse.Token;
			Issued = tokenResponse.IssueInstant;
			Expires = tokenResponse.NotAfter;
			UserInformation = tokenResponse.DisplayClaims["xui"][0];
        }

        public override string ToString()
        {
            return String.Format("<{0} Jwt={1}, Issued={2}, Expires={3}>",
                                 base.ToString(), Jwt, Issued, Expires);
        }
    }
}
