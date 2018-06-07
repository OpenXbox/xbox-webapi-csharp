using System;
using XboxWebApi.Extensions;

namespace XboxWebApi.Services.Model
{
    public class Conversation : IStringable
    {
        public ConversationHeader Summary;
        public ConversationMessage[] Messages;
    }
    public class ConversationResponse : IStringable
    {
        public Conversation Conversation;
    }
}