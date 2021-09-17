using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands
{
    public class CreateMigrationAndRunMigratorCommand : IConsoleCommand, ITransientDependency
    {
        public ILogger<CreateMigrationAndRunMigratorCommand> Logger { get; set; }

        public CreateMigrationAndRunMigratorCommand()
        {
            Logger = NullLogger<CreateMigrationAndRunMigratorCommand>.Instance;
        }

        public virtual async Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            if (commandLineArgs.Target.IsNullOrEmpty())
            {
                throw new CliUsageException("DbMigrations folder path is missing!");
            }

            var dbMigrationsFolder = commandLineArgs.Target;

            var dbMigratorProjectPath = GetDbMigratorProjectPath(dbMigrationsFolder);
            if (dbMigratorProjectPath == null)
            {
                throw new Exception("DbMigrator is not found!");
            }

            if (!IsDotNetEfToolInstalled())
            {
                InstallDotnetEfTool();
            }

            var tenantDbContextName = FindTenantDbContextName(dbMigrationsFolder);
            var dbContextName = tenantDbContextName != null ?
                FindDbContextName(dbMigrationsFolder)
                : null;

            var migrationOutput = AddMigrationAndGetOutput(dbMigrationsFolder, dbContextName, "Migrations");
            var tenantMigrationOutput = tenantDbContextName != null ?
                AddMigrationAndGetOutput(dbMigrationsFolder, tenantDbContextName, "TenantMigrations")
                : null;

            if (CheckMigrationOutput(migrationOutput) && CheckMigrationOutput(tenantMigrationOutput))
            {
                // Migration added successfully
                CmdHelper.RunCmd("cd \"" + Path.GetDirectoryName(dbMigratorProjectPath) + "\" && dotnet run");
                await Task.CompletedTask;
            }
            else
            {
                var exceptionMsg = "Migrations failed! A migration command didn't run successfully:" +
                                   Environment.NewLine +
                                   Environment.NewLine + migrationOutput +
                                   Environment.NewLine +
                                   Environment.NewLine + tenantMigrationOutput;

                Logger.LogError(exceptionMsg);
                throw new Exception(exceptionMsg);
            }
        }

        private string FindTenantDbContextName(string dbMigrationsFolder)
        {
            var tenantDbContext = Directory.GetFiles(dbMigrationsFolder, "*TenantMigrationsDbContext.cs", SearchOption.AllDirectories)
                                      .FirstOrDefault() ??
                                  Directory.GetFiles(dbMigrationsFolder, "*TenantDbContext.cs", SearchOption.AllDirectories)
                                      .FirstOrDefault();

            if (tenantDbContext == null)
            {
                return null;
            }

            return Path.GetFileName(tenantDbContext).RemovePostFix(".cs");
        }

        private string FindDbContextName(string dbMigrationsFolder)
        {
            var dbContext = Directory.GetFiles(dbMigrationsFolder, "*MigrationsDbContext.cs", SearchOption.AllDirectories)
                                .FirstOrDefault(fp => !fp.EndsWith("TenantMigrationsDbContext.cs")) ??
                            Directory.GetFiles(dbMigrationsFolder, "*DbContext.cs", SearchOption.AllDirectories)
                                .FirstOrDefault(fp => !fp.EndsWith("TenantDbContext.cs"));

            if (dbContext == null)
            {
                return null;
            }

            return Path.GetFileName(dbContext).RemovePostFix(".cs");
        }

        private static string AddMigrationAndGetOutput(string dbMigrationsFolder, string dbContext, string outputDirectory)
        {
            var dbContextOption = string.IsNullOrWhiteSpace(dbContext)
                ? string.Empty
                : $"--context {dbContext}";

            var addMigrationCmd = $"cd \"{dbMigrationsFolder}\" && " +
                                  $"dotnet ef migrations add Initial --output-dir {outputDirectory} {dbContextOption}";

            return CmdHelper.RunCmdAndGetOutput(addMigrationCmd);
        }

        private static bool IsDotNetEfToolInstalled()
        {
            var output = CmdHelper.RunCmdAndGetOutput("dotnet tool list -g");
            return output.Contains("dotnet-ef");
        }

        private static bool CheckMigrationOutput(string output)
        {
            return output == null || (output.Contains("Done.") &&
                           output.Contains("To undo this action") &&
                           output.Contains("ef migrations remove"));
        }

        private void InstallDotnetEfTool()
        {
            Logger.LogInformation("Installing dotnet-ef tool...");
            CmdHelper.RunCmd("dotnet tool install --global dotnet-ef");
            Logger.LogInformation("dotnet-ef tool is installed.");
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

        public string GetShortDescription()
        {
            return string.Empty;
        }
    }
}
