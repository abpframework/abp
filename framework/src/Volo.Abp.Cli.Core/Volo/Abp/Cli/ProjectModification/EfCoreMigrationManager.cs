using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification
{
    public class EfCoreMigrationManager : ITransientDependency
    {
        public ILogger<EfCoreMigrationManager> Logger { get; set; }

        public EfCoreMigrationManager()
        {
            Logger = NullLogger<EfCoreMigrationManager>.Instance;
        }

        public void AddMigration(string dbMigrationsCsprojFile, string module)
        {
            var dbMigrationsProjectFolder = Path.GetDirectoryName(dbMigrationsCsprojFile);
            var moduleName = ParseModuleName(module);
            var migrationName = "Added_" + moduleName + "_Module" + GetUniquePostFix();

            var tenantDbContextName = FindTenantDbContextName(dbMigrationsProjectFolder);
            var dbContextName = tenantDbContextName != null ?
                FindDbContextName(dbMigrationsProjectFolder)
                : null;

            if (!string.IsNullOrEmpty(tenantDbContextName))
            {
                RunAddMigrationCommand(dbMigrationsProjectFolder, migrationName, tenantDbContextName, "TenantMigrations");
            }

            RunAddMigrationCommand(dbMigrationsProjectFolder, migrationName, dbContextName, "Migrations");
        }

        protected virtual void RunAddMigrationCommand(
            string dbMigrationsProjectFolder,
            string migrationName,
            string dbContext,
            string outputDirectory)
        {
            var dbContextOption = string.IsNullOrWhiteSpace(dbContext)
                ? string.Empty
                : $"--context {dbContext}";

            CmdHelper.RunCmd($"cd \"{dbMigrationsProjectFolder}\" && dotnet ef migrations add {migrationName}" +
                             $" --output-dir {outputDirectory}" +
                             $" {dbContextOption}");
        }

        protected virtual string ParseModuleName(string fullModuleName)
        {
            var words = fullModuleName?.Split('.');
            if (words == null || words.Length <= 1)
            {
                return "";
            }

            return words[words.Length - 1];
        }

        protected virtual string GetUniquePostFix()
        {
            return "_" + new Random().Next(1, 99999);
        }

        protected virtual string FindDbContextName(string dbMigrationsFolder)
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

        protected virtual string FindTenantDbContextName(string dbMigrationsFolder)
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
    }
}
