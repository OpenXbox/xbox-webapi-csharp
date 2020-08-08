using System;

namespace XboxWebApi.Services.Model
{
    public class People
    {
        public ulong Xuid;
        public DateTime AddedDateTimeUtc;
        public bool IsFavorite;
        public bool IsKnown;
        public string[] SocialNetworks;
        public bool IsFollowedByCaller;
        public bool IsFollowingCaller;
        public bool IsUnfollowingFeed;
    }
}