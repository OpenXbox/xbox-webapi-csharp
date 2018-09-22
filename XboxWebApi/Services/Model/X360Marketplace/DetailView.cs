using System;

namespace XboxWebApi.Services.Model.X360Marketplace
{
    public enum CatalogDetailLevel
    {
        // Id, Name, updated date, Game content type, Media Id
        Minimal = 1,
        IncludeCategories = 2,
        IncludeBoxart = 3,
        IncludeScreenshots = 4,
        Full = 5
    }
}