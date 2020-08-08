using System;

namespace XboxWebApi.Authentication.Headless.Model
{
    public class TwoFactorAuthStrategy
    {
        public string Data { get; set; }
        public TwoFactorAuthMethod Type { get; set; }
        public string ClearDigits { get; set; }
        public string CtryISO { get; set; }
        public string Display { get; set; }
        public bool OtcEnabled { get; set; }
        public bool OtcSent { get; set; }
        public bool IsLost { get; set; }
        public bool IsSleeping { get; set; }
        public bool IsSADef { get; set; }
        public bool IsVoiceDef { get; set; }
        public bool IsVoiceOnly { get; set; }
        public bool PushEnabled { get; set; }
    }
}