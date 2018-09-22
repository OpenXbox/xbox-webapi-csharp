using System;
using System.Collections.Specialized;
using XboxWebApi.Extensions;

namespace XboxWebApi.Services.Model.CQS
{
    public class CQSLineupRequestQuery
    {
        public string Desired;

        public CQSLineupRequestQuery()
        {
           Desired = CQSVesperType.MobileLineup;
        }

        public NameValueCollection GetQuery()
        {
            return new NameValueCollection()
            {
                {"desired", Desired}
            };
        }
    }
}