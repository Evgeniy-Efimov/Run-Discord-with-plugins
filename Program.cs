using Microsoft.Extensions.Logging;
using RunDiscord.Helpers;
using System.Diagnostics;

ILogger<Program> logger = null;

try
{
    var settings = SettingsHelper.GetSettings();

    LogsHelper.InitLogger(settings.LoggerSettings);
    logger = LogsHelper.GetLogger<Program>();

    var rootFolder = settings.RunDiscordSettings.RootFolder;
    var exeFileName = settings.RunDiscordSettings.ExeFileName;
    var pluginFilesNames = settings.RunDiscordSettings.PluginFilesNames;
    var pluginFilesRootFolder = settings.RunDiscordSettings.PluginFilesRootFolder;
    var proxySettings = settings.ProxySettings;
    var exeFilesInfo = IOHelper.FindFiles(rootFolder, exeFileName);
    
    //Find exe file
    if (exeFilesInfo == null || exeFilesInfo.Count < 1)
    {
        throw new Exception($"File '{exeFileName}' not found in '{rootFolder}'");
    }

    var exeFileInfo = exeFilesInfo
        .ToDictionary(k => k.FullName, v => v)
        .OrderByDescending(i => i.Key).First().Value;
    logger.LogInformation($"Found latest exe file path: {exeFileInfo?.FullName}");

    //Copy plugin files
    foreach (var pluginFileName in pluginFilesNames)
    {
        var sourceFileInfo = new FileInfo(Path.Combine(pluginFilesRootFolder, pluginFileName));
        var targetFileInfo = new FileInfo(Path.Combine(exeFileInfo.Directory.FullName, sourceFileInfo.Name));

        if (!sourceFileInfo.Exists)
        {
            throw new Exception($"Plugin file not found: {sourceFileInfo.FullName}");
        }

        if (targetFileInfo.Exists)
        {
            logger.LogInformation($"Plugin file already exists: {targetFileInfo.FullName}");
        }
        else
        {
            sourceFileInfo.CopyTo(targetFileInfo.FullName);
            logger.LogInformation($"Plugin file copied: {targetFileInfo.FullName}");
        }
    }

    logger.LogInformation($"Wait for proxy at {proxySettings.Host}:{proxySettings.Port}...");
    ProxyHelper.WaitForProxyAsync(proxySettings).Wait();

    logger.LogInformation("Done. Start application...");
    Process.Start(settings.ExplorerProcessName, exeFileInfo.FullName);

    logger.LogInformation("Done. Closing console...");
}
catch (Exception ex)
{
    if (logger != null)
    {
        logger.LogError(ex, "Error");
    }
    
    Console.WriteLine($"Error: {ex?.Message}");
    Console.ReadLine();
}