using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using XboxWebApi.Common;
using XboxWebApi.Services.Model.CQS;

namespace XboxWebApi.Services.Api
{
    public class CQSService : XblService
    {
        public CQSService(IXblConfiguration config)
            : base(config, "https://cqs.xboxlive.com/")
        {
            Headers = new Dictionary<string,string>(){
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

        public async Task<HttpResponseMessage> GetChannelListAsync(string localeInfo, Guid headendId)
        {
            CQSLineupRequestQuery query = new CQSLineupRequestQuery();
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"epg/{localeInfo}/lineups/{headendId}/channels");
            request.Headers.Add(Headers);
            request.AddQueryParameter(query.GetQuery());

            var response = await HttpClient.SendAsync(request);
            return response;
        }

        public async Task<HttpResponseMessage> GetScheduleAsync(string localeInfo, Guid headendId,
                                DateTime startDate, int durationMinutes,
                                int channelSkip, int channelCount)
        {
            CQSScheduleRequestQuery query = new CQSScheduleRequestQuery(
                startDate, durationMinutes, channelSkip, channelCount);
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"epg/{localeInfo}/lineups/{headendId}/programs");
            request.Headers.Add(Headers);
            request.AddQueryParameter(query.GetQuery());

            var response = await HttpClient.SendAsync(request);
            return response;
        }
    }
}