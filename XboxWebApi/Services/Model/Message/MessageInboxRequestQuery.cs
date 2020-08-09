using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace XboxWebApi.Services.Model
{
    public class MessageInboxRequestQuery : Common.IHttpRequestQuery
    {
        public int SkipItems;
        public int MaxItems;

        public MessageInboxRequestQuery(int skipItems, int maxItems)
        {
           SkipItems = skipItems;
           MaxItems = maxItems;
        }

        public Dictionary<string,string> GetQuery()
        {
            return new Dictionary<string,string>()
            {
                {"skipItems", SkipItems.ToString()},
                {"maxItems", MaxItems.ToString()}
            };
        }
    }
}