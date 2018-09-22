using System;
using System.Collections.Specialized;
using XboxWebApi.Extensions;

namespace XboxWebApi.Services.Model.EDS
{
    public class EDSScheduleRequestQuery
    {
        public DateTime StartDate;

        public EDSScheduleRequestQuery(DateTime startDate, int a, int b, int c)
        {
            StartDate = startDate;
        }

        public NameValueCollection GetQuery()
        {
            return new NameValueCollection()
            {
                {"channelLineupId", StartDate.ToString()}
            };
        }
    }
}