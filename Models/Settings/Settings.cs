namespace RunDiscord.Models.Settings
{
    public class Settings
    {
        public readonly string ExplorerProcessName = "explorer";
        public RunDiscordSettings RunDiscordSettings { get; set; }
        public LoggerSettings LoggerSettings { get; set; }
    }
}
