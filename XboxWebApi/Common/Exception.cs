using System;
using System.Net.Http;

namespace XboxWebApi.Common
{
    public class ApiException: Exception
    {
        public HttpRequestMessage HttpResponse { get; internal set; }
        public ApiException()
        {
        }

        public ApiException(string message, HttpRequestMessage response)
            : base(message)
        {
            HttpResponse = response;
        }

        public ApiException(string message, Exception inner, HttpRequestMessage response)
            : base(message, inner)
        {
            HttpResponse = response;
        }

        public ApiException(string message)
            : base(message)
        {
        }

        public ApiException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}