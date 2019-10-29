using System.Collections.Generic;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Building.Steps;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.App
{
    public abstract class AppTemplateBase : TemplateInfo
    {
        protected AppTemplateBase(string templateName)
            : base(templateName, DatabaseProvider.EntityFrameworkCore, UiFramework.Mvc)
        {
            DocumentUrl = "https://docs.abp.io/en/abp/latest/Startup-Templates/Application";
        }

        public static bool IsAppTemplate(string templateName)
        {
            return templateName == AppTemplate.TemplateName ||
                   templateName == AppProTemplate.TemplateName;
        }

        public override IEnumerable<ProjectBuildPipelineStep> GetCustomSteps(ProjectBuildContext context)
        {
            var steps = new List<ProjectBuildPipelineStep>();

            SwitchDatabaseProvider(context, steps);
            DeleteUnrelatedProjects(context, steps);
            RandomizeSslPorts(context, steps);
            UpdateNuGetConfig(context, steps);
            CleanupFolderHierarchy(context, steps);

            return steps;
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
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.MongoDB"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.MongoDB.Tests", projectFolderPath: "/aspnet-core/test/MyCompanyName.MyProjectName.MongoDB.Tests"));
            }
        }

        private void DeleteUnrelatedProjects(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            if (context.BuildArgs.UiFramework == UiFramework.Mvc)
            {
                if (context.BuildArgs.ExtraProperties.ContainsKey("tiered"))
                {
                    steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web"));
                    steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Tests", projectFolderPath: "/aspnet-core/test/MyCompanyName.MyProjectName.Web.Tests"));
                    steps.Add(new AppTemplateProjectRenameStep("MyCompanyName.MyProjectName.Web.Host", "MyCompanyName.MyProjectName.Web"));
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

            if (context.BuildArgs.UiFramework != UiFramework.Mvc)
            {
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Host"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Tests", projectFolderPath: "/aspnet-core/test/MyCompanyName.MyProjectName.Web.Tests"));

                if (context.BuildArgs.ExtraProperties.ContainsKey("separate-identity-server"))
                {
                    steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.HttpApi.HostWithIds"));
                    steps.Add(new AngularEnvironmentFilePortChangeForSeparatedIdentityServersStep());
                }
                else
                {
                    steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.HttpApi.Host"));
                    steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.IdentityServer"));
                    steps.Add(new AppTemplateProjectRenameStep("MyCompanyName.MyProjectName.HttpApi.HostWithIds", "MyCompanyName.MyProjectName.HttpApi.Host"));
                    steps.Add(new AppTemplateChangeConsoleTestClientPortSettingsStep("44305"));
                }
            }

            if (context.BuildArgs.UiFramework != UiFramework.Angular)
            {
                steps.Add(new RemoveFolderStep("/angular"));
            }
        }

        private static void RandomizeSslPorts(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
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

        private void UpdateNuGetConfig(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            steps.Add(new UpdateNuGetConfigStep("/aspnet-core/NuGet.Config"));
        }

        private void CleanupFolderHierarchy(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            if (context.BuildArgs.UiFramework == UiFramework.Mvc)
            {
                steps.Add(new MoveFolderStep("/aspnet-core/", "/"));
            }
        }
    }
}