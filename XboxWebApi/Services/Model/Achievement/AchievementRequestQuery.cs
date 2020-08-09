using System;
using System.Collections.Generic;

namespace XboxWebApi.Services.Model.Achievement
{
    public class AchievementRequestQuery : Common.IHttpRequestQuery
    {
        public ulong TitleId;

        public AchievementRequestQuery(ulong titleId)
        {
           TitleId = titleId;
        }

        public Dictionary<string,string> GetQuery()
        {
            return new Dictionary<string,string>()
            {
                {"titleId", TitleId.ToString()}
            };
        }
    }
}