using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using RestSharp;
using XboxWebApi.Common;
using XboxWebApi.Extensions;
using XboxWebApi.Services.Model.CQS;

namespace XboxWebApi.Services.Api
{
    public class CQSService : XblService
    {
        public CQSService(IXblConfiguration config, IRestSharpEx httpClient)
            : base(config, "https://cqs.xboxlive.com", httpClient)
        {
            Headers = new NameValueCollection(){
                {"Cache-Control", "no-cache"},
                {"Accept", "application/json"},
                {"Pragma", "no-cache"},
                {"x-xbl-client-type", "Companion"},
                {"x-xbl-client-version", "2.0"},
                {"x-xbl-contract-version", "1.b"},
                {"x-xbl-device-type", "WindowsPhone"},
                {"x-xbl-isautomated-client", "true"}
            };
        }

        public void GetChannelList(string localeInfo, Guid headendId)
        {
            CQSLineupRequestQuery query = new CQSLineupRequestQuery();
            RestRequestEx request = new RestRequestEx(
                $"epg/{localeInfo}/lineups/{headendId}/channels", Method.GET);
            request.AddHeaders(Headers);
            request.AddQueryParameters(query.GetQuery());

            IRestResponse response = HttpClient.Execute(request);
            Console.WriteLine(response.Content);
        }

        public void GetSchedule(string localeInfo, Guid headendId,
                                DateTime startDate, int durationMinutes,
                                int channelSkip, int channelCount)
        {
            CQSScheduleRequestQuery query = new CQSScheduleRequestQuery(
                startDate, durationMinutes, channelSkip, channelCount);
            RestRequestEx request = new RestRequestEx(
                $"epg/{localeInfo}/lineups/{headendId}/programs", Method.GET);
            request.AddHeaders(Headers);
            request.AddQueryParameters(query.GetQuery());

            IRestResponse response = HttpClient.Execute(request);
            Console.WriteLine(response.Content);
        }
    }
}