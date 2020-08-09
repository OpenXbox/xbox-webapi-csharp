using System;

namespace XboxWebApi.Authentication.Headless.Model
{
    public enum TwoFactorAuthMethod
    {
        Voice = -3,
        Unknown = 0,
        Email = 1,
        AltEmail = 2,
        SMS = 3,
        DeviceId = 4,
        CSS = 5,
        SQSA = 6,
        HIP = 8,
        Birthday = 9,
        TOTPAuthenticator = 10,
        RecoveryCode = 11,
        StrongTicket = 13,
        TOTPAuthenticatorV2 = 14,
        UniversalSecondFactor = 15
    }
}