using System;
using System.Collections.Generic;

namespace XboxWebApi.Services.Model.EDS
{
    public class EDSScheduleRequestQuery : Common.IHttpRequestQuery
    {
        public DateTime StartDate;

        public EDSScheduleRequestQuery(DateTime startDate, int a, int b, int c)
        {
            StartDate = startDate;
        }

        public Dictionary<string,string> GetQuery()
        {
            return new Dictionary<string,string>()
            {
                {"channelLineupId", StartDate.ToString()}
            };
        }
    }
}