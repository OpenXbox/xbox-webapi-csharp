using System;

namespace XboxWebApi.Services.Model.EDS
{
    public enum MediaGroup
    {
        /*
        Media Group, used as parameter for EDS API

        GameType:
        Xbox360Game, XboxGameTrial, Xbox360GameContent, Xbox360GameDemo, XboxTheme, XboxOriginalGame,
        XboxGamerTile, XboxArcadeGame, XboxGameConsumable, XboxGameVideo, XboxGameTrailer, XboxBundle, XboxXnaCommunityGame,
        XboxMarketplace, AvatarItem, MobileGame, XboxMobilePDLC, XboxMobileConsumable, WebGame, MetroGame, MetroGameContent,
        MetroGameConsumable, DGame, DGameDemo, DConsumable, DDurable

        AppType: XboxApp, DApp
        MovieType: Movie
        TVType: TVShow (one-off TV shows), TVEpisode, TVSeries, TVSeason
        MusicType: Album, Track, MusicVideo
        MusicArtistType: MusicArtist
        WebVideoType: WebVideo, WebVideoCollection
        EnhancedContentType: GameLayer, GameActivity, AppActivity, VideoLayer, VideoActivity, DActivity, DNativeApp
        SubscriptionType: Subscription
        */
        GameType,
        AppType,
        MovieType,
        TVType,
        MusicType,
        MusicArtistType,
        WebVideoType,
        EnhancedContentType,
        SubscriptionType
    }
}