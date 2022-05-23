using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands.Services;

public class InitialMigrationCreator : ITransientDependency
{
    public ICmdHelper CmdHelper { get; }
    public DotnetEfToolManager DotnetEfToolManager { get; }
    public ILogger<InitialMigrationCreator> Logger { get; set; }
    
    public InitialMigrationCreator(ICmdHelper cmdHelper, DotnetEfToolManager dotnetEfToolManager)
    {
        CmdHelper = cmdHelper;
        DotnetEfToolManager = dotnetEfToolManager;

        Logger = NullLogger<InitialMigrationCreator>.Instance;
    }

    public async Task<bool> CreateAsync(string targetProjectFolder, bool layeredTemplate = true)
    {
        if (targetProjectFolder == null || !Directory.Exists(targetProjectFolder))
        {
            Logger.LogError($"This path doesn't exist: {targetProjectFolder}");
            return false;
        }
        
        Logger.LogInformation("Creating initial migrations...");

        await DotnetEfToolManager.BeSureInstalledAsync();
        
        var tenantDbContextName = FindTenantDbContextName(targetProjectFolder);
        var dbContextName = tenantDbContextName != null ?
            FindDbContextName(targetProjectFolder)
            : null;

        var migrationOutput = AddMigrationAndGetOutput(targetProjectFolder, dbContextName, "Migrations");
        var tenantMigrationOutput = tenantDbContextName != null ?
            AddMigrationAndGetOutput(targetProjectFolder, tenantDbContextName, "TenantMigrations")
            : null;

        var migrationSuccess = CheckMigrationOutput(migrationOutput) && CheckMigrationOutput(tenantMigrationOutput);

        if (migrationSuccess)
        {
            Logger.LogInformation("Initial migrations are created.");
        }
        else
        {
            Logger.LogError("Creating initial migrations process is failed! Details:" + Environment.NewLine
                + migrationOutput + Environment.NewLine
                + tenantMigrationOutput + Environment.NewLine);
        }

        return migrationSuccess;
    }
    
    private string FindTenantDbContextName(string projectFolder)
    {
        var tenantDbContext = Directory.GetFiles(projectFolder, "*TenantMigrationsDbContext.cs", SearchOption.AllDirectories)
                                  .FirstOrDefault() ??
                              Directory.GetFiles(projectFolder, "*TenantDbContext.cs", SearchOption.AllDirectories)
                                  .FirstOrDefault();

        if (tenantDbContext == null)
        {
            return null;
        }

        return Path.GetFileName(tenantDbContext).RemovePostFix(".cs");
    }

    private string FindDbContextName(string projectFolder)
    {
        var dbContext = Directory.GetFiles(projectFolder, "*MigrationsDbContext.cs", SearchOption.AllDirectories)
                            .FirstOrDefault(fp => !fp.EndsWith("TenantMigrationsDbContext.cs")) ??
                        Directory.GetFiles(projectFolder, "*DbContext.cs", SearchOption.AllDirectories)
                            .FirstOrDefault(fp => !fp.EndsWith("TenantDbContext.cs"));

        if (dbContext == null)
        {
            return null;
        }

        return Path.GetFileName(dbContext).RemovePostFix(".cs");
    }

    private string AddMigrationAndGetOutput(string dbMigrationsFolder, string dbContext, string outputDirectory)
    {
        var dbContextOption = string.IsNullOrWhiteSpace(dbContext)
            ? string.Empty
            : $"--context {dbContext}";

        var addMigrationCmd = $"dotnet ef migrations add Initial --output-dir {outputDirectory} {dbContextOption}";

        return CmdHelper.RunCmdAndGetOutput(addMigrationCmd, out int exitCode, dbMigrationsFolder);
    }

    private static bool CheckMigrationOutput(string output)
    {
        return output == null || (output.Contains("Done.") &&
                                  output.Contains("To undo this action") &&
                                  output.Contains("ef migrations remove"));
    }
}