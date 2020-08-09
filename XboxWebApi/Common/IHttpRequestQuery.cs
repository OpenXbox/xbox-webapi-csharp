using System;
using System.Collections.Generic;

namespace XboxWebApi.Common
{
    public interface IHttpRequestQuery
    {
        Dictionary<string,string> GetQuery();
    }
}