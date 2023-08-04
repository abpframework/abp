namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

public class RemoveFilesStep : ProjectBuildPipelineStep
{
    private readonly string _filePath;

    public RemoveFilesStep(string filePath)
    {
        _filePath = filePath;
    }

    public override void Execute(ProjectBuildContext context)
    {
        context.Files.RemoveAll(file => file.Name.Contains(_filePath));
    }
}