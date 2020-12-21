using System;
using System.Linq;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps
{
    public class DatabaseManagementSystemChangeStep : ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            switch (context.BuildArgs.DatabaseManagementSystem)
            {
                    case DatabaseManagementSystem.MySQL:
                        ChangeEntityFrameworkCoreDependency(context,"Volo.Abp.EntityFrameworkCore.MySQL",
                            "Volo.Abp.EntityFrameworkCore.MySQL",
                            "AbpEntityFrameworkCoreMySQLModule");
                        ChangeUseSqlServer(context,"UseMySql");
                    break;

                    case DatabaseManagementSystem.PostgreSQL:
                        ChangeEntityFrameworkCoreDependency(context,"Volo.Abp.EntityFrameworkCore.PostgreSql",
                            "Volo.Abp.EntityFrameworkCore.PostgreSql",
                            "AbpEntityFrameworkCorePostgreSqlModule");
                        ChangeUseSqlServer(context,"UseNpgsql");
                        break;

                    case DatabaseManagementSystem.Oracle:
                        ChangeEntityFrameworkCoreDependency(context,"Volo.Abp.EntityFrameworkCore.Oracle",
                            "Volo.Abp.EntityFrameworkCore.Oracle",
                            "AbpEntityFrameworkCoreOracleModule");
                        ChangeUseSqlServer(context,"UseOracle");
                        break;

                    case DatabaseManagementSystem.OracleDevart:
                        ChangeEntityFrameworkCoreDependency(context,"Volo.Abp.EntityFrameworkCore.Oracle.Devart",
                            "Volo.Abp.EntityFrameworkCore.Oracle.Devart",
                            "AbpEntityFrameworkCoreOracleDevartModule");
                        ChangeUseSqlServer(context,"UseOracle");
                        break;

                    case DatabaseManagementSystem.SQLite:
                        ChangeEntityFrameworkCoreDependency(context,"Volo.Abp.EntityFrameworkCore.Sqlite",
                            "Volo.Abp.EntityFrameworkCore.Sqlite",
                            "AbpEntityFrameworkCoreSqliteModule");
                        ChangeUseSqlServer(context,"UseSqlite");
                        break;

                    default:
                        return;
            }
        }

        private void ChangeEntityFrameworkCoreDependency(ProjectBuildContext context, string newPackageName, string newModuleNamespace, string newModuleClass)
        {
            var efCoreProjectFile = context.Files.First(f => f.Name.EndsWith("EntityFrameworkCore.csproj", StringComparison.OrdinalIgnoreCase));
            efCoreProjectFile.ReplaceText("Volo.Abp.EntityFrameworkCore.SqlServer", newPackageName);

            var efCoreModuleClass = context.Files.First(f => f.Name.EndsWith("EntityFrameworkCoreModule.cs", StringComparison.OrdinalIgnoreCase));
            efCoreModuleClass.ReplaceText("Volo.Abp.EntityFrameworkCore.SqlServer", newModuleNamespace);
            efCoreModuleClass.ReplaceText("AbpEntityFrameworkCoreSqlServerModule", newModuleClass);
        }

        private void ChangeUseSqlServer(ProjectBuildContext context, string newUseMethod)
        {
            var oldUseMethod = "UseSqlServer";

            var efCoreModuleClass = context.Files.First(f => f.Name.EndsWith("EntityFrameworkCoreModule.cs", StringComparison.OrdinalIgnoreCase));
            efCoreModuleClass.ReplaceText(oldUseMethod, newUseMethod);

            var dbContextFactoryFile = context.Files.First(f => f.Name.EndsWith("MigrationsDbContextFactory.cs", StringComparison.OrdinalIgnoreCase));
            dbContextFactoryFile.ReplaceText(oldUseMethod, newUseMethod);
        }
    }
}
