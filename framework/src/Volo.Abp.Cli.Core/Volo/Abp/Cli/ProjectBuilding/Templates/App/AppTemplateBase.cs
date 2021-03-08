﻿using System;
using System.Collections.Generic;
using NuGet.Versioning;
using Volo.Abp.Cli.Commands;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Building.Steps;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.App
{
    public abstract class AppTemplateBase : TemplateInfo
    {
        protected AppTemplateBase(string templateName)
            : base(templateName, DatabaseProvider.EntityFrameworkCore, UiFramework.Mvc)
        {
        }

        public static bool IsAppTemplate(string templateName)
        {
            return templateName == AppTemplate.TemplateName ||
                   templateName == AppProTemplate.TemplateName;
        }

        public override IEnumerable<ProjectBuildPipelineStep> GetCustomSteps(ProjectBuildContext context)
        {
            var steps = new List<ProjectBuildPipelineStep>();

            ConfigureTenantSchema(context, steps);
            SwitchDatabaseProvider(context, steps);
            DeleteUnrelatedProjects(context, steps);
            RemoveMigrations(context, steps);
            ConfigurePublicWebSite(context, steps);
            RemoveUnnecessaryPorts(context, steps);
            RandomizeSslPorts(context, steps);
            RandomizeStringEncryption(context, steps);
            UpdateNuGetConfig(context, steps);
            ChangeConnectionString(context, steps);
            CleanupFolderHierarchy(context, steps);

            return steps;
        }

        private static void ConfigureTenantSchema(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            if (context.BuildArgs.ExtraProperties.ContainsKey("separate-tenant-schema"))
            {
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.EntityFrameworkCore.DbMigrations"));
                steps.Add(new AppTemplateProjectRenameStep("MyCompanyName.MyProjectName.EntityFrameworkCore.SeparateDbMigrations", "MyCompanyName.MyProjectName.EntityFrameworkCore.DbMigrations"));
            }
            else
            {
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.EntityFrameworkCore.SeparateDbMigrations"));
            }
        }

        private static void SwitchDatabaseProvider(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            if (context.BuildArgs.DatabaseProvider == DatabaseProvider.MongoDb)
            {
                steps.Add(new AppTemplateSwitchEntityFrameworkCoreToMongoDbStep());
            }

            if (context.BuildArgs.DatabaseProvider != DatabaseProvider.EntityFrameworkCore)
            {
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.EntityFrameworkCore"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.EntityFrameworkCore.DbMigrations"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.EntityFrameworkCore.Tests", projectFolderPath: "/aspnet-core/test/MyCompanyName.MyProjectName.EntityFrameworkCore.Tests"));
            }

            if (context.BuildArgs.DatabaseProvider != DatabaseProvider.MongoDb)
            {
                steps.Add(new AppTemplateRemoveMongodbCollectionFixtureStep());
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.MongoDB"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.MongoDB.Tests", projectFolderPath: "/aspnet-core/test/MyCompanyName.MyProjectName.MongoDB.Tests"));
            }
        }

        private static void DeleteUnrelatedProjects(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            switch (context.BuildArgs.UiFramework)
            {
                case UiFramework.None:
                    ConfigureWithoutUi(context, steps);
                    break;

                case UiFramework.Angular:
                    ConfigureWithAngularUi(context, steps);
                    break;


                case UiFramework.Blazor:
                    ConfigureWithBlazorUi(context, steps);
                    break;

                case UiFramework.Mvc:
                case UiFramework.NotSpecified:
                    ConfigureWithMvcUi(context, steps);
                    break;
            }

            if (context.BuildArgs.UiFramework != UiFramework.Blazor)
            {
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor"));
            }

            if (context.BuildArgs.UiFramework != UiFramework.Angular)
            {
                steps.Add(new RemoveFolderStep("/angular"));
            }

            if (context.BuildArgs.MobileApp != MobileApp.ReactNative)
            {
                steps.Add(new RemoveFolderStep(MobileApp.ReactNative.GetFolderName().EnsureStartsWith('/')));
            }

            if (!context.BuildArgs.PublicWebSite)
            {
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Public"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Public.Host"));
            }
            else
            {
                if (context.BuildArgs.ExtraProperties.ContainsKey(NewCommand.Options.Tiered.Long) || context.BuildArgs.ExtraProperties.ContainsKey("separate-identity-server"))
                {
                    steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Public"));
                }
                else
                {
                    steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Public.Host"));
                }
            }
        }

        private void ConfigurePublicWebSite(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            if (!context.BuildArgs.PublicWebSite)
            {
                if (!context.BuildArgs.ExtraProperties.ContainsKey(NewCommand.Options.Tiered.Long) &&
                    !context.BuildArgs.ExtraProperties.ContainsKey("separate-identity-server"))
                {
                    steps.Add(new RemovePublicRedisStep());
                }

                steps.Add(new RemoveCmsKitStep());
                return;
            }

            if (context.BuildArgs.ExtraProperties.ContainsKey(NewCommand.Options.Tiered.Long) || context.BuildArgs.ExtraProperties.ContainsKey("separate-identity-server"))
            {
                steps.Add(new AppTemplateProjectRenameStep("MyCompanyName.MyProjectName.Web.Public.Host","MyCompanyName.MyProjectName.Web.Public"));
                steps.Add(new ChangeDbMigratorPublicPortStep());
            }
            else if (context.BuildArgs.UiFramework != UiFramework.NotSpecified && context.BuildArgs.UiFramework != UiFramework.Mvc)
            {
                steps.Add(new ChangePublicAuthPortStep());
            }

            if (context.BuildArgs.DatabaseProvider != DatabaseProvider.NotSpecified && context.BuildArgs.DatabaseProvider != DatabaseProvider.EntityFrameworkCore)
            {
                steps.Add(new RemoveEfCoreDependencyFromPublicStep());
            }

            // We disabled cms-kit for v4.2 release.
            if (true || context.BuildArgs.ExtraProperties.ContainsKey("without-cms-kit"))
            {
                steps.Add(new RemoveCmsKitStep());
            }
            else
            {
                steps.Add(new RemoveGlobalFeaturesPackageStep());
            }
        }

        private static void ConfigureWithoutUi(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Host"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Tests", projectFolderPath: "/aspnet-core/test/MyCompanyName.MyProjectName.Web.Tests"));

            if (context.BuildArgs.ExtraProperties.ContainsKey("separate-identity-server"))
            {
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.HttpApi.HostWithIds"));
                steps.Add(new AppTemplateChangeDbMigratorPortSettingsStep("44300"));
            }
            else
            {
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.HttpApi.Host"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.IdentityServer"));
                steps.Add(new AppTemplateProjectRenameStep("MyCompanyName.MyProjectName.HttpApi.HostWithIds", "MyCompanyName.MyProjectName.HttpApi.Host"));
                steps.Add(new AppTemplateChangeConsoleTestClientPortSettingsStep("44305"));
            }
        }

        private static void ConfigureWithBlazorUi(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Host"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Tests", projectFolderPath: "/aspnet-core/test/MyCompanyName.MyProjectName.Web.Tests"));

            if (context.BuildArgs.ExtraProperties.ContainsKey("separate-identity-server"))
            {
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.HttpApi.HostWithIds"));
                steps.Add(new BlazorAppsettingsFilePortChangeForSeparatedIdentityServersStep());
                steps.Add(new AppTemplateChangeDbMigratorPortSettingsStep("44300"));
            }
            else
            {
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.HttpApi.Host"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.IdentityServer"));
                steps.Add(new AppTemplateProjectRenameStep("MyCompanyName.MyProjectName.HttpApi.HostWithIds", "MyCompanyName.MyProjectName.HttpApi.Host"));
                steps.Add(new AppTemplateChangeConsoleTestClientPortSettingsStep("44305"));
            }
        }

        private static void ConfigureWithMvcUi(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            if (context.BuildArgs.ExtraProperties.ContainsKey("tiered"))
            {
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Tests", projectFolderPath: "/aspnet-core/test/MyCompanyName.MyProjectName.Web.Tests"));
                steps.Add(new AppTemplateProjectRenameStep("MyCompanyName.MyProjectName.Web.Host", "MyCompanyName.MyProjectName.Web"));
                steps.Add(new AppTemplateChangeDbMigratorPortSettingsStep("44300"));
            }
            else
            {
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Host"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.HttpApi.Host"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.IdentityServer"));
                steps.Add(new AppTemplateChangeConsoleTestClientPortSettingsStep("44303"));
            }

            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.HttpApi.HostWithIds"));
        }

        private static void ConfigureWithAngularUi(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Host"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Tests", projectFolderPath: "/aspnet-core/test/MyCompanyName.MyProjectName.Web.Tests"));

            if (context.BuildArgs.ExtraProperties.ContainsKey("separate-identity-server"))
            {
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.HttpApi.HostWithIds"));
                steps.Add(new AngularEnvironmentFilePortChangeForSeparatedIdentityServersStep());
                steps.Add(new AppTemplateChangeDbMigratorPortSettingsStep("44300"));

                if (context.BuildArgs.MobileApp == MobileApp.ReactNative)
                {
                    steps.Add(new ReactEnvironmentFilePortChangeForSeparatedIdentityServersStep());
                }
            }
            else
            {
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.HttpApi.Host"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.IdentityServer"));
                steps.Add(new AppTemplateProjectRenameStep("MyCompanyName.MyProjectName.HttpApi.HostWithIds", "MyCompanyName.MyProjectName.HttpApi.Host"));
                steps.Add(new AppTemplateChangeConsoleTestClientPortSettingsStep("44305"));
            }
        }

        private static void RemoveUnnecessaryPorts(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            steps.Add(new RemoveUnnecessaryPortsStep());
        }

        private static void RandomizeSslPorts(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            if (context.BuildArgs.ExtraProperties.ContainsKey("no-random-port"))
            {
                return;
            }

            steps.Add(new TemplateRandomSslPortStep(
                    new List<string>
                    {
                        "https://localhost:44300",
                        "https://localhost:44301",
                        "https://localhost:44302",
                        "https://localhost:44303",
                        "https://localhost:44305"
                    }
                )
            );
        }

        private static void RandomizeStringEncryption(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            steps.Add(new RandomizeStringEncryptionStep());
        }

        private static void UpdateNuGetConfig(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            steps.Add(new UpdateNuGetConfigStep("/aspnet-core/NuGet.Config"));
        }

        private static void RemoveMigrations(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            if (string.IsNullOrWhiteSpace(context.BuildArgs.Version) ||
                SemanticVersion.Parse(context.BuildArgs.Version) > new SemanticVersion(4,1,99))
            {
                steps.Add(new RemoveFolderStep("/aspnet-core/src/MyCompanyName.MyProjectName.EntityFrameworkCore.DbMigrations/Migrations"));
                steps.Add(new RemoveFolderStep("/aspnet-core/src/MyCompanyName.MyProjectName.EntityFrameworkCore.DbMigrations/TenantMigrations"));
            }
        }

        private static void ChangeConnectionString(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            if (context.BuildArgs.ConnectionString != null)
            {
                steps.Add(new ConnectionStringChangeStep());
            }
        }

        private static void CleanupFolderHierarchy(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            if ((context.BuildArgs.UiFramework == UiFramework.Mvc || context.BuildArgs.UiFramework == UiFramework.Blazor) &&
                context.BuildArgs.MobileApp == MobileApp.None)
            {
                steps.Add(new MoveFolderStep("/aspnet-core/", "/"));
            }
        }
    }
}
