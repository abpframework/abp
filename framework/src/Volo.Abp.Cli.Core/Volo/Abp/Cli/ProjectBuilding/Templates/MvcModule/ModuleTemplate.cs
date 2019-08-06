using System.Collections.Generic;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Building.Steps;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.MvcModule
{
    public class ModuleTemplate : TemplateInfo
    {
        /// <summary>
        /// "module".
        /// </summary>
        public const string TemplateName = "module";

        public ModuleTemplate()
            : base(TemplateName)
        {
            DocumentUrl = "https://docs.abp.io/en/abp/latest/Startup-Templates/Module";
        }

        public override IEnumerable<ProjectBuildPipelineStep> GetCustomSteps(ProjectBuildContext context)
        {
            var steps = new List<ProjectBuildPipelineStep>();

            DeleteUnrelatedProjects(context, steps);
            RandomizeSslPorts(context, steps);
            CleanupFolderHierarchy(context, steps);

            return steps;
        }

        private void DeleteUnrelatedProjects(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            if (context.BuildArgs.ExtraProperties.ContainsKey("no-ui"))
            {
                steps.Add(new RemoveProjectFromSolutionStep(
                    "MyCompanyName.MyProjectName.Web"
                ));

                steps.Add(new RemoveProjectFromSolutionStep(
                    "MyCompanyName.MyProjectName.Web.Host",
                    projectFolderPath: "/aspnet-core/host/MyCompanyName.MyProjectName.Web.Host"
                ));

                steps.Add(new RemoveProjectFromSolutionStep(
                    "MyCompanyName.MyProjectName.Web.Unified",
                    projectFolderPath: "/aspnet-core/host/MyCompanyName.MyProjectName.Web.Unified"
                ));
            }
        }

        private void RandomizeSslPorts(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            steps.Add(new TemplateRandomSslPortStep(new List<string>
            {
                "https://localhost:44300",
                "https://localhost:44301",
                "https://localhost:44302",
                "https://localhost:44303"
            }));
        }

        private void CleanupFolderHierarchy(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
        {
            steps.Add(new MoveFolderStep("/aspnet-core/", "/"));
        }
    }
}
