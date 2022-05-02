using System;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

public class RemoveFileStep : ProjectBuildPipelineStep
{
    private readonly string _filePath;
    public RemoveFileStep(string filePath)
    {
        _filePath = filePath;
    }

    public override void Execute(ProjectBuildContext context)
    {
        var fileToRemove = context.Files.Find(x => x.Name.EndsWith(_filePath));
        if (fileToRemove != null)
        {
            context.Files.Remove(fileToRemove);
        }
    }
}
