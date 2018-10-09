using System;
using System.Collections.Specialized;
using RestSharp;
using XboxWebApi.Common;
using XboxWebApi.Extensions;

namespace XboxWebApi.Services
{
    public class XblService
    {
        protected IRestSharpEx HttpClient { get; }

        public string BaseUrl { get; internal set; }
        public NameValueCollection Headers { get; internal set; }
        public IXblConfiguration Config { get; internal set; }
        public XblService(IXblConfiguration config, string baseUrl, IRestSharpEx httpClient)
        {
            Config = config;
            BaseUrl = baseUrl;

            HttpClient = httpClient;
            Console.WriteLine("ParamsNull: " + (HttpClient.DefaultParameters == null));
            HttpClient.BaseUrl = new Uri(baseUrl);
            HttpClient.AddDefaultHeader("Authorization", $"XBL3.0 x={Config.Userhash};{Config.xToken.Jwt}");
            HttpClient.SetSerializer(NewtonsoftJsonSerializer.CamelCase);
        }
    }
}
