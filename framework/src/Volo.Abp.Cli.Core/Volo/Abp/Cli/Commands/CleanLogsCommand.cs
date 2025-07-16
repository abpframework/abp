using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.Cli.Args;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands;

public class CleanLogsCommand : IConsoleCommand, ITransientDependency
{
    public const string Name = "clean-logs";

    public ILogger<CleanCommand> Logger { get; set; }

    public CleanLogsCommand(ILogger<CleanCommand> logger)
    {
        Logger = logger;
    }

    public Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        var logsEntries = Directory.EnumerateDirectories(Directory.GetCurrentDirectory(), "Logs", SearchOption.AllDirectories);

        Logger.LogInformation($"Removing 'Logs' files...");
        foreach (var path in logsEntries)
        {
            var files = Directory.GetFiles(path, "*logs.txt");

            foreach (var file in files)
            {
                Logger.LogInformation($"Deleting: {file}");
                File.Delete(file);
            }
        }
        Logger.LogInformation($"'Logs' files removed successfully!");

        Logger.LogInformation("Logs cleaned successfully!");
        return Task.CompletedTask;
    }

    public string GetUsageInfo()
    {
        var sb = new StringBuilder();

        sb.AppendLine("");
        sb.AppendLine("Usage:");
        sb.AppendLine("  abp clean-logs");
        sb.AppendLine("");
        sb.AppendLine("See the documentation for more info: https://abp.io/docs/latest/cli");

        return sb.ToString();
    }

    public static string GetShortDescription()
    {
        return "Delete all *logs.txt files in current folder.";
    }
}