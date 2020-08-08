using System;
using System.Collections.Generic;

namespace XboxWebApi.Services.Model.EDS
{
    public class EDSLineupRequestQuery : Common.IHttpRequestQuery
    {
        public Guid ChannelLineupId;

        public EDSLineupRequestQuery(Guid channelLineupId)
        {
           ChannelLineupId = channelLineupId;
        }

        public Dictionary<string,string> GetQuery()
        {
            return new Dictionary<string,string>()
            {
                {"channelLineupId", ChannelLineupId.ToString()}
            };
        }
    }
}