using System;
using System.IO;
using Microsoft.Extensions.Logging;
using XboxWebApi.Authentication;
using XboxWebApi.Authentication.Model;
using XboxWebApi.Authentication.Headless;

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
        [Verb("headless", HelpText = "Authenticate headless via email/password.")]
        class HeadlessOptions {
            [Option('t', "tokenfile", Required = false, HelpText = "Filepath to save tokens to.")]
            public string TokenFilepath { get; set; }

            [Value(0, MetaName = "email", Required = true, HelpText = "Email address of Microsoft account.")]
            public string Email { get; set; }

            [Value(1, MetaName = "password", Required = true, HelpText = "Password of Microsoft account.")]
            public string Password { get; set; }
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
		    if (!success)
                    	throw new Exception("Authentication failed!");
                    Console.WriteLine("Authentication succeeded");

                    if (!String.IsNullOrEmpty(args.TokenFilepath))
                    {
                        success = await authenticator.DumpToJsonFileAsync(args.TokenFilepath);
                        if (!success)
                        {
                            Console.WriteLine($"Failed to dump tokens to {args.TokenFilepath}");
                            return 2;
                        }
                        Console.WriteLine($"Tokens saved to {args.TokenFilepath}");
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

        static async Task<int> ChooseAuthStrategy(IEnumerable<string> choices)
        {
            Console.WriteLine("Choose desired auth strategy");
            foreach (var choice in choices)
            {
                Console.WriteLine(choice);
            }
            string userChoice = await Console.In.ReadLineAsync();
            return Int32.Parse(userChoice);
        }

        static async Task<string> VerifyProof(string prompt)
        {
            Console.WriteLine(prompt);
            return await Console.In.ReadLineAsync();
        }

        static async Task<string> EnterOneTimeCode(string prompt)
        {
            Console.WriteLine(prompt);
            return await Console.In.ReadLineAsync();
        }

        static async Task<int> RunHeadlessAuth(HeadlessOptions args)
        {
            Console.WriteLine(":: Headless ::");
            var authUrl = AuthenticationService.GetWindowsLiveAuthenticationUrl();
            try
            {
                var headlessAuthService = new HeadlessAuthenticationService(authUrl);
                headlessAuthService.ChooseAuthStrategyCallback = ChooseAuthStrategy;
                headlessAuthService.VerifyPosessionCallback = VerifyProof;
                headlessAuthService.EnterOneTimeCodeCallback = EnterOneTimeCode;

                var response = await headlessAuthService.AuthenticateAsync(args.Email, args.Password);
                var authenticator = new AuthenticationService(response);
                bool success = await authenticator.AuthenticateAsync();
                Console.WriteLine("Authentication succeeded");

                if (!String.IsNullOrEmpty(args.TokenFilepath))
                {
                    success = await authenticator.DumpToJsonFileAsync(args.TokenFilepath);
                    if (!success)
                    {
                        Console.WriteLine($"Failed to dump tokens to {args.TokenFilepath}");
                        return 2;
                    }
                    Console.WriteLine($"Tokens saved to {args.TokenFilepath}");
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("Headless authentication failed, error: " + exc.Message);
                return 1;
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
                Console.WriteLine($"Failed to refresh tokens, error: {exc}");
                return 1;
            }
            return 0;
        }

        async static Task Main(string[] args)
        {
            await CommandLine.Parser.Default.ParseArguments<OAuthOptions, HeadlessOptions, RefreshOptions>(args)
	                    .MapResult(
                            (OAuthOptions opts) => RunOAuth(opts),
                            (HeadlessOptions opts) => RunHeadlessAuth(opts),
                            (RefreshOptions opts) => RunTokenRefresh(opts),
                            errs => Task.FromResult(1));
        }
    }
}
