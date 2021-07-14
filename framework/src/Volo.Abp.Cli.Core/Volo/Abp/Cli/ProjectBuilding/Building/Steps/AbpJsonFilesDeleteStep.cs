namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps
{
    public class AbpJsonFilesDeleteStep : ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            context.Files.RemoveAll(f =>
                f.Name.EndsWith(".abpsln.json") ||
                f.Name.EndsWith(".abpmdl.json") ||
                f.Name.EndsWith(".abppkg.json")
            );
        }
    }
}
