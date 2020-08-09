using System;

namespace XboxWebApi.Authentication.Headless.Model
{
    public class OtcResponse
    {
        public int State { get; set; }
        public string SessionLookupKey { get; set; }
    }
}