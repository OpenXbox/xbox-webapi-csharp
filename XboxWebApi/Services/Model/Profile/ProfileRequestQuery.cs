using System;
using System.Collections.Generic;

namespace XboxWebApi.Services.Model
{
    public class ProfileRequestQuery : Common.IHttpRequestQuery
    {
        public ProfileSetting[] Settings;

        public ProfileRequestQuery(ProfileSetting[] settings)
        {
            Settings = settings;
        }

        public Dictionary<string,string> GetQuery()
        {
            return new Dictionary<string,string>()
            {
                {"settings", String.Join(",", Array.ConvertAll(Settings, x => x.ToString()))}
            };
        }
    }
}