using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands.Internal;

public class RecreateInitialMigrationCommand : IConsoleCommand, ITransientDependency
{
    public const string Name = "recreate-initial-migration";

    public ILogger<RecreateInitialMigrationCommand> Logger { get; set; }

    protected CmdHelper CmdHelper { get; }

    public RecreateInitialMigrationCommand(CmdHelper cmdHelper)
    {
        CmdHelper = cmdHelper;
        Logger = NullLogger<RecreateInitialMigrationCommand>.Instance;
    }

    public virtual Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        var csprojFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csproj", SearchOption.AllDirectories)
            .Where(x => x.Contains("templates") || x.Contains("test-app"))
            .Where(x => File.ReadAllText(x).Contains("Microsoft.EntityFrameworkCore.Tools")).ToList();

        var projectCounts = 0;
        foreach (var csprojFile in csprojFiles)
        {
            var projectDir = Path.GetDirectoryName(csprojFile)!;

            if (!Directory.Exists(Path.Combine(projectDir, "Migrations")))
            {
                continue;
            }

            Logger.LogInformation($"Recreating migrations for {csprojFile}");

            if (Directory.Exists(Path.Combine(projectDir, "Migrations")))
            {
                Directory.Delete(Path.Combine(projectDir, "Migrations"), true);
            }

            var separateDbContext = false;
            if (Directory.Exists(Path.Combine(projectDir, "TenantMigrations")))
            {
                Directory.Delete(Path.Combine(projectDir, "TenantMigrations"), true);
                separateDbContext = true;
            }
            if (!separateDbContext)
            {
                CmdHelper.RunCmd($"dotnet ef migrations add Initial", workingDirectory: projectDir);
            }
            else
            {
                CmdHelper.RunCmd($"dotnet ef migrations add Initial --context MyProjectNameDbContext", workingDirectory: projectDir);
                CmdHelper.RunCmd($"dotnet ef migrations add Initial --context MyProjectNameTenantDbContext --output-dir TenantMigrations", workingDirectory: projectDir);
            }

            if (Directory.Exists(Path.Combine(projectDir, "Logs")))
            {
                Directory.Delete(Path.Combine(projectDir, "Logs"), true);
            }

            projectCounts++;
        }

        Logger.LogInformation(projectCounts > 0
            ? $"Done! All {projectCounts} migrations recreated."
            : "No project found to recreate migrations.");

        return Task.CompletedTask;
    }

    public string GetUsageInfo()
    {
        return GetShortDescription();
    }

    public static string GetShortDescription()
    {
        return "This is a internal command. Please run 'abp recreate-initial-migration' command in abp or volo root directory.";
    }
}
