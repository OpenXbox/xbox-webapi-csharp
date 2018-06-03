# Xbox WebAPI library for .NET

[![Build status](https://ci.appveyor.com/api/projects/status/8nhploewqrf5atdl/branch/master?svg=true)](https://ci.appveyor.com/project/tuxuser/xbox-webapi-csharp/branch/master)
[![NuGet](https://img.shields.io/nuget/v/OpenXbox.XboxWebApi.svg)](https://www.nuget.org/packages/OpenXbox.XboxWebApi)

C# Xbox WebAPI, includes support for authentication with Windows Live / Xbox Live.

## Usage

Basic authentication flow

```cs
using System;
using XboxWebApi.Authentication;
using XboxWebApi.Authentication.Model;

// ...

string requestUrl = AuthenticationService.GetWindowsLiveAuthenticationUrl();

/* Call requestUrl via WebWidget or manually and authenticate */

WindowsLiveResponse response = AuthenticationService.ParseWindowsLiveResponse(
    "<Received Redirection URL>");

AuthenticationService authenticator = new AuthenticationService(
    new AccessToken(response), new RefreshToken(response));

if (!authenticator.Authenticate())
    throw new Exception("Authentication failed!");

Console.WriteLine(authenticator.XToken);
Console.WriteLine(authenticator.UserInformation);
```

## Documentation

Not yet, please look at `XboxWebApi.Tests` for now.

## Credits

Informations on endpoints gathered from:

* [joealcorn/xbox](https://github.com/joealcorn/xbox)
* [XboxLive REST Reference](https://docs.microsoft.com/en-us/windows/uwp/xbox-live/xbox-live-rest/atoc-xboxlivews-reference)
* [XboxLiveTraceAnalyzer APIMap](https://github.com/Microsoft/xbox-live-trace-analyzer/blob/master/Source/XboxLiveTraceAnalyzer.APIMap.csv)
* [Xbox Live Service API](https://github.com/Microsoft/xbox-live-api)

## Disclaimer

Xbox, Xbox One, Smartglass and Xbox Live are trademarks of Microsoft Corporation.
Team OpenXbox is in no way endorsed by or affiliated with Microsoft Corporation, or
any associated subsidiaries, logos or trademarks.
