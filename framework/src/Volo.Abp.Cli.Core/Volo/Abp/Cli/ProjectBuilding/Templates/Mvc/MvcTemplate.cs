using System.Collections.Generic;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Building.Steps;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.Mvc
{
    public class MvcTemplate : TemplateInfo
    {
        /// <summary>
        /// "mvc".
        /// </summary>
        public const string TemplateName = "mvc";

        public MvcTemplate()
            : base(TemplateName, DatabaseProvider.EntityFrameworkCore)
        {

        }

        public override IEnumerable<ProjectBuildPipelineStep> GetCustomSteps(ProjectBuildContext context)
        {
            var steps = new List<ProjectBuildPipelineStep>();

            SwitchDatabaseProvider(context, steps);
            DeleteUnrelatedProjects(context, steps);
            ChangeLocalhostPort(steps);

            return steps;
        }

        private static void SwitchDatabaseProvider(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            if (context.BuildArgs.DatabaseProvider == DatabaseProvider.MongoDb)
            {
                steps.Add(new MvcTemplateSwitchEntityFrameworkCoreToMongoDbStep());
            }

            if (context.BuildArgs.DatabaseProvider != DatabaseProvider.EntityFrameworkCore)
            {
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.EntityFrameworkCore"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.EntityFrameworkCore.DbMigrations"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.EntityFrameworkCore.Tests", projectFolderPath: "/test/MyCompanyName.MyProjectName.EntityFrameworkCore.Tests"));
            }

            if (context.BuildArgs.DatabaseProvider != DatabaseProvider.MongoDb)
            {
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.MongoDB"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.MongoDB.Tests", projectFolderPath: "/test/MyCompanyName.MyProjectName.MongoDB.Tests"));
            }
        }

        private void DeleteUnrelatedProjects(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            if (context.BuildArgs.ExtraProperties.ContainsKey("tiered"))
            {
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Tests", projectFolderPath: "/test/MyCompanyName.MyProjectName.Web.Tests"));

                steps.Add(new MvcTemplateProjectRenameStep("MyCompanyName.MyProjectName.Web.Host", "MyCompanyName.MyProjectName.Web"));
            }
            else
            {
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Host"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.HttpApi.Host"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.IdentityServer"));
            }
        }

        private void ChangeLocalhostPort(List<ProjectBuildPipelineStep> steps)
        {
            //Disabled this since port change is complext and should be re-considered later

            //steps.Add(
            //    new ChangeLocalhostPortStep(
            //        "/src/MyCompanyName.MyProjectName.Web/Properties/launchSettings.json",
            //        53929,
            //        53932
            //    )
            //);
        }
    }
}