using System;
using System.Collections.Generic;

using XboxWebApi.Common;

namespace XboxWebApi.Services.Model
{
    public class ProfilesRequest
    {
        public ulong[] UserIds;
        public ProfileSetting[] Settings;

        public ProfilesRequest(ulong[] xuids, ProfileSetting[] settings)
        {
            UserIds = xuids;
            Settings = settings;
        }
    }
}