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

            var dbMigratorProjectPath = GetDbMigratorProjectPath(commandLineArgs.Target);
            if (dbMigratorProjectPath == null)
            {
                throw new Exception("DbMigrator is not found!");
            }

            if (!IsDotNetEfToolInstalled())
            {
                InstallDotnetEfTool();
            }

            var addMigrationCmd = $"cd \"{commandLineArgs.Target}\" && " +
                                  $"dotnet ef migrations add Initial -s \"{dbMigratorProjectPath}\"";

            var output = CmdHelper.RunCmdAndGetOutput(addMigrationCmd);
            if (output.Contains("Done.") &&
                output.Contains("To undo this action") &&
                output.Contains("ef migrations remove"))
            {
                // Migration added successfully
                CmdHelper.RunCmd("cd \"" + Path.GetDirectoryName(dbMigratorProjectPath) + "\" && dotnet run");
                await Task.CompletedTask;
            }
            else
            {
                var exceptionMsg = "Migrations failed! The following command didn't run successfully:" +
                                   Environment.NewLine +
                                   addMigrationCmd +
                                   Environment.NewLine + output;

                Logger.LogError(exceptionMsg);
                throw new Exception(exceptionMsg);
            }
        }

        private static bool IsDotNetEfToolInstalled()
        {
            var output = CmdHelper.RunCmdAndGetOutput("dotnet tool list -g");
            return output.Contains("dotnet-ef");
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