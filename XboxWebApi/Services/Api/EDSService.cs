using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using XboxWebApi.Common;
using XboxWebApi.Services.Model.EDS;

namespace XboxWebApi.Services.Api
{
    public class EDSService : XblService
    {
        public EDSService(IXblConfiguration config)
            : base(config, "https://eds.xboxlive.com/")
        {
            Headers = new Dictionary<string,string>(){
                {"Cache-Control", "no-cache"},
                {"Accept", "application/json"},
                {"Pragma", "no-cache"},
                {"x-xbl-client-type", "Companion"},
                {"x-xbl-client-version", "2.0"},
                {"x-xbl-contract-version", "3.2"},
                {"x-xbl-device-type", "WindowsPhone"},
                {"x-xbl-isautomated-client", "true"}
            };
        }

        public async Task<HttpResponseMessage> GetChannelListAsync(Guid lineupId)
        {
            EDSLineupRequestQuery query = new EDSLineupRequestQuery(lineupId);
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"media/{this.Config.Locale.Locale}/tvchannels");
            request.Headers.Add(Headers);
            request.AddQueryParameter(query.GetQuery());

            var response = await HttpClient.SendAsync(request);
            return response;
        }

        public async Task<HttpResponseMessage> GetScheduleAsync(string localeInfo, Guid headendId,
                                DateTime startDate, int durationMinutes,
                                int channelSkip, int channelCount)
        {
            EDSScheduleRequestQuery query = new EDSScheduleRequestQuery(
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