using System;
using System.Collections.Generic;

namespace XboxWebApi.Services.Model
{
    public class PresenceRequestQuery : Common.IHttpRequestQuery
    {
        public PresenceLevel Level;

        public PresenceRequestQuery(PresenceLevel level)
        {
            Level = level;
        }

        public Dictionary<string,string> GetQuery()
        {
            string value;
            switch (Level)
            {
                case PresenceLevel.All:
                    value = "all";
                    break;
                case PresenceLevel.Device:
                    value = "device";
                    break;
                case PresenceLevel.Title:
                    value = "title";
                    break;
                case PresenceLevel.User:
                    value = "user";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        $"Unhandled PresenceLevel: {Level}");

            }
            return new Dictionary<string,string>()
            {
                {"level", value}
            };
        }
    }
}