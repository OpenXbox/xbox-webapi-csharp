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
        private readonly IRestClient _httpClient;

        public XblLocale Locale { get; internal set ;}

        public string UserAgent;
        public X360MarketplaceService(XblLocale locale, IRestClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseUrl = new Uri("http://marketplace-xb.xboxlive.com");

            Locale = locale;
            UserAgent = "Xbox Live Client/2.0.17526.0";
        }

        public CatalogItemResponse GetCatalogItem(Guid productId)
        {
            CatalogItemRequestQuery query = new CatalogItemRequestQuery();
            
            RestRequestEx request = new RestRequestEx(
                $"marketplacecatalog/v1/product/{Locale.Locale}/{productId}", Method.GET);
            request.AddQueryParameters(query.GetQuery());
            
            IRestResponse<CatalogItemResponse> response = _httpClient
                .Execute<CatalogItemResponse>(request);
            Console.WriteLine(response.Content);
            return response.Data;
        }

        public Uri GetDownloadUrl(string titleId, string xcpFilename) =>
            new Uri(
                new Uri("http://download.xboxlive.com"), 
                new Uri($"content/{titleId}/{xcpFilename}")
            );
    }
}