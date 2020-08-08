using System;
using System.Collections.Generic;
using XboxWebApi.Common;

namespace XboxWebApi.Authentication.Headless.Model
{
    public class LoginRequest : IHttpFormEncodedContent
    {
        private bool NeedsRemoteNGCParams { get; set; }
        public string Login { get; set; }
        public string Passwd { get; set; }
        public string PPFT { get; set; }
        public string PPSX { get; set; }
        public string SI { get; set; }
        public int Type { get; set; }
        public int NewUser { get; set; }
        public int LoginOptions { get; set; }

        // Only set the following if (NeedsRemoteNGCParams == true)
        public int Ps { get; set; }
        public string PsRNGCEntropy { get; set; }
        public int PsRNGCDefaultType { get; set; }

        public void SetNeedsRemoteNGCParams(bool value)
        {
            NeedsRemoteNGCParams = value;
        }

        public Dictionary<string, string> GetFormContent()
        {
            var content = new Dictionary<string,string>()
            {
                {"login", Login},
                {"passwd", Passwd},
                {"PPFT", PPFT},
                {"PPSX", PPSX},
                {"SI", SI},
                {"type", Type.ToString()},
                {"NewUser", NewUser.ToString()},
                {"LoginOptions", LoginOptions.ToString()}
            };

            if (NeedsRemoteNGCParams)
            {
                content.Add("ps", Ps.ToString());
                content.Add("psRNGCEntropy", PsRNGCEntropy);
                content.Add("psRNGCDefaultType", PsRNGCDefaultType.ToString());
            }

            return content;
        }
    }
}