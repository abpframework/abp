using System.Collections.Generic;

namespace Volo.Utils.SolutionTemplating.Building
{
    public class ProjectBuildPipeline
    {
        public List<ProjectBuildPipelineStep> Steps { get; }

        public ProjectBuildPipeline()
        {
            Steps = new List<ProjectBuildPipelineStep>();
        }

        public void Execute(ProjectBuildContext context)
        {
            foreach (var step in Steps)
            {
                step.Execute(context);
            }
        }
    }
}