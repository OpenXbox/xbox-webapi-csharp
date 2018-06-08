using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp.Serializers;
using RestSharp.Deserializers;

namespace XboxWebApi.Common
{
    /*
    ::: Serializing :::
    private void SetJsonContent(RestRequest request, object obj)
    {
        request.RequestFormat = DataFormat.Json;
        request.JsonSerializer = NewtonsoftJsonSerializer.Default;
        request.AddJsonBody(obj);
    }

    ::: Deserializing :::
    private RestClient CreateClient(string baseUrl)
    {
        var client = new RestClient(baseUrl);

        // Override with Newtonsoft JSON Handler
        client.AddHandler("application/json", NewtonsoftJsonSerializer.Default);
        client.AddHandler("text/json", NewtonsoftJsonSerializer.Default);
        client.AddHandler("text/x-json", NewtonsoftJsonSerializer.Default);
        client.AddHandler("text/javascript", NewtonsoftJsonSerializer.Default);
        client.AddHandler("*+json", NewtonsoftJsonSerializer.Default);

        return client;
    }
    
     */
    // Copyright (c) Philipp Wagner. All rights reserved.
    // Licensed under the MIT license. See LICENSE file in the project root for full license information.
    // Source: https://www.bytefish.de/blog/restsharp_custom_json_serializer
    public class NewtonsoftJsonSerializer : ISerializer, IDeserializer
    {
        private Newtonsoft.Json.JsonSerializer serializer;

        public NewtonsoftJsonSerializer(Newtonsoft.Json.JsonSerializer serializer)
        {
            this.serializer = serializer;           
        }

        public string ContentType {
            get { return "application/json"; } // Probably used for Serialization?
            set { }
        }

        public string DateFormat { get; set; }

        public string Namespace { get; set; }

        public string RootElement { get; set; }

        public string Serialize(object obj)
        {
            using (var stringWriter = new System.IO.StringWriter())
            {
                using (var jsonTextWriter = new JsonTextWriter(stringWriter))
                {
                    serializer.Serialize(jsonTextWriter, obj);

                    return stringWriter.ToString();
                }
            }
        }

        public T Deserialize<T>(string content)
        {
            using (var stringReader = new System.IO.StringReader(content))
            {
                using (var jsonTextReader = new JsonTextReader(stringReader))
                {
                    return serializer.Deserialize<T>(jsonTextReader);
                }
            }
        }

        public T Deserialize<T>(RestSharp.IRestResponse response)
        {
            return Deserialize<T>(response.Content);
        }

        public static NewtonsoftJsonSerializer Create(JsonNamingStrategy namingStrategy)
        {
            Newtonsoft.Json.Serialization.NamingStrategy chosenStrategy;
            switch (namingStrategy)
            {
                case JsonNamingStrategy.Default:
                    chosenStrategy = new DefaultNamingStrategy();
                    break;
                case JsonNamingStrategy.CamelCase:
                    chosenStrategy = new CamelCaseNamingStrategy();
                    break;
                case JsonNamingStrategy.SnakeCase:
                    chosenStrategy = new SnakeCaseNamingStrategy();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        $"Unhandled JsonNamingStrategy: {namingStrategy}");
            }

            return new NewtonsoftJsonSerializer(new Newtonsoft.Json.JsonSerializer()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None,
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = chosenStrategy
                }
            });
        }

        public static NewtonsoftJsonSerializer Default =>
                NewtonsoftJsonSerializer.Create(JsonNamingStrategy.Default);

        public static NewtonsoftJsonSerializer CamelCase =>
                NewtonsoftJsonSerializer.Create(JsonNamingStrategy.CamelCase);

        public static NewtonsoftJsonSerializer SnakeCase =>
                NewtonsoftJsonSerializer.Create(JsonNamingStrategy.SnakeCase);
    }
}