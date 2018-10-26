using System;
using System.Collections.Specialized;
using RestSharp;
using XboxWebApi.Authentication;
using XboxWebApi.Authentication.Model;

namespace XboxWebApi.Services
{
    public interface IXblConfiguration
    {
        XToken xToken { get; }
        XblLocale Locale { get; }

        XboxUserInformation UserInfo { get; }
        ulong XboxUserId { get; }
        string Userhash { get; }
        bool TokenValid { get; }
    }
}