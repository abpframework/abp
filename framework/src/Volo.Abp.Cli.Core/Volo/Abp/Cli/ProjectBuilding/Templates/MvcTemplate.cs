using System.Collections.Generic;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Building.Steps;

namespace Volo.Abp.Cli.ProjectBuilding.Templates
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
            RemoveOtherDatabaseProviders(context, steps);
            ChangeLocalhostPort(steps);
            return steps;
        }

        private static void SwitchDatabaseProvider(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            if (context.BuildArgs.DatabaseProvider == DatabaseProvider.MongoDb)
            {
                steps.Add(new SwitchEntityFrameworkCoreToMongoDbStep());
            }
        }

        private static void RemoveOtherDatabaseProviders(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            if (context.BuildArgs.DatabaseProvider != DatabaseProvider.EntityFrameworkCore)
            {
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.EntityFrameworkCore"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.EntityFrameworkCore.DbMigrations"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Application.Tests", projectFolderPath: "test/MyCompanyName.MyProjectName.Application.Tests"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Tests", projectFolderPath: "test/MyCompanyName.MyProjectName.Web.Tests"));
            }

            if (context.BuildArgs.DatabaseProvider != DatabaseProvider.MongoDb)
            {
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.MongoDB"));
            }
        }

        private void ChangeLocalhostPort(List<ProjectBuildPipelineStep> steps)
        {
            steps.Add(
                new ChangeLocalhostPortStep(
                    "/src/MyCompanyName.MyProjectName.Web/Properties/launchSettings.json",
                    53929,
                    53932
                )
            );
        }
    }
}