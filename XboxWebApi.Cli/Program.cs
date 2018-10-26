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
            FileStream tokenOutputFile = null;
            string responseUrl = null;
            string requestUrl = AuthenticationService.GetWindowsLiveAuthenticationUrl();

            if (args.Length < 1)
            {
                Console.WriteLine("1) Open following URL in your WebBrowser:\n\n{0}\n\n" +
                                    "2) Authenticate with your Microsoft Account\n" +
                                    "3) Paste returned URL from addressbar: \n", requestUrl);
                return;
            }

            if (args.Length == 2)
            {
                string tokenOutputFilePath = args[1];
                try
                {
                    tokenOutputFile = new FileStream(tokenOutputFilePath, FileMode.Create);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to open token outputfile \'{0}\', error: {1}",
                        tokenOutputFile , e.Message);
                    return;
                }
                Console.WriteLine("Storing tokens to file \'{0}\' on successful auth",
                    tokenOutputFilePath);
            }

            responseUrl = args[0];

            WindowsLiveResponse response = AuthenticationService.ParseWindowsLiveResponse(responseUrl);

            AuthenticationService authenticator = new AuthenticationService(
                new AccessToken(response), new RefreshToken(response));

            bool success = authenticator.Authenticate();
            if (!success)
            {
                Console.WriteLine("Authentication failed!");
                return;
            }

            if (tokenOutputFile != null)
            {
                authenticator.DumpToFile(tokenOutputFile);
            }

            Console.WriteLine(authenticator.XToken);
            Console.WriteLine(authenticator.UserInformation);
        }
    }
}
