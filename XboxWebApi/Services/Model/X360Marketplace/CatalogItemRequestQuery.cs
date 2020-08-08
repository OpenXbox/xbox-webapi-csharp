using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace XboxWebApi.Services.Model.X360Marketplace
{
    public class CatalogItemRequestQuery : Common.IHttpRequestQuery
    {
        public int[] BodyTypes;
        public int DetailLevel;
        public int PageNum;
        public int PageSize;
        public int Stores;
        public int[] Tiers;
        public int OfferFilter;
        public CategoryId[] ProductTypes;

        public CatalogItemRequestQuery()
        {
            BodyTypes = new int[]{1, 3};
            DetailLevel = 5;
            PageNum = 1;
            PageSize = 1;
            Stores = 1;
            Tiers = new int[]{1, 3};
            OfferFilter = 1;
            ProductTypes = new CategoryId[]
            {
                CategoryId.GamesOnDemand360,
                CategoryId.ArcadeDemo,
                CategoryId.DownloadableContent1,
                CategoryId.FullGameDemos,
                CategoryId.Themes,
                CategoryId.GamesOnDemandClassic,
                CategoryId.Unknown1,
                CategoryId.Arcade,
                CategoryId.Videos,
                CategoryId.GameTrailers,
                CategoryId.IndieGames,
                CategoryId.Unknown2,
                CategoryId.AvatarItems,
                CategoryId.Unknown3
            };
        }

        public Dictionary<string,string> GetQuery()
        {
            return new Dictionary<string,string>()
            {
                {"bodytypes", String.Join(".", Array.ConvertAll(BodyTypes, x => x.ToString()))},
                {"detailview", "detaillevel" + DetailLevel},
                {"pagenum", PageNum.ToString()},
                {"stores", Stores.ToString()},
                {"tiers", String.Join(".", Array.ConvertAll(Tiers, x => x.ToString()))},
                {"offerfilter", OfferFilter.ToString()},
                {"producttypes", String.Join(".", Array.ConvertAll(ProductTypes, x => ((int)x).ToString()))}
            };
        }
    }
}