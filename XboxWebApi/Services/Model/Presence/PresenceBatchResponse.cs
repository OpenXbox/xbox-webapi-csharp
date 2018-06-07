using System;
using System.Collections.Generic;
using XboxWebApi.Extensions;

namespace XboxWebApi.Services.Model
{
    public class PresenceBatchResponse : List<PresenceResponse>, IStringable
    {
    }
}