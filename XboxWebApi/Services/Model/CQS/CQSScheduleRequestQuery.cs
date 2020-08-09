using System;
using System.Collections.Generic;

namespace XboxWebApi.Services.Model.CQS
{
    public class CQSScheduleRequestQuery : Common.IHttpRequestQuery
    {
        public DateTime StartDate;
        public int DurationMinutes;
        public int ChannelSkip;
        public int ChannelCount;
        public string Desired;

        public CQSScheduleRequestQuery(DateTime startDate, int durationMinutes,
                                      int channelSkip, int channelCount)
        {
            Desired = CQSVesperType.MobileSchedule;
            StartDate = startDate;
            DurationMinutes = durationMinutes;
            ChannelSkip = channelSkip;
            ChannelCount = channelCount;
        }

        public Dictionary<string,string> GetQuery()
        {
            return new Dictionary<string,string>()
            {
                {"startDate", StartDate.ToString("o")},
                {"durationMinutes", DurationMinutes.ToString()},
                {"channelSkip", ChannelSkip.ToString()},
                {"channelCount", ChannelCount.ToString()},
                {"desired", Desired}
            };
        }
    }
}