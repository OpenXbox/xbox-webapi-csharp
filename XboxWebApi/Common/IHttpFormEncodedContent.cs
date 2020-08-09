using System;
using System.Collections.Generic;

namespace XboxWebApi.Common
{
    public interface IHttpFormEncodedContent
    {
        Dictionary<string,string> GetFormContent();
    }
}