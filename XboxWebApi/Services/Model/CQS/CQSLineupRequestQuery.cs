using System;
using System.Collections.Generic;

namespace XboxWebApi.Services.Model.CQS
{
    public class CQSLineupRequestQuery : Common.IHttpRequestQuery
    {
        public string Desired;

        public CQSLineupRequestQuery()
        {
           Desired = CQSVesperType.MobileLineup;
        }

        public Dictionary<string,string> GetQuery()
        {
            return new Dictionary<string,string>()
            {
                {"desired", Desired}
            };
        }
    }
}