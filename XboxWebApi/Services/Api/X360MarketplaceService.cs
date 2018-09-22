using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using RestSharp;
using XboxWebApi.Common;
using XboxWebApi.Extensions;
using XboxWebApi.Services.Model.X360Marketplace;

namespace XboxWebApi.Services.Api
{
    public class X360MarketplaceService
    {
        public XblLocale Locale { get; internal set ;}

        public string UserAgent;
        public X360MarketplaceService(XblLocale locale)
        {
            Locale = locale;
            UserAgent = "Xbox Live Client/2.0.17526.0";
        }

        public RestClientEx ClientFactory(string baseUrl, JsonNamingStrategy naming)
        {
            RestClientEx client = new RestClientEx(baseUrl, naming);
            client.AddDefaultHeader("User-Agent", UserAgent);
            return client;
        }

        public CatalogItemResponse GetCatalogItem(Guid productId)
        {
            CatalogItemRequestQuery query = new CatalogItemRequestQuery();
            RestClient client = ClientFactory(
                "http://marketplace-xb.xboxlive.com", JsonNamingStrategy.Default);
            RestRequestEx request = new RestRequestEx(
                $"marketplacecatalog/v1/product/{Locale.Locale}/{productId}", Method.GET);
            request.AddQueryParameters(query.GetQuery());
            IRestResponse<CatalogItemResponse> response = client
                .Execute<CatalogItemResponse>(request);
            Console.WriteLine(response.Content);
            return response.Data;
        }

        public CatalogOverviewResponse GetCatalogOverview()
        {
            CatalogOverviewRequestQuery query = new CatalogOverviewRequestQuery(Locale);
            RestClient client = ClientFactory(
                "http://catalog.xboxlive.com", JsonNamingStrategy.Default);
            RestRequestEx request = new RestRequestEx(
                $"Catalog/Catalog.asmx/Query", Method.GET);
            List<Tuple<string, string>> q = query.GetQuery();
            // Query contains duplicate keys, hence following add-method
            q.ForEach(x => request.AddQueryParameter(x.Item1, x.Item2));
            IRestResponse<CatalogOverviewResponse> response = client
                .Execute<CatalogOverviewResponse>(request);
            return response.Data;
        }

        public Uri GetDownloadUrl(string titleId, string xcpFilename)
        {
            RestClient client = ClientFactory(
                "http://download.xboxlive.com", JsonNamingStrategy.Default);
            RestRequestEx request = new RestRequestEx(
                $"content/{titleId}/{xcpFilename}", Method.GET);
            return client.BuildUri(request);
        }
    }
}