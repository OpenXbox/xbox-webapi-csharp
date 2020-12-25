using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace XboxWebApi.Services
{
    public class XblService
    {
        protected HttpClient HttpClient { get; }

        public string BaseUrl { get; internal set; }
        public Dictionary<string,string> Headers { get; internal set; }
        public IXblConfiguration Config { get; internal set; }
        public XblService(IXblConfiguration config, string baseUrl)
        {
            HttpClient = new HttpClient();

            Config = config;
            BaseUrl = baseUrl;

            HttpClient.BaseAddress = new Uri(baseUrl);
            
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("XBL3.0",$"x={Config.Userhash};{Config.xToken.Jwt}");
        }
    }
}
