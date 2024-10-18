namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

public class RemoveFileStep : ProjectBuildPipelineStep
{
    private readonly string _filePath;
    private readonly bool _fullPath;

    public RemoveFileStep(string filePath, bool fullPath = true)
    {
        _filePath = filePath;
        _fullPath = fullPath;
    }

    public override void Execute(ProjectBuildContext context)
    {
        var fileToRemove = _fullPath
            ? context.Files.Find(x => x.Name == _filePath)
            : context.Files.Find(x => x.Name.EndsWith(_filePath));
        if (fileToRemove != null)
        {
            context.Files.Remove(fileToRemove);
        }
    }
}
