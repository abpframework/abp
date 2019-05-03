using System.Collections.Generic;
using Volo.Abp.ProjectBuilding.Building;
using Volo.Abp.ProjectBuilding.Building.Steps;

namespace Volo.Abp.ProjectBuilding.Templates
{
    public class MvcApplicationTemplate : TemplateInfo
    {
        /// <summary>
        /// "mvc".
        /// </summary>
        public const string TemplateName = "mvc";

        public MvcApplicationTemplate()
            : base(TemplateName, DatabaseProvider.EntityFrameworkCore)
        {
        }

        public override IEnumerable<ProjectBuildPipelineStep> GetCustomSteps(ProjectBuildContext context)
        {
            var steps = new List<ProjectBuildPipelineStep>();
            SwitchDatabaseProvider(context, steps);
            RemoveOtherDatabaseProviders(context, steps);
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
    }
}