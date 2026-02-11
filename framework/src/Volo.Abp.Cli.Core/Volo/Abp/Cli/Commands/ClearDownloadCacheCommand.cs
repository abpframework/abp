using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Args;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands;

public class ClearDownloadCacheCommand : IConsoleCommand, ITransientDependency
{
    public const string Name = "clear-download-cache";

    public ILogger<ClearDownloadCacheCommand> Logger { get; set; }

    public ClearDownloadCacheCommand()
    {
        Logger = NullLogger<ClearDownloadCacheCommand>.Instance;
    }

    public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        Logger.LogInformation("Clearing the templates download cache...");
        foreach (var cacheFile in Directory.GetFiles(CliPaths.TemplateCache, "*.zip"))
        {
            Logger.LogInformation($"Deleting {cacheFile}");
            try
            {
                File.Delete(cacheFile);
            }
            catch (Exception e)
            {
                Logger.LogError(e, $"Could not delete {cacheFile}");
            }
        }
        Logger.LogInformation("Done.");
        await Task.CompletedTask;
    }

    public string GetUsageInfo()
    {
        var sb = new StringBuilder();

        sb.AppendLine("");
        sb.AppendLine("Usage:");
        sb.AppendLine("  abp clear-download-cache");
        sb.AppendLine("");
        sb.AppendLine("See the documentation for more info: https://abp.io/docs/latest/cli");

        return sb.ToString();
    }

    public static string GetShortDescription()
    {
        return "Clears the templates download cache.";
    }
}
