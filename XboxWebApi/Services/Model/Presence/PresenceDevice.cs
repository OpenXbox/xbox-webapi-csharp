using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using XboxWebApi.Extensions;

namespace XboxWebApi.Services.Model
{
    public class PresenceActivity : IStringable
    {
        public string RichPresence;
    }
    public class PresenceTitle : IStringable
    {
        public ulong Id;
        public string Name;
        public TitleViewState Placement;
        public TitleState State;
        public DateTime LastModified;
        public PresenceActivity Activity;
    }

    public class PresenceDevice : IStringable
    {
        public DeviceType Type;
        public PresenceTitle[] Titles;
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum DeviceType
    {
        XboxOne,
        Xbox360,
        WindowsOneCore,
        iOS,
        Android
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum TitleViewState
    {
        Full,
        Fill,
        Snapped,
        Background
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum TitleState
    {
        Active,
        Inactive
    }
}