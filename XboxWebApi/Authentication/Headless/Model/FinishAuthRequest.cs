using System;
using System.Collections.Generic;
using XboxWebApi.Common;

namespace XboxWebApi.Authentication.Headless.Model
{
    public class FinishAuthRequest : IHttpFormEncodedContent
    {
        public string Login { get; set; }
        public string PPFT { get; set; }
        public string SentProofIDE {get; set; }
        public int Sacxt { get; set; }
        public int Saav { get; set; }
        public string GeneralVerify { get; set; }
        public int Type { get; set; }
        public string Purpose { get; set; }
        public string I18 { get; set; }

        public string Otc { get; set; }
        public string Slk { get; set; }
        public string ProofConfirmation { get; set; }

        public Dictionary<string, string> GetFormContent()
        {
            var content = new Dictionary<string,string>()
            {
                {"login", Login},
                {"PPFT", PPFT},
                {"SentProofIDE", SentProofIDE},
                {"sacxt", Sacxt.ToString()},
                {"saav", Saav.ToString()},
                {"GeneralVerify", GeneralVerify},
                {"type", Type.ToString()},
                {"purpose", Purpose},
                {"i18", I18}
            };


            if (!String.IsNullOrEmpty(Otc))
            {
                content.Add("otc", Otc);
            }

            if (!String.IsNullOrEmpty(Slk))
            {
                content.Add("slk", Slk);
            }

            if (!String.IsNullOrEmpty(ProofConfirmation))
            {
                content.Add("ProofConfirmation", ProofConfirmation);
            }

            return content;
        }
    }
}