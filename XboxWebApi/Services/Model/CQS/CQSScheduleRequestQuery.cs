using System;
using System.Collections.Specialized;
using XboxWebApi.Extensions;

namespace XboxWebApi.Services.Model.CQS
{
    public class CQSScheduleRequestQuery
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

        public NameValueCollection GetQuery()
        {
            return new NameValueCollection()
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