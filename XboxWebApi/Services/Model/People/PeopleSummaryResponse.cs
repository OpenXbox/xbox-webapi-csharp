using System;

namespace XboxWebApi.Services.Model
{
    public class PeopleSummaryResponse
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