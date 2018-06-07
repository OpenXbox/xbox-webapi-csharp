using System;
using System.ComponentModel;
using XboxWebApi.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XboxWebApi.Services.Model
{
    [JsonConverter(typeof(StringEnumConverter), true)]
    public enum PresenceLevel
    {
        User,
        Device,
        Title,
        All
    }
}