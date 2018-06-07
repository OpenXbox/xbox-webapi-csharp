using System;
using System.Collections.Generic;
using XboxWebApi.Extensions;

using XboxWebApi.Common;

namespace XboxWebApi.Services.Model
{
    public class PresenceBatchRequest
    {
        public string[] Users;
        public bool OnlineOnly;
        public PresenceLevel Level;

        public PresenceBatchRequest(ulong[] xuids, PresenceLevel level, bool onlineOnly=false)
        {
            if (xuids.Length > 1100)
                throw new ArgumentOutOfRangeException("Xuid list > 1100");
            Users = Array.ConvertAll(xuids, x=>x.ToString());
            OnlineOnly = onlineOnly;
            Level = level;
        }
    }
}