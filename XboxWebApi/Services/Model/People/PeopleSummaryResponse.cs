using System;
using XboxWebApi.Extensions;

namespace XboxWebApi.Services.Model
{
    public class PeopleSummaryResponse : IStringable
    {
        public int TargetFollowingCount;
        public int TargetFollowerCount;
        public bool IsCallerFollowingTarget;
        public bool IsTargetFollowingCaller;
        public bool HasCallerMarkedTargetAsFavorite;
        public bool HasCallerMarkedTargetAsKnown;
        public string LegacyFriendStatus;
        public int AvailablePeopleSlots;
        public int RecentChangeCount;
        public string Watermark;
    }
}