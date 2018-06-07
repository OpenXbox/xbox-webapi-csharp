using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using XboxWebApi.Extensions;

namespace XboxWebApi.Authentication.Model
{
    public class XASResponse : IStringable
    {
		public string Token;
		public DateTime IssueInstant;
		public DateTime NotAfter;
		public Dictionary<string, List<XboxUserInformation>> DisplayClaims;
    }
}
