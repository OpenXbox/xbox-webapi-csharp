using System;
using XboxWebApi.Extensions;

namespace XboxWebApi.Services.Model
{
    public class PagingInfo : IStringable
    {
        public string ContinuationToken;
        public int TotalItems;
    }
}