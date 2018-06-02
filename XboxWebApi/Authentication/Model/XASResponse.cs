using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace XboxWebApi.Authentication.Model
{
    public partial class XASResponse
    {
		public string Token;
		public DateTime IssueInstant;
		public DateTime NotAfter;
		public Dictionary<string, List<XboxUserInformation>> DisplayClaims;
    }

	public partial class XASResponse
    {
		public static XASResponse FromJson(string json) =>
		    JsonConvert.DeserializeObject<XASResponse>(
                json, Common.JsonSetting.StandardSetting());
    }
}
