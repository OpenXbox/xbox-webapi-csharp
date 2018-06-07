using System;
using System.IO;
using XboxWebApi.Extensions;
using XboxWebApi.Authentication;
using XboxWebApi.Authentication.Model;

namespace XboxWebApi.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            string responseUrl;
            string requestUrl = AuthenticationService.GetWindowsLiveAuthenticationUrl();

            if (args.Length < 1)
            {
                Console.WriteLine("1) Open following URL in your WebBrowser:\n\n{0}\n\n" +
                                    "2) Authenticate with your Microsoft Account\n" +
                                    "3) Paste returned URL from addressbar: \n", requestUrl);

                // Caveat: Console.ReadLine() is limited to 254 chars on Windows
                responseUrl = Console.ReadLine();
            }
            else
            {
                responseUrl = args[0];
            }

            WindowsLiveResponse response = AuthenticationService.ParseWindowsLiveResponse(responseUrl);

            AuthenticationService authenticator = new AuthenticationService(
                new AccessToken(response), new RefreshToken(response));

            bool success = authenticator.Authenticate();
            if (!success)
            {
                Console.WriteLine("Authentication failed!");
                return;
            }
            Console.WriteLine(authenticator.XToken);
            Console.WriteLine(authenticator.UserInformation);
        }
    }
}
