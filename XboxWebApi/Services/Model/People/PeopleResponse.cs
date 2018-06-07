using System;
using System.Collections.Generic;
using XboxWebApi.Extensions;

namespace XboxWebApi.Services.Model
{
    public class PeopleResponse : IStringable
    {
        public int TotalCount;
        public People[] People;

        public ulong[] GetXuids()
        {
            return Array.ConvertAll(People, p => p.Xuid);
        }
    }
}