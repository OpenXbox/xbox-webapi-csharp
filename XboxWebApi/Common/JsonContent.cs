using System;
using System.Text;
using System.Net.Http;

namespace XboxWebApi.Common
{
    public class JsonContent : StringContent
    {
        public const string JsonMediaType = "application/json";

        public JsonContent(object content)
            : base(NewtonsoftJsonSerializer.Create(JsonNamingStrategy.Default).Serialize(content),
                   Encoding.UTF8, JsonMediaType)
        {
        }

        public JsonContent(object content, JsonNamingStrategy namingStrategy)
            : base(NewtonsoftJsonSerializer.Create(namingStrategy).Serialize(content),
                   Encoding.UTF8, JsonMediaType)
        {
        }

        public JsonContent(object content, JsonNamingStrategy namingStrategy, Encoding encoding)
            : base(NewtonsoftJsonSerializer.Create(namingStrategy).Serialize(content),
                   encoding, JsonMediaType)
        {
        }

        public JsonContent(object content, JsonNamingStrategy namingStrategy, Encoding encoding, string mediaType)
            : base(NewtonsoftJsonSerializer.Create(namingStrategy).Serialize(content),
                   encoding, mediaType)
        {
        }
    }
}
