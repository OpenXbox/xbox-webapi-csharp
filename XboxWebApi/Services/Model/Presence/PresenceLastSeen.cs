using System;

namespace XboxWebApi.Services.Model
{
    public class PresenceLastSeen
    {
        public DeviceType DeviceType;
        public ulong TitleId;
        public string TitleName;
        public DateTime Timestamp;
    }
}