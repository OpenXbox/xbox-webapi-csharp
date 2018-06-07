using System;
using XboxWebApi.Extensions;

namespace XboxWebApi.Services.Model
{
    public class MessageResponse : IStringable
    {
        public InboxMessageHeader Header;
        public string MessageText;
        public object AttachmentId;
        public object Attachment;
        public MessageAction[] Actions;
    }
}