using System;
using RestSharp;

namespace XboxWebApi.Common
{
    public class ApiException: Exception
    {
        public IRestResponse HttpResponse { get; internal set; }
        public ApiException()
        {
        }

        public ApiException(string message, IRestResponse response)
            : base(message)
        {
            HttpResponse = response;
        }

        public ApiException(string message, Exception inner, IRestResponse response)
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