namespace RunDiscord.Models.Settings
{
    public class RunDiscordSettings
    {
        public string RootFolder { get; set; }
        public string ExeFileName { get; set; }
        public string PluginFilesRootFolder { get; set; }
        public string PluginFilesNamesString { get; set; }
        public string[] PluginFilesNames
        {
            get
            {
                return (PluginFilesNamesString ?? "")
                    .Split(';')
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToArray();
            }
        }
    };
}
