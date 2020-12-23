using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification
{
    public class EfCoreMigrationRecreater  : ITransientDependency
    {
        public ILogger<EfCoreMigrationRecreater> Logger { get; set; }

        public EfCoreMigrationRecreater()
        {
            Logger = NullLogger<EfCoreMigrationRecreater>.Instance;
        }

        public void Recreate(string solutionFolder)
        {
            if (Directory.Exists(Path.Combine(solutionFolder, "aspnet-core")))
            {
                solutionFolder = Path.Combine(solutionFolder, "aspnet-core");
            }

            var srcFolder = Path.Combine(solutionFolder, "src");

            try
            {
                var migrationsFolder = Directory.GetDirectories(srcFolder).First(d => d.EndsWith(".EntityFrameworkCore.DbMigrations"));
                Directory.Delete(Path.Combine(migrationsFolder, "Migrations"), true);

                var migratorFolder = Directory.GetDirectories(srcFolder).First(d => d.EndsWith(".DbMigrator"));
                var migratorProjectFile = Directory.GetFiles(migratorFolder).First(d => d.EndsWith(".DbMigrator.csproj"));
                var addMigrationCommand = $"dotnet ef migrations add Initial --startup-project {migratorProjectFile}";
                CmdHelper.RunCmd($"cd {migrationsFolder} && {addMigrationCommand}");
            }
            catch (Exception e)
            {
                Logger.LogWarning("Re-creating migrations process failed.");
                throw e;
            }
        }
    }
}
