using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Building.Steps;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.Module
{
    public abstract class ModuleTemplateBase : TemplateInfo
    {
        protected ModuleTemplateBase([NotNull] string name)
            : base(name)
        {
        }

        public override IEnumerable<ProjectBuildPipelineStep> GetCustomSteps(ProjectBuildContext context)
        {
            var steps = new List<ProjectBuildPipelineStep>();

            DeleteUnrelatedProjects(context, steps);
            RandomizeSslPorts(context, steps);
            UpdateNuGetConfig(context, steps);
            ChangeConnectionString(context, steps);
            CleanupFolderHierarchy(context, steps);

            return steps;
        }

        private void DeleteUnrelatedProjects(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            if (!context.BuildArgs.ExtraProperties.ContainsKey("no-ui"))
            {
                return;
            }

            steps.Add(new RemoveProjectFromSolutionStep(
                "MyCompanyName.MyProjectName.Web"
            ));

            steps.Add(new RemoveProjectFromSolutionStep(
                "MyCompanyName.MyProjectName.Blazor"
            ));

            steps.Add(new RemoveProjectFromSolutionStep(
                "MyCompanyName.MyProjectName.Blazor.Server"
            ));

            steps.Add(new RemoveProjectFromSolutionStep(
                "MyCompanyName.MyProjectName.Blazor.WebAssembly"
            ));

            steps.Add(new RemoveProjectFromSolutionStep(
                "MyCompanyName.MyProjectName.Blazor.Host",
                projectFolderPath: "/aspnet-core/host/MyCompanyName.MyProjectName.Blazor.Host"
            ));

            steps.Add(new RemoveProjectFromSolutionStep(
                "MyCompanyName.MyProjectName.Blazor.Server.Host",
                projectFolderPath: "/aspnet-core/host/MyCompanyName.MyProjectName.Blazor.Server.Host"
            ));

            steps.Add(new RemoveProjectFromSolutionStep(
                "MyCompanyName.MyProjectName.Web.Host",
                projectFolderPath: "/aspnet-core/host/MyCompanyName.MyProjectName.Web.Host"
            ));

            steps.Add(new RemoveProjectFromSolutionStep(
                "MyCompanyName.MyProjectName.Web.Unified",
                projectFolderPath: "/aspnet-core/host/MyCompanyName.MyProjectName.Web.Unified"
            ));

            steps.Add(new RemoveFolderStep("/angular"));
        }

        private void RandomizeSslPorts(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            if (context.BuildArgs.ExtraProperties.ContainsKey("no-random-port"))
            {
                return;
            }

            steps.Add(new TemplateRandomSslPortStep(new List<string>
            {
                "https://localhost:44300",
                "https://localhost:44301",
                "https://localhost:44302",
                "https://localhost:44303"
            }));
        }

        private static void UpdateNuGetConfig(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            steps.Add(new UpdateNuGetConfigStep("/aspnet-core/NuGet.Config"));
            steps.Add(new UpdateNuGetConfigStep("/NuGet.Config"));
        }

        private static void ChangeConnectionString(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            if (context.BuildArgs.ConnectionString != null)
            {
                steps.Add(new ConnectionStringChangeStep());
            }
        }

        private void CleanupFolderHierarchy(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            steps.Add(new MoveFolderStep("/aspnet-core/", "/"));
        }
    }
}
