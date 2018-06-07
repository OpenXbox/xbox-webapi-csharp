using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using XboxWebApi.Extensions;

namespace XboxWebApi.Services.Model
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum LaunchType
    {
        FiveByFive,
        DeepLink
    }
    public class MessageAction : IStringable
    {
        public int Id;
        public string VuiGui;
        public object VuiAlm;
        public object VuiPron;
        public object VuiConf;
        public string Launch;
        public LaunchType LaunchType;
    }
}