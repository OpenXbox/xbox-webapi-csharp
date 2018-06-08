using System;
using XboxWebApi.Extensions;

namespace XboxWebApi.Services.Model
{
    public class ProfileSettingElement : IStringable
    {
        public ProfileSetting Id;
        public string Value;
    }

    public class ProfileUser : IStringable
    {
        public ulong Id;
        public ulong HostId;
        public ProfileSettingElement[] Settings;
        public bool IsSponsoredUser;
    }

    public class ProfileResponse : IStringable
    {
        public ProfileUser[] ProfileUsers;
    }
}