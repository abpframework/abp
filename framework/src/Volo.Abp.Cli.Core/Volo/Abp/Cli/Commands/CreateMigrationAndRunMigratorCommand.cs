using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Commands.Services;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands;

public class CreateMigrationAndRunMigratorCommand : IConsoleCommand, ITransientDependency
{
    private readonly InitialMigrationCreator _initialMigrationCreator;
    public const string Name = "create-migration-and-run-migrator";

    public ICmdHelper CmdHelper { get; }
    public DotnetEfToolManager DotnetEfToolManager { get; }
    public ILogger<CreateMigrationAndRunMigratorCommand> Logger { get; set; }

    public CreateMigrationAndRunMigratorCommand(ICmdHelper cmdHelper, InitialMigrationCreator initialMigrationCreator, DotnetEfToolManager dotnetEfToolManager)
    {
        _initialMigrationCreator = initialMigrationCreator;
        CmdHelper = cmdHelper;
        DotnetEfToolManager = dotnetEfToolManager;
        Logger = NullLogger<CreateMigrationAndRunMigratorCommand>.Instance;
    }

    public virtual async Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        if (commandLineArgs.Target.IsNullOrEmpty())
        {
            throw new CliUsageException("DbMigrations folder path is missing!");
        }

        var dbMigrationsFolder = commandLineArgs.Target;

        var nolayers = commandLineArgs.Options.ContainsKey("nolayers");
        var dbMigratorProjectPath = GetDbMigratorProjectPath(dbMigrationsFolder);
        if (!nolayers && dbMigratorProjectPath == null)
        {
            throw new Exception("DbMigrator is not found!");
        }

        await DotnetEfToolManager.BeSureInstalledAsync();

        var migrationsCreatedSuccessfully = await _initialMigrationCreator.CreateAsync(commandLineArgs.Target, !nolayers);

        if (migrationsCreatedSuccessfully)
        {
            if (nolayers)
            {
                CmdHelper.RunCmd("dotnet run --migrate-database", Path.GetDirectoryName(Path.Combine(dbMigrationsFolder, "MyCompanyName.MyProjectName")));
            }
            else
            {
                CmdHelper.RunCmd("dotnet run",  Path.GetDirectoryName(dbMigratorProjectPath));
            }
            await Task.CompletedTask;
        }
        else
        {
            var exceptionMsg = "Migrations failed! A migration command didn't run successfully.";

            Logger.LogError(exceptionMsg);
            throw new Exception(exceptionMsg);
        }
    }
    
    private static string GetDbMigratorProjectPath(string dbMigrationsFolderPath)
    {
        var srcFolder = Directory.GetParent(dbMigrationsFolderPath);
        var dbMigratorDirectory = Directory.GetDirectories(srcFolder.FullName)
            .FirstOrDefault(d => d.EndsWith(".DbMigrator"));

        return dbMigratorDirectory == null
            ? null
            : Directory.GetFiles(dbMigratorDirectory).FirstOrDefault(f => f.EndsWith(".csproj"));
    }

    public string GetUsageInfo()
    {
        return string.Empty;
    }

    public static string GetShortDescription()
    {
        return string.Empty;
    }
}
