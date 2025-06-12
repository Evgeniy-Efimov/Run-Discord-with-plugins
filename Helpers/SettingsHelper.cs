using Microsoft.Extensions.Configuration;
using RunDiscord.Models.Settings;

namespace RunDiscord.Helpers
{
    public static class SettingsHelper
    {
        private const string SettingsFileName = "appsettings.json";

        public static Settings GetSettings()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(SettingsFileName, optional: false, reloadOnChange: true)
                .Build();

            var runDiscordSettings = new RunDiscordSettings();
            var loggerSettings = new LoggerSettings();
            var proxySettings = new ProxySettings();

            configuration.GetSection(nameof(RunDiscordSettings)).Bind(runDiscordSettings);
            configuration.GetSection(nameof(LoggerSettings)).Bind(loggerSettings);
            configuration.GetSection(nameof(ProxySettings)).Bind(proxySettings);

            return new Settings() 
            {
                RunDiscordSettings = runDiscordSettings,
                LoggerSettings = loggerSettings,
                ProxySettings = proxySettings
            };
        }
    }
}
