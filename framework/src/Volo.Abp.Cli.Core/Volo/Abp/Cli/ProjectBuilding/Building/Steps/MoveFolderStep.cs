using System;
using System.Linq;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

public class MoveFolderStep : ProjectBuildPipelineStep
{
    private readonly string _sourceFolder;
    private readonly string _targetFolder;

    public MoveFolderStep(string sourceFolder, string targetFolder)
    {
        _sourceFolder = sourceFolder;
        _targetFolder = targetFolder;
    }

    public override void Execute(ProjectBuildContext context)
    {
        var fileEntries = context.Files.Where(file => file.Name.StartsWith(_sourceFolder)).ToList();
        foreach (var fileEntry in fileEntries)
        {
            var newName = fileEntry.Name.ReplaceFirst(_sourceFolder, _targetFolder);

            if (newName.IsIn("", "/"))
            {
                context.Files.Remove(fileEntry);
                continue;
            }

            fileEntry.SetName(newName);
        }
    }
}
