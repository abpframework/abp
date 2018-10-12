using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Volo.Utils.SolutionTemplating.Building;
using Volo.Utils.SolutionTemplating.Building.Steps;

namespace Volo.AbpWebSite.Templates
{
    public class MvcApplicationTemplate : TemplateInfo
    {
        public MvcApplicationTemplate(IConfigurationRoot configuration)
            : base(
                "abp-mvc-app",
                new GithubRepositoryInfo("abpframework/abp", configuration["GithubAccessToken"]),
                "/templates/mvc")
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
            if (context.Request.DatabaseProvider == DatabaseProvider.MongoDb)
            {
                steps.Add(new SwitchEntityFrameworkCoreToMongoDbStep());
            }
        }

        private static void RemoveOtherDatabaseProviders(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            if (context.Request.DatabaseProvider != DatabaseProvider.EntityFrameworkCore)
            {
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.EntityFrameworkCore"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Application.Tests", projectFolderPath: "test/MyCompanyName.MyProjectName.Application.Tests"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Tests", projectFolderPath: "test/MyCompanyName.MyProjectName.Web.Tests"));
            }

            if (context.Request.DatabaseProvider != DatabaseProvider.MongoDb)
            {
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.MongoDB"));
            }
        }
    }
}