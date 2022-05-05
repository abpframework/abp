using System.Linq;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

public class MoveFileStep : ProjectBuildPipelineStep
{
    private readonly string _filePath;
    private readonly string _newPath;

    public MoveFileStep(string filePath, string newPath)
    {
        _filePath = filePath;
        _newPath = newPath;
    }

    public override void Execute(ProjectBuildContext context)
    {
        var fileToMove = context.Files.Find(x => x.Name == _filePath);
        var newFileExist = context.Files.Any(x => x.Name == _newPath);
        
        if (fileToMove == null || newFileExist)
        {
            return;
        }

        fileToMove.SetName(_newPath);
    }
}
