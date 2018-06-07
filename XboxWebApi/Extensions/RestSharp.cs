using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;
using RestSharp;
using XboxWebApi.Common;
using XboxWebApi.Extensions;

namespace XboxWebApi.Extensions
{
    public class RestClientEx : RestClient
    {
        public RestClientEx() : base()
        {
            SetSerializer(NewtonsoftJsonSerializer.Default);
        }

        public RestClientEx(Uri baseUrl) : base(baseUrl)
        {
            SetSerializer(NewtonsoftJsonSerializer.Default);
        }

        public RestClientEx(string baseUrl) : base(baseUrl)
        {
            SetSerializer(NewtonsoftJsonSerializer.Default);
        }

        public RestClientEx(Uri baseUrl, JsonNamingStrategy namingStrategy)
            : base(baseUrl)
        {
            SetSerializer(NewtonsoftJsonSerializer.Create(namingStrategy));
        }

        public RestClientEx(string baseUrl, JsonNamingStrategy namingStrategy)
            : base(baseUrl)
        {
            SetSerializer(NewtonsoftJsonSerializer.Create(namingStrategy));
        }

        public void SetSerializer(NewtonsoftJsonSerializer serializer)
        {
            base.AddHandler("application/json", serializer);
            base.AddHandler("text/json", serializer);
            base.AddHandler("text/x-json", serializer);
            base.AddHandler("text/javascript", serializer);
            base.AddHandler("*+json", serializer);
        }

        public override IRestResponse Execute(IRestRequest request)
        {
            IRestResponse response = base.Execute(request);

            Debug.WriteLine(GetDebugInfo(response));
            if (response.ErrorException != null || !response.IsSuccessful)
            {
                throw new ApiException($"Http request failed, Code: {response.StatusCode}, " +
                                       $"Message: \"{response.ErrorMessage}\", " +
                                       $"see IRestResponse for details", response);
            }

            return response;
        }

        public override IRestResponse<T> Execute<T>(IRestRequest request)
        {
            IRestResponse<T> response = base.Execute<T>(request);
            if (response.ErrorException != null || !response.IsSuccessful)
            {
                throw new ApiException($"Http request failed, Code: {response.StatusCode}, " +
                                       $"Message: \"{response.ErrorMessage}\", " +
                                       $"see IRestResponse for details", response);
            }
            Debug.WriteLine(GetDebugInfo<T>(response));

            return response;
        }

        private string GetDebugInfo<T>(IRestResponse<T> response)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Empty);
            sb.AppendLine("## CONTENT DESERIALIZATION ##");
            sb.AppendLine($"Created {typeof(T)}: {((IStringable)response.Data)}");
            return sb.ToString();
        }

        private string GetDebugInfo(IRestResponse response)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Empty);
            sb.AppendLine("#------ DEBUG ------#");
            sb.AppendLine($"BaseURL: {this.BaseUrl}");
            sb.AppendLine(String.Empty);
            sb.AppendLine("## REQUEST ##");
            sb.AppendLine($"Method: {response.Request.Method}");
            sb.AppendLine($"Resource: {response.Request.Resource}");
            if (response.Request.Parameters != null && response.Request.Parameters.Count > 0)
            {
                sb.AppendLine("Parameters:");
                response.Request.Parameters.ForEach(x =>
                {
                    // Skip printing Authorization header
                    if (x.Name == "Authorization")
                        return;
                    sb.AppendLine($"  Parameter: {x.Type}: {x.Name}={x.Value}");
                });
            }
            if (response.ErrorException != null)
            {
                sb.AppendLine(String.Empty);
                sb.AppendLine("## ERROR ##");
                sb.AppendLine($"Exception: {response.ErrorException}");
                sb.AppendLine($"ErrorMessage: {response.ErrorMessage}");
                sb.AppendLine("# DEBUG END #");
                return sb.ToString();
            }
            sb.AppendLine(String.Empty);
            sb.AppendLine("## RESPONSE ##");
            sb.AppendLine($"StatusCode: {response.StatusCode}");
            sb.AppendLine($"StatusDescription: {response.StatusDescription}");
            sb.AppendLine($"Content: {response.Content}");
            sb.AppendLine($"ContentLength: {response.ContentLength}");
            sb.AppendLine($"ContentType: {response.ContentType}");
            sb.AppendLine($"ResponseUri: {response.ResponseUri}");
            if (response.Cookies != null && response.Cookies.Count > 0)
            {
                sb.AppendLine("Cookies:");
                foreach(RestResponseCookie cookie in response.Cookies)
                    sb.AppendLine($"  Cookie: {cookie}");
            }
            if (response.Headers != null && response.Headers.Count > 0)
            {
                sb.AppendLine("Headers:");
                foreach(Parameter header in response.Headers)
                    sb.AppendLine($"  Header: {header.Type}: {header.Name}={header.Value}");
            }
            sb.AppendLine("#---- DEBUG END ----#");
            return sb.ToString();
        }
    }

    public class RestRequestEx : RestRequest
    {
        public RestRequestEx() : base(){}
        public RestRequestEx(Method method) : base(method){}
        public RestRequestEx(string resource) : base(resource){}
        public RestRequestEx(string resource, Method method)
            : base(resource, method){}
        public RestRequestEx(Uri resource) : base(resource){}
        public RestRequestEx(Uri resource, Method method)
            : base(resource, method){}

        public IRestRequest AddHeaders(NameValueCollection headers)
        {
            foreach (string kv in headers)
                this.AddHeader(kv, headers[kv]);
            return this;
        }

        public IRestRequest AddQueryParameters(NameValueCollection parameters)
        {
            foreach (string kv in parameters)
                this.AddQueryParameter(kv, parameters[kv]);
            return this;
        }

        public IRestRequest AddJsonBody(Object obj, JsonNamingStrategy namingStrategy)
        {
            base.RequestFormat = DataFormat.Json;
            base.JsonSerializer = NewtonsoftJsonSerializer.Create(namingStrategy);
            return base.AddJsonBody(obj);
        }
    }
}