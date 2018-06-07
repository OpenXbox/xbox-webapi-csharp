using System;
using System.Collections.Specialized;
using XboxWebApi.Extensions;

namespace XboxWebApi.Services.Model
{
    public class MessageInboxRequestQuery
    {
        public int SkipItems;
        public int MaxItems;

        public MessageInboxRequestQuery(int skipItems, int maxItems)
        {
           SkipItems = skipItems;
           MaxItems = maxItems;
        }

        public NameValueCollection GetQuery()
        {
            return new NameValueCollection()
            {
                {"skipItems", SkipItems.ToString()},
                {"maxItems", MaxItems.ToString()}
            };
        }
    }
}