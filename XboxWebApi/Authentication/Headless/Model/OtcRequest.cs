using System;
using System.Collections.Generic;
using XboxWebApi.Common;

namespace XboxWebApi.Authentication.Headless.Model
{
    public class OtcRequest : IHttpFormEncodedContent
    {
        private string AuthDataPostField { get; set; }
        private string AuthData { get; set; }

        public string Login { get; set; }
        public string Flowtoken { get; set; }
        public string Purpose {get; set; }
        public string UiMode { get; set; }
        public string Channel { get; set; }
        public string ProofConfirmation { get; set; }

        public void SetPostFieldName(string fieldName)
        {
            AuthDataPostField = fieldName;
        }

        public void SetAuthData(string authData)
        {
            AuthData = authData;
        }

        public Dictionary<string, string> GetFormContent()
        {
            if (String.IsNullOrEmpty(AuthDataPostField) || String.IsNullOrEmpty(AuthData))
            {
                throw new InvalidProgramException("No AuthData POST-field or AuthData provided!");
            }

            var content = new Dictionary<string,string>()
            {
                {"login", Login},
                {"flowtoken", Flowtoken},
                {"purpose", Purpose},
                {"UIMode", UiMode},
                {"channel", Channel},
                {AuthDataPostField, AuthData}
            };

            if (!String.IsNullOrEmpty(ProofConfirmation))
            {
                content.Add("ProofConfirmation", ProofConfirmation);
            }

            return content;
        }
    }
}