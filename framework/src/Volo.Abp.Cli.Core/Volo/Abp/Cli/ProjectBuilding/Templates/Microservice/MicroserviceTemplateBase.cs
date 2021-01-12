using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Building.Steps;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.Microservice
{
    public abstract class MicroserviceTemplateBase : TemplateInfo
    {
        protected MicroserviceTemplateBase([NotNull] string name)
            : base(name)
        {
        }

        public override IEnumerable<ProjectBuildPipelineStep> GetCustomSteps(ProjectBuildContext context)
        {
            var steps = new List<ProjectBuildPipelineStep>();

            DeleteUnrelatedProjects(context, steps);
            RandomizeStringEncryption(context, steps);
            UpdateNuGetConfig(context, steps);

            return steps;
        }

        private static void DeleteUnrelatedProjects(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            switch (context.BuildArgs.UiFramework)
            {
                case UiFramework.None:
                    steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web"));
                    steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.WebGateway"));
                    steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor"));
                    steps.Add(new RemoveFolderStep("/angular"));
                    break;

                case UiFramework.Angular:
                    steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web"));
                    steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.WebGateway"));
                    steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor"));
                    break;


                case UiFramework.Blazor:
                    steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web"));
                    steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.WebGateway"));
                    steps.Add(new RemoveFolderStep("/angular"));
                    break;

                case UiFramework.Mvc:
                case UiFramework.NotSpecified:
                    steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor"));
                    steps.Add(new RemoveFolderStep("/angular"));
                    break;
            }

            if (!context.BuildArgs.PublicWebSite)
            {
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.PublicWeb"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.PublicWebGateway"));
            }
        }

        private static void RandomizeStringEncryption(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            steps.Add(new RandomizeStringEncryptionStep());
        }

        private static void UpdateNuGetConfig(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            steps.Add(new UpdateNuGetConfigStep("/NuGet.Config"));
        }
    }
}
