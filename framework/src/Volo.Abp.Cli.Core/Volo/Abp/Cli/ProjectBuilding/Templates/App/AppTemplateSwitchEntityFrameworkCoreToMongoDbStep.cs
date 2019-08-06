using System;
using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.App
{
    public class AppTemplateSwitchEntityFrameworkCoreToMongoDbStep : ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            //MyCompanyName.MyProjectName.Web

            ChangeProjectReference(
                context,
                "/aspnet-core/src/MyCompanyName.MyProjectName.Web/MyCompanyName.MyProjectName.Web.csproj",
                "EntityFrameworkCore.DbMigrations",
                "MongoDB"
            );

            ChangeModuleDependency(
                context,
                "/aspnet-core/src/MyCompanyName.MyProjectName.Web/MyProjectNameWebModule.cs",
                "MyCompanyName.MyProjectName.EntityFrameworkCore",
                "MyCompanyName.MyProjectName.MongoDB",
                "MyProjectNameEntityFrameworkCoreDbMigrationsModule",
                "MyProjectNameMongoDbModule"
            );

            ChangeConnectionStringToMongoDb(
                context,
                "/aspnet-core/src/MyCompanyName.MyProjectName.Web/appsettings.json"
            );

            //MyCompanyName.MyProjectName.IdentityServer

            ChangeProjectReference(
                context,
                "/aspnet-core/src/MyCompanyName.MyProjectName.IdentityServer/MyCompanyName.MyProjectName.IdentityServer.csproj",
                "EntityFrameworkCore.DbMigrations",
                "MongoDB"
            );

            ChangeModuleDependency(
                context,
                "/aspnet-core/src/MyCompanyName.MyProjectName.IdentityServer/MyProjectNameIdentityServerModule.cs",
                "MyCompanyName.MyProjectName.EntityFrameworkCore",
                "MyCompanyName.MyProjectName.MongoDB",
                "MyProjectNameEntityFrameworkCoreDbMigrationsModule",
                "MyProjectNameMongoDbModule"
            );

            ChangeConnectionStringToMongoDb(
                context,
                "/aspnet-core/src/MyCompanyName.MyProjectName.IdentityServer/appsettings.json"
            );

            //MyCompanyName.MyProjectName.HttpApi.Host

            ChangeProjectReference(
                context,
                "/aspnet-core/src/MyCompanyName.MyProjectName.HttpApi.Host/MyCompanyName.MyProjectName.HttpApi.Host.csproj",
                "EntityFrameworkCore.DbMigrations",
                "MongoDB"
            );

            ChangeModuleDependency(
                context,
                "/aspnet-core/src/MyCompanyName.MyProjectName.HttpApi.Host/MyProjectNameHttpApiHostModule.cs",
                "MyCompanyName.MyProjectName.EntityFrameworkCore",
                "MyCompanyName.MyProjectName.MongoDB",
                "MyProjectNameEntityFrameworkCoreDbMigrationsModule",
                "MyProjectNameMongoDbModule"
            );

            ChangeConnectionStringToMongoDb(
                context,
                "/aspnet-core/src/MyCompanyName.MyProjectName.HttpApi.Host/appsettings.json"
            );

            //MyCompanyName.MyProjectName.HttpApi.HostWithIds

            ChangeProjectReference(
                context,
                "/aspnet-core/src/MyCompanyName.MyProjectName.HttpApi.HostWithIds/MyCompanyName.MyProjectName.HttpApi.HostWithIds.csproj",
                "EntityFrameworkCore.DbMigrations",
                "MongoDB"
            );

            ChangeModuleDependency(
                context,
                "/aspnet-core/src/MyCompanyName.MyProjectName.HttpApi.HostWithIds/MyProjectNameHttpApiHostModule.cs",
                "MyCompanyName.MyProjectName.EntityFrameworkCore",
                "MyCompanyName.MyProjectName.MongoDB",
                "MyProjectNameEntityFrameworkCoreDbMigrationsModule",
                "MyProjectNameMongoDbModule"
            );

            ChangeConnectionStringToMongoDb(
                context,
                "/aspnet-core/src/MyCompanyName.MyProjectName.HttpApi.HostWithIds/appsettings.json"
            );

            //MyCompanyName.MyProjectName.DbMigrator

            ChangeProjectReference(
                context,
                "/aspnet-core/src/MyCompanyName.MyProjectName.DbMigrator/MyCompanyName.MyProjectName.DbMigrator.csproj",
                "EntityFrameworkCore.DbMigrations",
                "MongoDB"
            );

            ChangeModuleDependency(
                context,
                "/aspnet-core/src/MyCompanyName.MyProjectName.DbMigrator/MyProjectNameDbMigratorModule.cs",
                "MyCompanyName.MyProjectName.EntityFrameworkCore",
                "MyCompanyName.MyProjectName.MongoDB",
                "MyProjectNameEntityFrameworkCoreDbMigrationsModule",
                "MyProjectNameMongoDbModule"
            );

            ChangeConnectionStringToMongoDb(
                context,
                "/aspnet-core/src/MyCompanyName.MyProjectName.DbMigrator/appsettings.json"
            );

            //MyCompanyName.MyProjectName.Domain.Tests

            ChangeProjectReference(
                context,
                "/aspnet-core/test/MyCompanyName.MyProjectName.Domain.Tests/MyCompanyName.MyProjectName.Domain.Tests.csproj",
                "EntityFrameworkCore.Tests",
                "MongoDB.Tests"
            );

            ChangeModuleDependency(
                context,
                "/aspnet-core/test/MyCompanyName.MyProjectName.Domain.Tests/MyProjectNameDomainTestModule.cs",
                "MyCompanyName.MyProjectName.EntityFrameworkCore",
                "MyCompanyName.MyProjectName.MongoDB",
                "MyProjectNameEntityFrameworkCoreTestModule",
                "MyProjectNameMongoDbTestModule"
            );
        }

        private void ChangeProjectReference(
            ProjectBuildContext context,
            string targetProjectFilePath,
            string oldReference,
            string newReference)
        {
            var file = context.GetFile(targetProjectFilePath);

            file.NormalizeLineEndings();

            var lines = file.GetLines();
            for (var i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("ProjectReference") && lines[i].Contains(oldReference))
                {
                    lines[i] = lines[i].Replace(oldReference, newReference);
                    file.SetLines(lines);
                    return;
                }
            }

            throw new ApplicationException($"Could not find the '{oldReference}' reference in the project '{targetProjectFilePath}'!");
        }

        private void ChangeModuleDependency(
            ProjectBuildContext context,
            string targetModuleFilePath,
            string oldNamespace,
            string newNamespace,
            string oldModuleName,
            string newModuleName)
        {
            var file = context.GetFile(targetModuleFilePath);

            file.NormalizeLineEndings();

            var lines = file.GetLines();

            for (var i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains($"using {oldNamespace};"))
                {
                    lines[i] = $"using {newNamespace};";
                }
                else if (lines[i].Contains(oldModuleName))
                {
                    lines[i] = lines[i].Replace(oldModuleName, newModuleName);
                }
            }

            file.SetLines(lines);
        }

        private void ChangeConnectionStringToMongoDb(
            ProjectBuildContext context,
            string appsettingFilePath)
        {
            var file = context.GetFile(appsettingFilePath);

            file.NormalizeLineEndings();

            var lines = file.GetLines();
            for (var i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("Default") && lines[i].Contains("Database"))
                {
                    lines[i] = "    \"Default\": \"mongodb://localhost:27017/MyProjectName\"";
                    file.SetLines(lines);
                    return;
                }
            }

            throw new ApplicationException("Could not find the 'Default' connection string in appsettings.json file!");
        }
    }
}