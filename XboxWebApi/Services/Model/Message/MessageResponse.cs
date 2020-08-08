using System;

namespace XboxWebApi.Services.Model
{
    public class MessageResponse
    {
        public InboxMessageHeader Header;
        public string MessageText;
        public object AttachmentId;
        public object Attachment;
        public MessageAction[] Actions;
    }
}