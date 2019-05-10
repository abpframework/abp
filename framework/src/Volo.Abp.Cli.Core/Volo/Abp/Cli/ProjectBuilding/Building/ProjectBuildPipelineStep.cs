namespace Volo.Abp.Cli.ProjectBuilding.Building
{
    public abstract class ProjectBuildPipelineStep
    {
        public abstract void Execute(ProjectBuildContext context);
    }
}