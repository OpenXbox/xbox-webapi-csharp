using System;

namespace XboxWebApi.Services.Model
{
    public class ProfileSettingElement
    {
        public ProfileSetting Id;
        public string Value;
    }

    public class ProfileUser
    {
        public ulong Id;
        public ulong HostId;
        public ProfileSettingElement[] Settings;
        public bool IsSponsoredUser;
    }

    public class ProfileResponse
    {
        public ProfileUser[] ProfileUsers;
    }
}