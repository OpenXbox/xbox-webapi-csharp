using System;
using System.Text;
using Newtonsoft.Json;

namespace XboxWebApi.Authentication.Model
{
    public class XboxUserInformation
    {
		[JsonProperty(PropertyName="agg")]
		public string AgeGroup;

		[JsonProperty(PropertyName="gtg")]
		public string Gamertag;

		[JsonProperty(PropertyName="prv")]
		public string Privileges;

		[JsonProperty(PropertyName="usr")]
		public string UserSettingsRestrictions;

		[JsonProperty(PropertyName="utr")]
		public string UserTitleRestrictions;

		[JsonProperty(PropertyName="xid")]
		public ulong XboxUserId;

		[JsonProperty(PropertyName="uhs")]
		public string Userhash;

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("Gamertag: {0}\n", Gamertag);
			sb.AppendFormat("XboxUserId: {0}\n", XboxUserId);
			sb.AppendFormat("Userhash: {0}\n", Userhash);
			sb.AppendFormat("Privileges: {0}\n", Privileges);
			sb.AppendFormat("UserSystemRestrictions: {0}\n", UserSettingsRestrictions);
			sb.AppendFormat("UserTitleRestrictions: {0}\n", UserTitleRestrictions);
			return sb.ToString();
		}
	}
}
