using System;
using System.Linq;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

public class RemoveFoldersStep : ProjectBuildPipelineStep
{
    private readonly string _folderPath;

    public RemoveFoldersStep(string folderPath)
    {
        _folderPath = folderPath;
    }

    public override void Execute(ProjectBuildContext context)
    {
        context.Files.RemoveAll(file => file.Name.Contains(_folderPath));
    }
}