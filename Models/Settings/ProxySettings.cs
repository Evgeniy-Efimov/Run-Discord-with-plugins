namespace RunDiscord.Models.Settings
{
    public class ProxySettings
    {
        public string Host { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 1080;
        public int TimeoutInMilliseconds { get; set; } = 60000;
    }
}
