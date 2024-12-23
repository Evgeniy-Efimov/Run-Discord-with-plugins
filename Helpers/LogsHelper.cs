using Microsoft.Extensions.Logging;
using RunDiscord.Models.Settings;
using Serilog;

namespace RunDiscord.Helpers
{
    public static class LogsHelper
    {
        private static ILoggerFactory _loggerFactory;

        public static void InitLogger(LoggerSettings loggerSettings)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File(loggerSettings.LogFilesPath, 
                    rollingInterval: RollingInterval.Day, 
                    retainedFileCountLimit: loggerSettings.MaximumFilesCount)
                .CreateLogger();

            _loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });
        }

        public static ILogger<T> GetLogger<T>()
        {
            return _loggerFactory.CreateLogger<T>();
        }
    }
}
