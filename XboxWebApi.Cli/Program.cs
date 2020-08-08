using System;
using System.IO;
using Microsoft.Extensions.Logging;
using XboxWebApi.Authentication;
using XboxWebApi.Authentication.Model;

using CommandLine;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace XboxWebApi.Cli
{
    class Program
    {
        static ILogger logger = XboxWebApi.Common.Logging.Factory.CreateLogger<Program>();
        
        [Verb("oauth", HelpText = "Authenticate via webbrowser / OAUTH2.")]
        class OAuthOptions {
            [Option('t', "tokenfile", Required = false, HelpText = "Filepath to save tokens to.")]
            public string TokenFilepath { get; set; }

            [Value(0, MetaName = "responseurl", HelpText = "Response URL.")]
            public string ResponseUrl{ get; set; }
        }
        [Verb("refresh", HelpText = "Refresh tokens via refresh token json.")]
        class RefreshOptions {
            [Value(0, MetaName = "tokenfile", Required = true, HelpText = "Filepath to load/save tokens.")]
            public string TokenFilepath { get; set; }
        }
        
        static async Task<int> RunOAuth(OAuthOptions args)
        {
            Console.WriteLine(":: OAUTH ::");
            if (String.IsNullOrEmpty(args.ResponseUrl))
            {
                string requestUrl = AuthenticationService.GetWindowsLiveAuthenticationUrl();
                Console.WriteLine("1) Open following URL in your WebBrowser:\n\n{0}\n\n" +
                                    "2) Authenticate with your Microsoft Account\n" +
                                    "3) Execute application again with returned URL from addressbar as the argument\n", requestUrl);
            }
            else
            {
                try
                {
                    WindowsLiveResponse response = AuthenticationService.ParseWindowsLiveResponse(args.ResponseUrl);
                    AuthenticationService authenticator = new AuthenticationService(response);

                    Console.WriteLine("Attempting authentication with Xbox Live...");
                    bool success = await authenticator.AuthenticateAsync();
                    Console.WriteLine("Authentication succeeded");

                    if (!String.IsNullOrEmpty(args.TokenFilepath))
                    {
                        success = await authenticator.DumpToJsonFileAsync(args.TokenFilepath);
                        if (!success)
                        {
                            Console.WriteLine("Failed to dump tokens to {}", args.TokenFilepath);
                            return 2;
                        }
                        Console.WriteLine("Tokens saved to {}", args.TokenFilepath);
                    }
                }
                catch (Exception exc)
                {
                    Console.WriteLine("Authentication failed! Error: " + exc.Message);
                    return 1;
                }
            }

            return 0;
        }

        static async Task<int> RunTokenRefresh(RefreshOptions args)
        {
            Console.WriteLine(":: Token refresh ::");
            try
            {
                Console.WriteLine("Loading tokens from file...");
                var authService = await AuthenticationService.LoadFromJsonFileAsync(args.TokenFilepath);
                Console.WriteLine("Refreshing tokens...");
                await authService.AuthenticateAsync();
                Console.WriteLine("Saving refreshed tokens to file...");
                await authService.DumpToJsonFileAsync(args.TokenFilepath);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Failed to refresh tokens, error: {}", exc);
                return 1;
            }
            return 0;
        }

        async static Task Main(string[] args)
        {
            await CommandLine.Parser.Default.ParseArguments<OAuthOptions, RefreshOptions>(args)
	                    .MapResult(
                            (OAuthOptions opts) => RunOAuth(opts),
                            (RefreshOptions opts) => RunTokenRefresh(opts),
                            errs => Task.FromResult(1));
        }
    }
}
