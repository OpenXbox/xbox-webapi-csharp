using System;
using System.Collections.Specialized;
using XboxWebApi.Authentication;
using XboxWebApi.Authentication.Model;

namespace XboxWebApi.Services
{
    public class XblConfiguration : IXblConfiguration
    {
        public XToken xToken { get; internal set; }
        public XblLocale Locale { get; internal set; }

        public XboxUserInformation UserInfo => xToken.UserInformation;
        public ulong XboxUserId => UserInfo.XboxUserId;
        public string Userhash => UserInfo.Userhash;
        public bool TokenValid => xToken.Valid;

        public XblConfiguration(XToken xtoken, XblLocale locale)
        {
            xToken = xtoken;
            Locale = locale;
        }
    }
}