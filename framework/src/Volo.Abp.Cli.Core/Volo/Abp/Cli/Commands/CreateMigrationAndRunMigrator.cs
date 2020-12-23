using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands
{
    public class CreateMigrationAndRunMigrator : IConsoleCommand, ITransientDependency
    {
        public virtual async Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            if (commandLineArgs.Target.IsNullOrEmpty())
            {
                throw new CliUsageException(
                    "DbMigrations folder path is missing!"
                );
            }

            var dbMigratorProjectPath = GetDbMigratorProjectPath(commandLineArgs.Target);

            if (dbMigratorProjectPath == null)
            {
                throw new Exception("DbMigrator is not found!");
            }

            CmdHelper.RunCmd("cd \"" + commandLineArgs.Target + "\" && dotnet ef migrations add Initial -s " + dbMigratorProjectPath);
            CmdHelper.RunCmd("cd \"" + Path.GetDirectoryName(dbMigratorProjectPath) + "\" && dotnet run");
        }

        private string GetDbMigratorProjectPath(string dbMigrationsFolderPath)
        {
            var srcFolder = Directory.GetParent(dbMigrationsFolderPath);

            var dbMigratorFolderPath = Directory.GetDirectories(srcFolder.FullName).FirstOrDefault(d => d.EndsWith(".DbMigrator"));

            if (dbMigratorFolderPath == null)
            {
                return null;
            }

            return Directory.GetFiles(dbMigratorFolderPath).FirstOrDefault(f => f.EndsWith(".csproj"));
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
