using System;
using XboxWebApi.Extensions;

namespace XboxWebApi.Services.Model
{
    public class InboxMessageHeader : IStringable
    {
        public int Id; // Not set for single message response
        public bool IsRead; // Not set for single message response
        public ulong SenderXuid;
        public string Sender;
        public DateTime Sent;
        public DateTime Expiration;
        public string MessageType;
        public bool HasText;
        public bool HasPhoto;
        public bool HasAudio;
        public string MessageFolderType;
    }

    public class InboxMessage : IStringable
    {
        public InboxMessageHeader Header;
        public string MessageSummary;
        public MessageAction[] Actions;
    }
}