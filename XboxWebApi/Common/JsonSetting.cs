using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace XboxWebApi.Common
{
    public static class JsonSetting
    {
		public static JsonSerializerSettings SnakeCaseSetting()
		{
			DefaultContractResolver contractResolver = new DefaultContractResolver
            {
				NamingStrategy = new SnakeCaseNamingStrategy()
            };
			return new JsonSerializerSettings
			{
				ContractResolver = contractResolver,
				Formatting = Formatting.None
			};
		}

		public static JsonSerializerSettings StandardSetting()
		{
			return new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None
            };
		}
    }
}
