using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification;

public class EfCoreMigrationManager : ITransientDependency
{
    public ICmdHelper CmdHelper { get; }
    public ILogger<EfCoreMigrationManager> Logger { get; set; }

    public EfCoreMigrationManager(ICmdHelper cmdHelper)
    {
        CmdHelper = cmdHelper;
        Logger = NullLogger<EfCoreMigrationManager>.Instance;
    }

    public void AddMigration(string dbMigrationsCsprojFile, string module)
    {
        var dbMigrationsProjectFolder = Path.GetDirectoryName(dbMigrationsCsprojFile);
        var moduleName = ParseModuleName(module);
        var migrationName = "Added_" + moduleName + "_Module" + GetUniquePostFix();

        var tenantDbContextName = FindTenantDbContextName(dbMigrationsProjectFolder);
        var dbContextNames = FindDbContextNames(dbMigrationsProjectFolder);

        if (!string.IsNullOrEmpty(tenantDbContextName))
        {
            RunAddMigrationCommand(dbMigrationsProjectFolder, migrationName, tenantDbContextName, "TenantMigrations");
        }

        if (dbContextNames.Count == 1)
        {
            RunAddMigrationCommand(dbMigrationsProjectFolder, migrationName, dbContextNames[0], "Migrations");
        }
        else if (dbContextNames.Count > 1)
        {
            // Each DbContext needs its own migration with --context, otherwise
            // `dotnet ef migrations add` fails with "More than one DbContext was found".
            // The migration name is suffixed with the context name so migrations for
            // different contexts do not collide in the same output directory.
            foreach (var dbContextName in dbContextNames)
            {
                RunAddMigrationCommand(dbMigrationsProjectFolder, migrationName + "_" + dbContextName, dbContextName, "Migrations");
            }
        }
        else
        {
            // No DbContext file could be detected by name. Keep the previous behavior
            // (no --context) so projects with an unconventionally named single
            // DbContext still work, but warn: the command fails if the project
            // actually contains more than one DbContext.
            Logger.LogWarning("Could not detect a DbContext in {ProjectFolder}. Running 'dotnet ef migrations add' without --context; it will fail if the project contains more than one DbContext.", dbMigrationsProjectFolder);
            RunAddMigrationCommand(dbMigrationsProjectFolder, migrationName, null, "Migrations");
        }
    }

    protected virtual void RunAddMigrationCommand(
        string dbMigrationsProjectFolder,
        string migrationName,
        string dbContext,
        string outputDirectory)
    {
        CmdHelper.RunCmd($"dotnet build", workingDirectory: dbMigrationsProjectFolder, exitCode: out var buildExitCode);
        if (buildExitCode != 0)
        {
            Logger.LogWarning("Dotnet build failed for project folder {ProjectFolder}. Skipping EF Core migration command.", dbMigrationsProjectFolder);
            return;
        }

        var dbContextOption = string.IsNullOrWhiteSpace(dbContext)
            ? string.Empty
            : $"--context {dbContext}";

        CmdHelper.RunCmd($"dotnet ef migrations add {migrationName}" +
                         $" --output-dir {outputDirectory}" +
                         $" {dbContextOption}",
            workingDirectory: dbMigrationsProjectFolder);
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
        return FindDbContextNames(dbMigrationsFolder).FirstOrDefault();
    }

    /// <summary>
    /// Finds all non-tenant DbContext names in the given folder, ordered with
    /// the migrations DbContext (e.g. *MigrationsDbContext) first.
    /// </summary>
    protected virtual List<string> FindDbContextNames(string dbMigrationsFolder)
    {
        var dbContextFiles = Directory.GetFiles(dbMigrationsFolder, "*DbContext.cs", SearchOption.AllDirectories)
            .Where(fp => !fp.EndsWith("TenantDbContext.cs") &&
                         !fp.EndsWith("TenantMigrationsDbContext.cs"))
            .ToList();

        return dbContextFiles
            .OrderByDescending(fp => fp.EndsWith("MigrationsDbContext.cs"))
            .ThenBy(fp => fp, StringComparer.OrdinalIgnoreCase)
            .Select(fp => Path.GetFileName(fp).RemovePostFix(".cs"))
            .ToList();
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
