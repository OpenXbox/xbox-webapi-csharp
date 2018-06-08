using System;
using System.Collections.Specialized;
using RestSharp;
using XboxWebApi.Common;
using XboxWebApi.Extensions;

namespace XboxWebApi.Services
{
    public class XblService
    {
        public string BaseUrl { get; internal set; }
        public NameValueCollection Headers { get; internal set; }
        public XblConfiguration Config { get; internal set; }
        public XblService(XblConfiguration config, string baseUrl)
        {
            Config = config;
            BaseUrl = baseUrl;
        }

        public RestClientEx ClientFactory(JsonNamingStrategy namingStrategy = JsonNamingStrategy.Default)
        {
            RestClientEx client = new RestClientEx(BaseUrl, namingStrategy);
            client.AddDefaultHeader("Authorization", $"XBL3.0 x={Config.Userhash};{Config.xToken.Jwt}");
            return client;
        }
    }
}
