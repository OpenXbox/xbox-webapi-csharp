using System;
using System.Collections.Specialized;
using RestSharp;
using XboxWebApi.Authentication;
using XboxWebApi.Authentication.Model;

namespace XboxWebApi.Services
{
    public class XblConfiguration
    {
        public XboxUserInformation UserInfo { get; internal set; }
        public XToken xToken { get; internal set; }
        public XblLocale Locale { get; internal set; }

        public ulong XboxUserId => UserInfo.XboxUserId;
        public string Userhash => UserInfo.Userhash;
        public bool TokenValid => xToken.Valid;

        public XblConfiguration(XboxUserInformation userInfo, XToken xtoken, XblLocale locale)
        {
            UserInfo = userInfo;
            xToken = xtoken;
            Locale = locale;
        }
    }
}