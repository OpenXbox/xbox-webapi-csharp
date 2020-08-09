using System;
using Microsoft.Extensions.Logging;

namespace XboxWebApi.Common
{
    public static class Logging
    {
        public static ILoggerFactory Factory { get; set; } =
            LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter(logLevel => logLevel >= LogLevel.Information)
                    .AddDebug();
            });
    }
}