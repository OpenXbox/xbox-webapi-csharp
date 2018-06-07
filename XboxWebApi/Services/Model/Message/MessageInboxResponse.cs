using System;
using XboxWebApi.Extensions;

namespace XboxWebApi.Services.Model
{
    public class MessageInboxResponse : IStringable
    {
        public InboxMessage[] Results;
        public PagingInfo PagingInfo;
    }
}