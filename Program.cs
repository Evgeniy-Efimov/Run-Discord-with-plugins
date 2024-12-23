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
    var exeFileInfo = IOHelper.FindFile(rootFolder, exeFileName);
    
    //Find exe file
    if (exeFileInfo == null)
    {
        throw new Exception($"File '{exeFileName}' not found in '{rootFolder}'");
    }

    logger.LogInformation($"Found exe file path: {exeFileInfo?.FullName}");

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

    Process.Start(settings.ExplorerProcessName, exeFileInfo.FullName);

    logger.LogInformation("Done. Closing console application...");
}
catch (Exception ex)
{
    if (logger != null)
    {
        logger.LogError(ex, "General error");
    }
    else
    {
        Console.WriteLine($"Error: {ex?.Message}");
    }

    Console.ReadLine();
}