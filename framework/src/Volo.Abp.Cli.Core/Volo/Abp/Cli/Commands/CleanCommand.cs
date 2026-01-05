using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Internal.Telemetry;
using Volo.Abp.Internal.Telemetry.Constants;

namespace Volo.Abp.Cli.Commands;

public class CleanCommand : IConsoleCommand, ITransientDependency
{
    public const string Name = "clean";

    public ILogger<CleanCommand> Logger { get; set; }
    
    protected ICmdHelper CmdHelper { get; }
    
    private readonly ITelemetryService _telemetryService;

    public CleanCommand(ICmdHelper cmdHelper, ITelemetryService telemetryService)
    {
        CmdHelper = cmdHelper;
        _telemetryService = telemetryService;
        Logger = NullLogger<CleanCommand>.Instance;
    }

    public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        await using var _ = _telemetryService.TrackActivityAsync(ActivityNameConsts.AbpCliCommandsClean);
        var binEntries = Directory.EnumerateDirectories(Directory.GetCurrentDirectory(), "bin", SearchOption.AllDirectories);
        var objEntries = Directory.EnumerateDirectories(Directory.GetCurrentDirectory(), "obj", SearchOption.AllDirectories);

        Logger.LogInformation("Cleaning the solution with 'dotnet clean' command...");
        CmdHelper.RunCmd($"dotnet clean", workingDirectory: Directory.GetCurrentDirectory());

        Logger.LogInformation($"Removing 'bin' and 'obj' folders...");
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
        Logger.LogInformation($"'bin' and 'obj' folders removed successfully!");

        Logger.LogInformation("Solution cleaned successfully!");
    }

    public string GetUsageInfo()
    {
        var sb = new StringBuilder();

        sb.AppendLine("");
        sb.AppendLine("Usage:");
        sb.AppendLine("  abp clean");
        sb.AppendLine("");
        sb.AppendLine("See the documentation for more info: https://abp.io/docs/latest/cli");

        return sb.ToString();
    }

    public static string GetShortDescription()
    {
        return "Delete all BIN and OBJ folders in current folder.";
    }
}
