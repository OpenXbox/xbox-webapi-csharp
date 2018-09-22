using System;
using System.Collections.Specialized;
using XboxWebApi.Extensions;

namespace XboxWebApi.Services.Model.EDS
{
    public class EDSLineupRequestQuery
    {
        public Guid ChannelLineupId;

        public EDSLineupRequestQuery(Guid channelLineupId)
        {
           ChannelLineupId = channelLineupId;
        }

        public NameValueCollection GetQuery()
        {
            return new NameValueCollection()
            {
                {"channelLineupId", ChannelLineupId.ToString()}
            };
        }
    }
}