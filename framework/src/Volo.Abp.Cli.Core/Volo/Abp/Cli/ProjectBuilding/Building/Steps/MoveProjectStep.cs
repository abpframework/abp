using System;
using System.Linq;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

public class MoveProjectStep : ProjectBuildPipelineStep
{
    private readonly string _pathInSlnFile;
    private readonly string _newPathInSlnFile;
    private readonly string _projectFolder;
    private readonly string _newProjectFolder;

    public MoveProjectStep(string projectFolder, string newProjectFolder, string pathInSlnFile, string newPathInSlnFile)
    {
        _pathInSlnFile = pathInSlnFile;
        _newPathInSlnFile = newPathInSlnFile;
        _projectFolder = projectFolder.EnsureEndsWith('/');
        _newProjectFolder = newProjectFolder.EnsureEndsWith('/');
    }

    public override void Execute(ProjectBuildContext context)
    {
        var fileEntries = context.Files.Where(file => file.Name.StartsWith(_projectFolder)).ToList();
        foreach (var fileEntry in fileEntries)
        {
            var newName = fileEntry.Name.ReplaceFirst(_projectFolder, _newProjectFolder);

            if (newName.IsIn("", "/"))
            {
                context.Files.Remove(fileEntry);
                continue;
            }

            fileEntry.SetName(newName);
        }

        var slnFile = context.Files.First(file => file.Name.EndsWith(".sln"));
        slnFile.SetContent(slnFile.Content.Replace($"\"{_pathInSlnFile}\"", $"\"{_newPathInSlnFile}\""));
    }
}
