using System;

namespace XboxWebApi.Authentication.Headless.Model
{
    /// <summary>
    /// Enumeration of possible Two-Factor-Authentication Session-States
    /// </summary>
    public enum TwoFactorAuthSessionState
    {
        ERROR = 0,
        REJECTED = 1, // GIF 2x2
        PENDING = 2,  // GIF 1x1
        APPROVED = 3  // GIF 1x2
    }
}
