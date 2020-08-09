using System;
using System.Collections.Generic;

namespace XboxWebApi.Services.Model
{
    public class PeopleResponse
    {
        public int TotalCount;
        public People[] People;

        public ulong[] GetXuids()
        {
            return Array.ConvertAll(People, p => p.Xuid);
        }
    }
}