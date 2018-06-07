using System;
using System.Collections.Specialized;
using XboxWebApi.Extensions;

namespace XboxWebApi.Services.Model
{
    public class PresenceRequestQuery
    {
        public PresenceLevel Level;

        public PresenceRequestQuery(PresenceLevel level)
        {
            Level = level;
        }

        public NameValueCollection GetQuery()
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
            return new NameValueCollection()
            {
                {"level", value}
            };
        }
    }
}