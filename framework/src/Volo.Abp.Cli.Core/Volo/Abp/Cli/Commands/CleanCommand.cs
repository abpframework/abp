using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Args;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands;

public class CleanCommand : IConsoleCommand, ITransientDependency
{
    public const string Name = "clean";
    
    public ILogger<CleanCommand> Logger { get; set; }

    public CleanCommand()
    {
        Logger = NullLogger<CleanCommand>.Instance;
    }

    public Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        var binEntries = Directory.EnumerateDirectories(Directory.GetCurrentDirectory(), "bin", SearchOption.AllDirectories);
        var objEntries = Directory.EnumerateDirectories(Directory.GetCurrentDirectory(), "obj", SearchOption.AllDirectories);

        foreach (var path in binEntries.Concat(objEntries))
        {
            if (path.IndexOf("node_modules", StringComparison.OrdinalIgnoreCase) > 0)
            {
                Logger.LogInformation($"Skipping: {path}");
            }
            else
            {
                Logger.LogInformation($"Deleting: {path}");
                Directory.Delete(path, true);
            }
        }

        Logger.LogInformation($"BIN and OBJ folders have been successfully deleted!");

        return Task.CompletedTask;
    }

    public string GetUsageInfo()
    {
        var sb = new StringBuilder();

        sb.AppendLine("");
        sb.AppendLine("Usage:");
        sb.AppendLine("  abp clean");
        sb.AppendLine("");
        sb.AppendLine("See the documentation for more info: https://docs.abp.io/en/abp/latest/CLI");

        return sb.ToString();
    }

    public string GetShortDescription()
    {
        return "Delete all BIN and OBJ folders in current folder.";
    }
}
