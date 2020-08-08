# Xbox WebAPI library for .NET

[![GitHub Workflow - Build](https://img.shields.io/github/workflow/status/OpenXbox/xbox-webapi-csharp/build?label=build)](https://github.com/OpenXbox/xbox-webapi-csharp/actions?query=workflow%3Abuild)
[![NuGet](https://img.shields.io/nuget/v/OpenXbox.XboxWebApi.svg)](https://www.nuget.org/packages/OpenXbox.XboxWebApi)
[![Discord](https://img.shields.io/badge/discord-OpenXbox-blue.svg)](https://discord.gg/E8kkJhQ)

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

AuthenticationService authenticator = new AuthenticationService(response);

string tokenFilePath = "tokens.json";
bool success = await authenticator.Authenticate();
if (!success)
    throw new Exception("Authentication failed!");

success = await authenticator.DumpToJsonFileAsync(tokenFilePath);
if (!success)
    throw new Exception("Failed to dump tokens");

Console.WriteLine("Tokens saved to {}", tokenFilePath);

Console.WriteLine(authenticator.XToken);
Console.WriteLine(authenticator.UserInformation);
```

Save token to JSON

```cs
using XboxWebApi.Authentication;

await authenticator.DumpToJsonFileAsync(tokenFilePath);
```

Load token from JSON

```cs
using XboxWebApi.Authentication;

AuthenticationService authenticator = await AuthenticationService.LoadFromJsonFileAsync("tokens.json");
```

Example Api Usage

```cs
using System;
using XboxWebApi.Common;
using XboxWebApi.Services;
using XboxWebApi.Services.Api;
using XboxWebApi.Services.Model;
using XboxWebApi.Authentication;

if (!authenticator.XToken.Valid)
{
    Console.WriteLine("Token expired, please refresh / reauthenticate");
    return;
}

XblConfiguration xblConfig = new XblConfiguration(authenticator.XToken, XblLanguage.United_States);

PresenceService presenceService = new PresenceService(xblConfig);
PeopleService peopleService = new PeopleService(xblConfig);
MessageService messageService = new MessageService(xblConfig);
// ... more services

var friends = await peopleService.GetFriendsAsync();
var presenceBatch = await presenceService.GetPresenceBatchAsync(friends.GetXuids());
for (int i = 0; i < friends.TotalCount; i++)
{
    Console.WriteLine($"{presenceBatch[i].Xuid} is {presenceBatch[i].State}");
}
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
