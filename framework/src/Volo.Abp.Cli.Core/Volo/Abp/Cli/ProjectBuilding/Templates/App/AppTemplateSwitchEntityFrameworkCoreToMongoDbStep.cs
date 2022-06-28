﻿using System;
using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.App;

public class AppTemplateSwitchEntityFrameworkCoreToMongoDbStep : ProjectBuildPipelineStep
{
    private readonly bool _hasDbMigrations;

    public AppTemplateSwitchEntityFrameworkCoreToMongoDbStep(bool hasDbMigrations)
    {
        _hasDbMigrations = hasDbMigrations;
    }

    public override void Execute(ProjectBuildContext context)
    {
        //MyCompanyName.MyProjectName.Web

        ChangeProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Web/MyCompanyName.MyProjectName.Web.csproj",
            _hasDbMigrations ? "EntityFrameworkCore.DbMigrations" : "EntityFrameworkCore",
            "MongoDB"
        );

        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Web/MyProjectNameWebModule.cs",
            "MyCompanyName.MyProjectName.EntityFrameworkCore",
            "MyCompanyName.MyProjectName.MongoDB",
            _hasDbMigrations ? "MyProjectNameEntityFrameworkCoreDbMigrationsModule" : "MyProjectNameEntityFrameworkCoreModule",
            "MyProjectNameMongoDbModule"
        );

        ChangeConnectionStringToMongoDb(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Web/appsettings.json"
        );

        //MyCompanyName.MyProjectName.AuthServer

        ChangeProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.AuthServer/MyCompanyName.MyProjectName.AuthServer.csproj",
            _hasDbMigrations ? "EntityFrameworkCore.DbMigrations" : "EntityFrameworkCore",
            "MongoDB"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.AuthServer/MyProjectNameAuthServerModule.cs",
            "MyCompanyName.MyProjectName.EntityFrameworkCore",
            "MyCompanyName.MyProjectName.MongoDB",
            _hasDbMigrations ? "MyProjectNameEntityFrameworkCoreDbMigrationsModule" : "MyProjectNameEntityFrameworkCoreModule",
            "MyProjectNameMongoDbModule"
        );

        ChangeConnectionStringToMongoDb(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.AuthServer/appsettings.json"
        );

        //MyCompanyName.MyProjectName.HttpApi.Host

        ChangeProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.HttpApi.Host/MyCompanyName.MyProjectName.HttpApi.Host.csproj",
            _hasDbMigrations ? "EntityFrameworkCore.DbMigrations" : "EntityFrameworkCore",
            "MongoDB"
        );

        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.HttpApi.Host/MyProjectNameHttpApiHostModule.cs",
            "MyCompanyName.MyProjectName.EntityFrameworkCore",
            "MyCompanyName.MyProjectName.MongoDB",
            _hasDbMigrations ? "MyProjectNameEntityFrameworkCoreDbMigrationsModule" : "MyProjectNameEntityFrameworkCoreModule",
            "MyProjectNameMongoDbModule"
        );

        ChangeConnectionStringToMongoDb(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.HttpApi.Host/appsettings.json"
        );

        //MyCompanyName.MyProjectName.Blazor.Server

        ChangeProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server/MyCompanyName.MyProjectName.Blazor.Server.csproj",
            _hasDbMigrations ? "EntityFrameworkCore.DbMigrations" : "EntityFrameworkCore",
            "MongoDB"
        );

        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server/MyProjectNameBlazorModule.cs",
            "MyCompanyName.MyProjectName.EntityFrameworkCore",
            "MyCompanyName.MyProjectName.MongoDB",
            _hasDbMigrations ? "MyProjectNameEntityFrameworkCoreDbMigrationsModule" : "MyProjectNameEntityFrameworkCoreModule",
            "MyProjectNameMongoDbModule"
        );

        ChangeConnectionStringToMongoDb(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server/appsettings.json"
        );

        //MyCompanyName.MyProjectName.HttpApi.HostWithIds

        ChangeProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.HttpApi.HostWithIds/MyCompanyName.MyProjectName.HttpApi.HostWithIds.csproj",
            _hasDbMigrations ? "EntityFrameworkCore.DbMigrations" : "EntityFrameworkCore",
            "MongoDB"
        );

        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.HttpApi.HostWithIds/MyProjectNameHttpApiHostModule.cs",
            "MyCompanyName.MyProjectName.EntityFrameworkCore",
            "MyCompanyName.MyProjectName.MongoDB",
            _hasDbMigrations ? "MyProjectNameEntityFrameworkCoreDbMigrationsModule" : "MyProjectNameEntityFrameworkCoreModule",
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
            _hasDbMigrations ? "EntityFrameworkCore.DbMigrations" : "EntityFrameworkCore",
            "MongoDB"
        );

        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.DbMigrator/MyProjectNameDbMigratorModule.cs",
            "MyCompanyName.MyProjectName.EntityFrameworkCore",
            "MyCompanyName.MyProjectName.MongoDB",
            _hasDbMigrations ? "MyProjectNameEntityFrameworkCoreDbMigrationsModule" : "MyProjectNameEntityFrameworkCoreModule",
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

        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/test/MyCompanyName.MyProjectName.Domain.Tests/MyProjectNameDomainTestModule.cs",
            "MyCompanyName.MyProjectName.EntityFrameworkCore",
            "MyCompanyName.MyProjectName.MongoDB",
            "MyProjectNameEntityFrameworkCoreTestModule",
            "MyProjectNameMongoDbTestModule"
        );

        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/test/MyCompanyName.MyProjectName.Domain.Tests/MyProjectNameDomainCollection.cs",
            "MyCompanyName.MyProjectName.EntityFrameworkCore",
            "MyCompanyName.MyProjectName.MongoDB",
            "MyProjectNameEntityFrameworkCoreCollectionFixtureBase",
            "MyProjectNameMongoDbCollectionFixtureBase"
        );

        //MyCompanyName.MyProjectName.Application.Tests

        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/test/MyCompanyName.MyProjectName.Application.Tests/MyProjectNameApplicationCollection.cs",
            "MyCompanyName.MyProjectName.EntityFrameworkCore",
            "MyCompanyName.MyProjectName.MongoDB",
            "MyProjectNameEntityFrameworkCoreCollectionFixtureBase",
            "MyProjectNameMongoDbCollectionFixtureBase"
        );

        //MyCompanyName.MyProjectName.Web.Tests

        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/test/MyCompanyName.MyProjectName.Web.Tests/MyProjectNameWebCollection.cs",
            "MyCompanyName.MyProjectName.EntityFrameworkCore",
            "MyCompanyName.MyProjectName.MongoDB",
            "MyProjectNameEntityFrameworkCoreCollectionFixtureBase",
            "MyProjectNameMongoDbCollectionFixtureBase"
        );

        // TODO: remove this method after published 6.0.0
        ProvideIdentityServerBackwardCompatibility(context);

        if (context.BuildArgs.PublicWebSite)
        {
            ChangeProjectReference(
                context,
                "/aspnet-core/src/MyCompanyName.MyProjectName.Web.Public/MyCompanyName.MyProjectName.Web.Public.csproj",
                _hasDbMigrations ? "EntityFrameworkCore.DbMigrations" : "EntityFrameworkCore",
                "MongoDB"
            );

            ChangeNamespaceAndKeyword(
                context,
                "/aspnet-core/src/MyCompanyName.MyProjectName.Web.Public/MyProjectNameWebPublicModule.cs",
                "MyCompanyName.MyProjectName.EntityFrameworkCore",
                "MyCompanyName.MyProjectName.MongoDB",
                _hasDbMigrations ? "MyProjectNameEntityFrameworkCoreDbMigrationsModule" : "MyProjectNameEntityFrameworkCoreModule",
                "MyProjectNameMongoDbModule"
            );

            ChangeConnectionStringToMongoDb(
                context,
                "/aspnet-core/src/MyCompanyName.MyProjectName.Web.Public/appsettings.json"
            );
        }
    }

    private void ChangeProjectReference(
        ProjectBuildContext context,
        string targetProjectFilePath,
        string oldReference,
        string newReference)
    {
        var file = context.FindFile(targetProjectFilePath);

        if (file == null)
        {
            return;
        }

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

    private void ChangeNamespaceAndKeyword(
        ProjectBuildContext context,
        string targetModuleFilePath,
        string oldNamespace,
        string newNamespace,
        string oldKeyword,
        string newKeyword)
    {
        var file = context.FindFile(targetModuleFilePath);

        if (file == null)
        {
            return;
        }

        file.NormalizeLineEndings();

        var lines = file.GetLines();

        for (var i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains($"using {oldNamespace};"))
            {
                lines[i] = $"using {newNamespace};";
            }
            else if (lines[i].Contains(oldKeyword))
            {
                lines[i] = lines[i].Replace(oldKeyword, newKeyword);
            }
        }

        file.SetLines(lines);
    }

    private void ChangeConnectionStringToMongoDb(
        ProjectBuildContext context,
        string appsettingFilePath)
    {
        var file = context.FindFile(appsettingFilePath);

        if (file == null)
        {
            return;
        }

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
    
    // TODO: remove this method after published 6.0.0
    private void ProvideIdentityServerBackwardCompatibility(ProjectBuildContext context)
    {
        ChangeProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.IdentityServer/MyCompanyName.MyProjectName.IdentityServer.csproj",
            _hasDbMigrations ? "EntityFrameworkCore.DbMigrations" : "EntityFrameworkCore",
            "MongoDB"
        );

        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.IdentityServer/MyProjectNameIdentityServerModule.cs",
            "MyCompanyName.MyProjectName.EntityFrameworkCore",
            "MyCompanyName.MyProjectName.MongoDB",
            _hasDbMigrations
                ? "MyProjectNameEntityFrameworkCoreDbMigrationsModule"
                : "MyProjectNameEntityFrameworkCoreModule",
            "MyProjectNameMongoDbModule"
        );

        ChangeConnectionStringToMongoDb(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.IdentityServer/appsettings.json"
        );
    }
}
