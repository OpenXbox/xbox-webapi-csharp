using System;
using System.Collections.Specialized;
using XboxWebApi.Extensions;

namespace XboxWebApi.Services.Model.Achievement
{
    public class AchievementRequestQuery
    {
        public ulong TitleId;

        public AchievementRequestQuery(ulong titleId)
        {
           TitleId = titleId;
        }

        public NameValueCollection GetQuery()
        {
            return new NameValueCollection()
            {
                {"titleId", TitleId.ToString()}
            };
        }
    }
}