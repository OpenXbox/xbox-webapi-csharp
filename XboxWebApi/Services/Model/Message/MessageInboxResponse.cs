using System;

namespace XboxWebApi.Services.Model
{
    public class MessageInboxResponse
    {
        public InboxMessage[] Results;
        public PagingInfo PagingInfo;
    }
}