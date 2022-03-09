using System;
using System.Linq;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

public class AppNoLayersDatabaseManagementSystemChangeStep : ProjectBuildPipelineStep
{
    public override void Execute(ProjectBuildContext context)
    {
        switch (context.BuildArgs.DatabaseManagementSystem)
        {
            case DatabaseManagementSystem.MySQL:
                ChangeEntityFrameworkCoreDependency(context, "Volo.Abp.EntityFrameworkCore.MySQL",
                    "Volo.Abp.EntityFrameworkCore.MySQL",
                    "AbpEntityFrameworkCoreMySQLModule");
                AddMySqlServerVersion(context);
                ChangeUseSqlServer(context, "UseMySQL", "UseMySql");
                break;

            case DatabaseManagementSystem.PostgreSQL:
                context.Symbols.Add("dbms:PostgreSQL");
                ChangeEntityFrameworkCoreDependency(context, "Volo.Abp.EntityFrameworkCore.PostgreSql",
                    "Volo.Abp.EntityFrameworkCore.PostgreSql",
                    "AbpEntityFrameworkCorePostgreSqlModule");
                ChangeUseSqlServer(context, "UseNpgsql");
                break;

            case DatabaseManagementSystem.Oracle:
                ChangeEntityFrameworkCoreDependency(context, "Volo.Abp.EntityFrameworkCore.Oracle",
                    "Volo.Abp.EntityFrameworkCore.Oracle",
                    "AbpEntityFrameworkCoreOracleModule");
                ChangeUseSqlServer(context, "UseOracle");
                break;

            case DatabaseManagementSystem.OracleDevart:
                ChangeEntityFrameworkCoreDependency(context, "Volo.Abp.EntityFrameworkCore.Oracle.Devart",
                    "Volo.Abp.EntityFrameworkCore.Oracle.Devart",
                    "AbpEntityFrameworkCoreOracleDevartModule");
                ChangeUseSqlServer(context, "UseOracle");
                break;

            case DatabaseManagementSystem.SQLite:
                ChangeEntityFrameworkCoreDependency(context, "Volo.Abp.EntityFrameworkCore.Sqlite",
                    "Volo.Abp.EntityFrameworkCore.Sqlite",
                    "AbpEntityFrameworkCoreSqliteModule");
                ChangeUseSqlServer(context, "UseSqlite");
                break;

            default:
                return;
        }
    }

    private void AddMySqlServerVersion(ProjectBuildContext context)
    {
        var dbContextFactoryFile = context.Files.FirstOrDefault(f => f.Name.EndsWith("DbContextFactory.cs", StringComparison.OrdinalIgnoreCase));

        dbContextFactoryFile?.ReplaceText("configuration.GetConnectionString(\"Default\")",
            "configuration.GetConnectionString(\"Default\"), MySqlServerVersion.LatestSupportedServerVersion");
    }

    private void ChangeEntityFrameworkCoreDependency(ProjectBuildContext context, string newPackageName, string newModuleNamespace, string newModuleClass)
    {
        var projectFile = context.Files.FirstOrDefault(f => f.Name.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase));
        projectFile?.ReplaceText("Volo.Abp.EntityFrameworkCore.SqlServer", newPackageName);

        var moduleClass = context.Files.FirstOrDefault(f => f.Name.EndsWith("Module.cs", StringComparison.OrdinalIgnoreCase));
        moduleClass?.ReplaceText("Volo.Abp.EntityFrameworkCore.SqlServer", newModuleNamespace);
        moduleClass?.ReplaceText("AbpEntityFrameworkCoreSqlServerModule", newModuleClass);
    }

    private void ChangeUseSqlServer(ProjectBuildContext context, string newUseMethodForEfModule, string newUseMethodForDbContext = null)
    {
        if (newUseMethodForDbContext == null)
        {
            newUseMethodForDbContext = newUseMethodForEfModule;
        }

        var oldUseMethod = "UseSqlServer";

        var moduleClass = context.Files.FirstOrDefault(f => f.Name.EndsWith("Module.cs", StringComparison.OrdinalIgnoreCase));
        moduleClass?.ReplaceText(oldUseMethod, newUseMethodForEfModule);

        var dbContextFactoryFile = context.Files.FirstOrDefault(f => f.Name.EndsWith("DbContextFactory.cs", StringComparison.OrdinalIgnoreCase));
        dbContextFactoryFile?.ReplaceText(oldUseMethod, newUseMethodForDbContext);
    }
}
