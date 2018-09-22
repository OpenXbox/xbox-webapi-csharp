using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using RestSharp;
using XboxWebApi.Common;
using XboxWebApi.Extensions;
using XboxWebApi.Services.Model.EDS;

namespace XboxWebApi.Services.Api
{
    public class EDSService : XblService
    {
        public EDSService(XblConfiguration config)
            : base(config, "https://eds.xboxlive.com")
        {
            Headers = new NameValueCollection(){
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

        public void GetChannelList(Guid lineupId)
        {
            EDSLineupRequestQuery query = new EDSLineupRequestQuery(lineupId);
            RestRequestEx request = new RestRequestEx(
                $"media/{this.Config.Locale.Locale}/tvchannels", Method.GET);
            request.AddHeaders(Headers);
            request.AddQueryParameters(query.GetQuery());
            IRestResponse response = ClientFactory(JsonNamingStrategy.CamelCase)
                .Execute(request);
            Console.WriteLine(response.Content);
        }

        public void GetSchedule(string localeInfo, Guid headendId,
                                DateTime startDate, int durationMinutes,
                                int channelSkip, int channelCount)
        {
            EDSScheduleRequestQuery query = new EDSScheduleRequestQuery(
                startDate, durationMinutes, channelSkip, channelCount);
            RestRequestEx request = new RestRequestEx(
                $"epg/{localeInfo}/lineups/{headendId}/programs", Method.GET);
            request.AddHeaders(Headers);
            request.AddQueryParameters(query.GetQuery());
            IRestResponse response = ClientFactory(JsonNamingStrategy.CamelCase)
                .Execute(request);
            Console.WriteLine(response.Content);
        }
    }
}