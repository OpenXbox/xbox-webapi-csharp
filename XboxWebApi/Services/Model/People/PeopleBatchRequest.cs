using System;

namespace XboxWebApi.Services.Model
{
    public class PeopleBatchRequest
    {
        public string[] Xuids;

        public PeopleBatchRequest(ulong[] xuids)
        {
            Xuids = Array.ConvertAll(xuids, x=>x.ToString());
        }
    }
}