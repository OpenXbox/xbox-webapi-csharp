using RestSharp;
using XboxWebApi.Common;

namespace XboxWebApi.Extensions
{
    public interface IRestSharpEx : IRestClient
    {
        void SetSerializer(NewtonsoftJsonSerializer serializer);
    }
}
