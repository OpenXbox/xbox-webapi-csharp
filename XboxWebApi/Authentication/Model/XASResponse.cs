using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace XboxWebApi.Authentication.Model
{
    public class XASResponse
    {
		public string Token;
		public DateTime IssueInstant;
		public DateTime NotAfter;
		public Dictionary<string, List<XboxUserInformation>> DisplayClaims;
    }
}
