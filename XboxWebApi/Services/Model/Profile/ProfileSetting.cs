using System;
using System.ComponentModel;
using XboxWebApi.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XboxWebApi.Services.Model
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProfileSetting
    {
        GameDisplayName,
        AppDisplayName,
        AppDisplayPicRaw,
        GameDisplayPicRaw,
        PublicGamerpic,
        ShowUserAsAvatar,
        Gamerscore,
        Gamertag,
        AccountTier,
        TenureLevel,
        XboxOneRep,
        PreferredColor,
        Location,
        Bio,
        Watermarks,
        RealName,
        RealNameOverride
    }
}