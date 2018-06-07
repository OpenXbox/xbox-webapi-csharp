using System;
using XboxWebApi.Extensions;

namespace XboxWebApi.Services.Model
{
    public class ConversationHeader : IStringable
    {
        public ulong SenderXuid;
        public string SenderGamerTag;
        public DateTime LastUpdated;
        public DateTime LastSent;
        public int MessageCount;
        public int UnreadMessageCount;
        public ConversationMessage LastMessage;
        public ConversationMessage[] Messages; // Not set for Conversations
    }

    public class ConversationsResponse : IStringable
    {
        public ConversationHeader[] Results;
    }
}