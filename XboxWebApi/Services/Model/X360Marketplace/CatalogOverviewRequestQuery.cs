using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace XboxWebApi.Services.Model.X360Marketplace
{
    public class CatalogOverviewRequestQuery
    {
        public XblLocale Locale;
        public string MethodName;
        public int Store;
        public int PageSize;
        public int PageNum;
        public CatalogDetailLevel DetailView;
        public int OfferFilterLevel;
        public int CategoryIDs;
        public int OrderBy;
        public int OrderDirection;
        public int ImageFormats;
        public int ImageSizes;
        public int UserTypes;
        public int MediaTypes;

        public CatalogOverviewRequestQuery(XblLocale locale)
        {
            Locale = locale;
            MethodName = "FindGames";
            Store = 1;
            PageSize = 25;
            PageNum = 1;
            DetailView = CatalogDetailLevel.IncludeBoxart;
            OfferFilterLevel = 1;
            CategoryIDs = 3027;
            OrderBy = 1;
            OrderDirection = 1;
            ImageFormats = 5;
            ImageSizes = 15;
            UserTypes = 1;
            MediaTypes = 23;
        }

        public List<Tuple<string, string>> GetQuery()
        {
            List<Tuple<string, string>> tup =  new List<Tuple<string, string>>();

            tup.Add(new Tuple<string, string>("methodName", MethodName));
            tup.Add(new Tuple<string, string>("Names", "Locale"));
            tup.Add(new Tuple<string, string>("Values", Locale.Locale));
            tup.Add(new Tuple<string, string>("Names", "LegalLocale"));
            tup.Add(new Tuple<string, string>("Values", Locale.Locale));
            tup.Add(new Tuple<string, string>("Names", "Store"));
            tup.Add(new Tuple<string, string>("Values", Store.ToString()));
            tup.Add(new Tuple<string, string>("Names", "PageSize"));
            tup.Add(new Tuple<string, string>("Values", PageSize.ToString()));
            tup.Add(new Tuple<string, string>("Names", "PageNum"));
            tup.Add(new Tuple<string, string>("Values", PageNum.ToString()));
            tup.Add(new Tuple<string, string>("Names", "DetailView"));
            tup.Add(new Tuple<string, string>("Values", ((int)DetailView).ToString()));
            tup.Add(new Tuple<string, string>("Names", "OfferFilterLevel"));
            tup.Add(new Tuple<string, string>("Values", OfferFilterLevel.ToString()));
            tup.Add(new Tuple<string, string>("Names", "CategoryIDs"));
            tup.Add(new Tuple<string, string>("Values", CategoryIDs.ToString()));
            tup.Add(new Tuple<string, string>("Names", "OrderBy"));
            tup.Add(new Tuple<string, string>("Values", OrderBy.ToString()));
            tup.Add(new Tuple<string, string>("Names", "OrderDirection"));
            tup.Add(new Tuple<string, string>("Values", OrderDirection.ToString()));
            tup.Add(new Tuple<string, string>("Names", "ImageFormats"));
            tup.Add(new Tuple<string, string>("Values", ImageFormats.ToString()));
            tup.Add(new Tuple<string, string>("Names", "ImageSizes"));
            tup.Add(new Tuple<string, string>("Values", ImageSizes.ToString()));
            tup.Add(new Tuple<string, string>("Names", "UserTypes"));
            tup.Add(new Tuple<string, string>("Values", UserTypes.ToString()));
            tup.Add(new Tuple<string, string>("Names", "MediaTypes"));
            tup.Add(new Tuple<string, string>("Values", MediaTypes.ToString()));
            return tup;
        }
    }
}