using System;
using XboxWebApi.Extensions;

namespace XboxWebApi.Services.Model
{
    public class GamertagRecipient
    {
        public string Gamertag;
    }

    public class XuidRecipient
    {
        public string Xuid;
    }

    public class MessageSendHeader : IStringable
    {
        public object[] Recipients;
    }

    public class MessageSendRequest : IStringable
    {
        public MessageSendHeader Header;
        public string MessageText;

        public MessageSendRequest(string messageText, ulong[] xuids)
        {
            Header = new MessageSendHeader()
            {
                Recipients = Array.ConvertAll(xuids, x =>
                    new XuidRecipient()
                    {
                        Xuid = x.ToString()
                    })
            };
            MessageText = messageText;
        }

        public MessageSendRequest(string messageText, string[] gamertags)
        {
            Header = new MessageSendHeader()
            {
                Recipients = Array.ConvertAll(gamertags, x =>
                    new GamertagRecipient()
                    {
                        Gamertag = x
                    })
            };
            MessageText = messageText;
        }
    }
}