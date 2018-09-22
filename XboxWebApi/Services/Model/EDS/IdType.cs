using System;

namespace XboxWebApi.Services.Model.EDS
{
    public enum IdType
    {
        Canonical,  // BING/MARKETPLACE
        XboxHexTitle,
        ScopedMediaId,
        ZuneCatalog,
        ZuneMediaInstance,
        AMG,
        MediaNet,
        ProviderContentId  // NETFLIX/HULU
    }
}