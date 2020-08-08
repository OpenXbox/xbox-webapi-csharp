using System;

namespace XboxWebApi.Services.Model
{
    public class ConversationMessage
    {
        public int MessageId;
        public int SenderTitleId;
        public ulong SenderXuid;
        public string SenderGamerTag;
        public DateTime SentTime;
        public DateTime LastUpdateTime;
        public string MessageType;
        public string MessageFolder;
        public bool IsRead;
        public bool HasPhoto;
        public bool HasAudio;
        public string MessageText;
        public MessageAction[] Actions;
    }
}