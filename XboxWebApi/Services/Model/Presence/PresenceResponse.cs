using System;

namespace XboxWebApi.Services.Model
{
    public class PresenceResponse
    {
        public ulong Xuid;
        public PresenceState State;
        public PresenceLastSeen LastSeen;
        public PresenceDevice[] Devices;
    }
}