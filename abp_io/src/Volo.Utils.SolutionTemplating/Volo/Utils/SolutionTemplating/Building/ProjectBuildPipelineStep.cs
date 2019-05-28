namespace Volo.Utils.SolutionTemplating.Building
{
    public abstract class ProjectBuildPipelineStep
    {
        public abstract void Execute(ProjectBuildContext context);
    }
}