using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace XboxWebApi.Common
{
    public static class HttpRequestMessageExtensions
    {
        public static void AddQueryParameter(this HttpRequestMessage request, string key, string value)
        {
            var result = QueryHelpers.AddQueryString(request.RequestUri.ToString(), key, value);
            request.RequestUri = new Uri(result, UriKind.Relative);
        }

        public static void AddQueryParameter(this HttpRequestMessage request, Dictionary<string,string> parameters)
        {
            foreach (var param in parameters)
                request.AddQueryParameter(param.Key, param.Value);
        }
    }

    public static class HttpResponseMessageExtensions
    {
    }

    public static class HttpRequestHeadersExtensions
    {
        public static void Add(this HttpRequestHeaders headers, Dictionary<string,string> toAdd)
        {
            foreach(var hdr in toAdd)
                headers.Add(hdr.Key, hdr.Value);
        }
    }

    public static class HttpContentExtensions
    {
        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content,
            JsonNamingStrategy namingStrategy=JsonNamingStrategy.Default)
        {
            var stringContent = await content.ReadAsStringAsync();
            return NewtonsoftJsonSerializer.Create(namingStrategy).Deserialize<T>(stringContent);
        }
    }
}