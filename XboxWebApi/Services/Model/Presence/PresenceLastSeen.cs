using System;
using XboxWebApi.Extensions;

namespace XboxWebApi.Services.Model
{
    public class PresenceLastSeen : IStringable
    {
        public DeviceType DeviceType;
        public ulong TitleId;
        public string TitleName;
        public DateTime Timestamp;
    }
}