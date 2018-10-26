using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using RestSharp;
using XboxWebApi.Common;
using XboxWebApi.Extensions;
using XboxWebApi.Services.Model.X360Marketplace;

namespace XboxWebApi.Services.Api
{
    public class X360CatalogService
    {
        private readonly IRestClient _httpClient;

        public XblLocale Locale { get; internal set ;}

        public string UserAgent;
        public X360CatalogService(XblLocale locale, IRestClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseUrl = new Uri("http://catalog.xboxlive.com");

            Locale = locale;
            UserAgent = "Xbox Live Client/2.0.17526.0";
        }

        public CatalogOverviewResponse GetCatalogOverview()
        {
            CatalogOverviewRequestQuery query = new CatalogOverviewRequestQuery(Locale);
            
            RestRequestEx request = new RestRequestEx(
                $"Catalog/Catalog.asmx/Query", Method.GET);
            List<Tuple<string, string>> q = query.GetQuery();
            // Query contains duplicate keys, hence following add-method
            q.ForEach(x => request.AddQueryParameter(x.Item1, x.Item2));

            IRestResponse<CatalogOverviewResponse> response = _httpClient.Execute<CatalogOverviewResponse>(request);
            return response.Data;
        }
    }
}