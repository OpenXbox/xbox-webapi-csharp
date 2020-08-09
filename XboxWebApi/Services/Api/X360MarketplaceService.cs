using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;
using XboxWebApi.Common;
using XboxWebApi.Services.Model.X360Marketplace;

namespace XboxWebApi.Services.Api
{
    public class X360MarketplaceService
    {
        private readonly HttpClient _httpClient;

        public XblLocale Locale { get; internal set ;}

        public string UserAgent;
        public X360MarketplaceService(XblLocale locale)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://marketplace-xb.xboxlive.com/");

            Locale = locale;
            UserAgent = "Xbox Live Client/2.0.17526.0";
        }

        public async Task<CatalogItemResponse> GetCatalogItemAsync(Guid productId)
        {
            CatalogItemRequestQuery query = new CatalogItemRequestQuery();
            
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"marketplacecatalog/v1/product/{Locale.Locale}/{productId}");
            request.AddQueryParameter(query.GetQuery());
            
            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsJsonAsync<CatalogItemResponse>();
        }

        public Uri GetDownloadUrl(string titleId, string xcpFilename) =>
            new Uri(
                new Uri("http://download.xboxlive.com"), 
                new Uri($"content/{titleId}/{xcpFilename}")
            );
    }
}