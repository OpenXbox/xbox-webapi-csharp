using System;
using System.IO;
using System.Collections.Generic;
using NUnit.Framework;
using XboxWebApi.Common;
using XboxWebApi.Authentication;
using XboxWebApi.Authentication.Model;

namespace XboxWebApi.UnitTests
{
    public static class TestConstants
    {
        public static XboxUserInformation TestUserInfo
        {
            get
            {
                return new XboxUserInformation()
                {
                    Gamertag = "TestGamertag",
                    Userhash = "12345678910",
                    XboxUserId = 250176543210,
                    AgeGroup = "Adult",
                    Privileges = "100",
                    UserTitleRestrictions = "101",
                    UserSettingsRestrictions = "102"
                };
            }
        }
        public static XToken TestXToken
        {
            get
            {
                return new XToken()
                {
                    Issued = new DateTime(2005, 11, 22),
                    Expires = new DateTime(2099, 11, 22),
                    Jwt = "eyABCDEFGHIJKLMNOPQRSTUVWXY=",
                    UserInformation = TestUserInfo
                };
            }
        }
    }
}