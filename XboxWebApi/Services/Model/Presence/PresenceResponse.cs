using System;
using XboxWebApi.Extensions;

namespace XboxWebApi.Services.Model
{
    public class PresenceResponse : IStringable
    {
        public ulong Xuid;
        public PresenceState State;
        public PresenceLastSeen LastSeen;
        public PresenceDevice[] Devices;
    }
}