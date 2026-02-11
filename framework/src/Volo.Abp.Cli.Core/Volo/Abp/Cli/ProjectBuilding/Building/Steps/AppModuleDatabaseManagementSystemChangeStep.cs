using System;
using System.Linq;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

public class AppModuleDatabaseManagementSystemChangeStep : ProjectBuildPipelineStep
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
                ChangeEntityFrameworkCoreDependency(context, "Volo.Abp.EntityFrameworkCore.PostgreSql",
                    "Volo.Abp.EntityFrameworkCore.PostgreSql",
                    "AbpEntityFrameworkCorePostgreSqlModule");
                ChangeUseSqlServer(context, "UseNpgsql");
                break;

            case DatabaseManagementSystem.Oracle:
                ChangeEntityFrameworkCoreDependency(context, "Volo.Abp.EntityFrameworkCore.Oracle",
                    "Volo.Abp.EntityFrameworkCore.Oracle",
                    "AbpEntityFrameworkCoreOracleModule");
                AdjustOracleDbContextOptionsBuilder(context);
                ChangeUseSqlServer(context, "UseOracle");
                break;

            case DatabaseManagementSystem.OracleDevart:
                ChangeEntityFrameworkCoreDependency(context, "Volo.Abp.EntityFrameworkCore.Oracle.Devart",
                    "Volo.Abp.EntityFrameworkCore.Oracle.Devart",
                    "AbpEntityFrameworkCoreOracleDevartModule");
                AdjustOracleDbContextOptionsBuilder(context);
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

    private void AdjustOracleDbContextOptionsBuilder(ProjectBuildContext context)
    {
        var dbContextFactoryFiles = context.Files.Where(f => f.Name.EndsWith("DbContextFactory.cs", StringComparison.OrdinalIgnoreCase));
        foreach (var dbContextFactoryFile in dbContextFactoryFiles)
        {
            dbContextFactoryFile?.ReplaceText("new DbContextOptionsBuilder",
                $"(DbContextOptionsBuilder<{context.BuildArgs.SolutionName.ProjectName}{(false ? "Migrations" : string.Empty)}DbContext>) new DbContextOptionsBuilder");
        }
    }

    private void AddMySqlServerVersion(ProjectBuildContext context)
    {
        var dbContextFactoryFiles = context.Files.Where(f => f.Name.EndsWith("DbContextFactory.cs", StringComparison.OrdinalIgnoreCase));
        foreach (var dbContextFactoryFile in dbContextFactoryFiles)
        {
            dbContextFactoryFile?.ReplaceText("configuration.GetConnectionString(\"Default\")", "configuration.GetConnectionString(\"Default\"), MySqlServerVersion.LatestSupportedServerVersion");
        }
    }

    private void ChangeEntityFrameworkCoreDependency(ProjectBuildContext context, string newPackageName, string newModuleNamespace, string newModuleClass)
    {
        var efCoreProjectFiles = context.Files.Where(f => f.Name.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase));
        foreach (var efCoreProjectFile in efCoreProjectFiles)
        {
            efCoreProjectFile?.ReplaceText("Volo.Abp.EntityFrameworkCore.SqlServer", newPackageName);
        }

        var efCoreModuleClasses = context.Files.Where(f => f.Name.EndsWith("Module.cs", StringComparison.OrdinalIgnoreCase));
        foreach (var efCoreModuleClass in efCoreModuleClasses)
        {
            efCoreModuleClass?.ReplaceText("Volo.Abp.EntityFrameworkCore.SqlServer", newModuleNamespace);
            efCoreModuleClass?.ReplaceText("AbpEntityFrameworkCoreSqlServerModule", newModuleClass);
        }
    }

    private void ChangeUseSqlServer(ProjectBuildContext context, string newUseMethodForEfModule, string newUseMethodForDbContext = null)
    {
        if (newUseMethodForDbContext == null)
        {
            newUseMethodForDbContext = newUseMethodForEfModule;
        }

        const string oldUseMethod = "UseSqlServer";

        var efCoreModuleClasses = context.Files.Where(f => f.Name.EndsWith("Module.cs", StringComparison.OrdinalIgnoreCase));
        foreach (var efCoreModuleClass in efCoreModuleClasses)
        {
            efCoreModuleClass.ReplaceText(oldUseMethod, newUseMethodForEfModule);
        }

        var dbContextFactoryFiles = context.Files.Where(f => f.Name.EndsWith("DbContextFactory.cs", StringComparison.OrdinalIgnoreCase));
        foreach (var dbContextFactoryFile in dbContextFactoryFiles)
        {
            dbContextFactoryFile?.ReplaceText(oldUseMethod, newUseMethodForDbContext);
        }
    }
}
