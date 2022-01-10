using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

public class RenameProjectInTyeStep : ProjectBuildPipelineStep
{
    private readonly string _oldName;
    private readonly string _newName;

    public RenameProjectInTyeStep(string oldName, string newName)
    {
        _oldName = oldName;
        _newName = newName;
    }

    public override void Execute(ProjectBuildContext context)
    {
        var tyeFile = context.Files.FirstOrDefault(f => f.Name == "/tye.yaml");

        if (tyeFile == null)
        {
            return;
        }

        var lines = tyeFile.GetLines();
        var oldNameLine = $"- name: {_oldName}";
        var newNameLine = $"- name: {_newName}";

        var newLines = lines.Select(line => line.Equals(oldNameLine) ? newNameLine : line).ToList();

        tyeFile.SetContent(string.Join(Environment.NewLine, newLines));
    }

}
