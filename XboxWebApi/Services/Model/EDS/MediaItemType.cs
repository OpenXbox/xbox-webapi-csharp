using System;

namespace XboxWebApi.Services.Model.EDS
{
    public enum MediaItemType
    {
        // Xenon
        Xbox360Game,
        Xbox360GameContent,
        Xbox360GameDemo,

        XboxGameTrial,
        XboxTheme,
        XboxOriginalGame,
        XboxGamerTile,
        XboxArcadeGame,
        XboxGameConsumable,
        XboxGameVideo,
        XboxGameTrailer,
        XboxBundle,
        XboxXnaCommunityGame,
        XboxMarketplace,
        XboxApp,

        // Durango
        DGame,
        DGameDemo,
        DConsumable,
        DDurable,
        DApp,
        DActivity,
        DNativeApp,

        // Metro
        MetroGame,
        MetroGameContent,
        MetroGameConsumable,

        AvatarItem,

        MobileGame,
        XboxMobilePDLC,
        XboxMobileConsumable,

        TVShow,
        TVEpisode,
        TVSeries,
        TVSeason,

        Album,
        Track,
        MusicVideo,
        MusicArtist,

        WebGame,
        WebVideo,
        WebVideoCollection,

        GameLayer,
        GameActivity,
        AppActivity,
        VideoLayer,
        VideoActivity,

        Subscription
    }
}