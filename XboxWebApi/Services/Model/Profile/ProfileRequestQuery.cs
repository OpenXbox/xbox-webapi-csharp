using System;
using System.Collections.Specialized;
using XboxWebApi.Extensions;

namespace XboxWebApi.Services.Model
{
    public class ProfileRequestQuery
    {
        public ProfileSetting[] Settings;

        public ProfileRequestQuery(ProfileSetting[] settings)
        {
            Settings = settings;
        }

        public NameValueCollection GetQuery()
        {
            return new NameValueCollection()
            {
                {"settings", String.Join(",", Array.ConvertAll(Settings, x => x.ToString()))}
            };
        }
    }
}