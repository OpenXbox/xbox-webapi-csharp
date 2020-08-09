using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using XboxWebApi.Common;
using XboxWebApi.Services.Model.X360Marketplace;

namespace XboxWebApi.Services.Api
{
    public class X360CatalogService
    {
        private readonly HttpClient _httpClient;

        public XblLocale Locale { get; internal set ;}

        public string UserAgent;
        public X360CatalogService(XblLocale locale)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://catalog.xboxlive.com/");

            Locale = locale;
            UserAgent = "Xbox Live Client/2.0.17526.0";
        }

        public async Task<CatalogOverviewResponse> GetCatalogOverviewAsync()
        {
            CatalogOverviewRequestQuery query = new CatalogOverviewRequestQuery(Locale);
            
            var request = new HttpRequestMessage(HttpMethod.Get, $"Catalog/Catalog.asmx/Query");
            List<Tuple<string, string>> q = query.GetQuery();
            // Query contains duplicate keys, hence following add-method
            q.ForEach(x => request.AddQueryParameter(x.Item1, x.Item2));

            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsJsonAsync<CatalogOverviewResponse>();
        }
    }
}