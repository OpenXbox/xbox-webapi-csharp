using System;

namespace XboxWebApi.Services.Model
{
    public class Conversation
    {
        public ConversationHeader Summary;
        public ConversationMessage[] Messages;
    }
    public class ConversationResponse
    {
        public Conversation Conversation;
    }
}