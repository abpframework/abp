using System;
using System.Linq;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

public class ProjectRenameStep : ProjectBuildPipelineStep
{
    private readonly string _oldName;
    private readonly string _newName;

    public ProjectRenameStep(string oldName, string newName)
    {
        _oldName = oldName;
        _newName = newName;
    }

    public override void Execute(ProjectBuildContext context)
    {
        var csprojFiles = context.Files.Where(f => f.Name.EndsWith(".csproj"));
        foreach (var file in csprojFiles)
        {
            if (file.Name.Contains(_oldName))
            {
                file.SetName(file.Name.Replace(_oldName, _newName));
            }
        }

        var slnFiles = context.Files.Where(f => f.Name.EndsWith(".sln"));
        foreach (var file in slnFiles)
        {
            file.NormalizeLineEndings();
            var lines = file.GetLines();

            for (var i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains(_oldName))
                {
                    lines[i] = lines[i].Replace(_oldName, _newName);
                }
            }
            file.SetLines(lines);
        }

        var directoryFiles = context.Files.Where(f => f.Name.Contains(_oldName));
        foreach (var file in directoryFiles)
        {
            file.SetName(file.Name.Replace(_oldName, _newName));
        }
    }

}
