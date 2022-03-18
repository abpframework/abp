using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

public class RemoveProjectFromSolutionStep : ProjectBuildPipelineStep
{
    private readonly string _projectName;
    private string _solutionFilePath;
    private string _projectFolderPath;

    private string ProjectNameWithQuotes => $"\"{_projectName}\"";

    public RemoveProjectFromSolutionStep(
        string projectName,
        string solutionFilePath = null,
        string projectFolderPath = null)
    {
        _projectName = projectName;
        _solutionFilePath = solutionFilePath;
        _projectFolderPath = projectFolderPath;
    }

    public override void Execute(ProjectBuildContext context)
    {
        SetSolutionAndProjectPathsIfNull(context);

        if (_solutionFilePath == null || _projectFolderPath == null)
        {
            return;
        }

        new RemoveFolderStep(_projectFolderPath).Execute(context);
        var solutionFile = context.GetFile(_solutionFilePath);
        solutionFile.NormalizeLineEndings();
        solutionFile.SetLines(RemoveProject(solutionFile.GetLines().ToList()));

        RemoveProjectFromAbpmdlFile(context);
    }

    private void RemoveProjectFromAbpmdlFile(ProjectBuildContext context)
    {
        var abpmdlFile = context.FindFile(_solutionFilePath.RemovePostFix(".sln") + ".abpmdl.json");

        if (abpmdlFile == null)
        {
            return;
        }

        var jsonRoot = JObject.Parse(abpmdlFile.Content);
        var packagesObj = (JObject)jsonRoot["packages"];

        packagesObj?.Remove(_projectName);

        abpmdlFile.SetContent(jsonRoot.ToString(Formatting.Indented));
    }

    private List<string> RemoveProject(List<string> solutionFileLines)
    {
        var projectKey = FindProjectKey(solutionFileLines);

        if (projectKey == null)
        {
            return solutionFileLines;
        }

        var newSolutionFileLines = new List<string>();
        var firstOccurence = true;

        for (var i = 0; i < solutionFileLines.Count; ++i)
        {
            if (solutionFileLines[i].Contains(projectKey))
            {
                if (firstOccurence)
                {
                    firstOccurence = false;
                    ++i; //Skip "EndProject" line too.
                }

                continue;
            }

            newSolutionFileLines.Add(solutionFileLines[i]);
        }

        return newSolutionFileLines;
    }

    private string FindProjectKey(List<string> solutionFileLines)
    {
        foreach (var solutionFileLine in solutionFileLines)
        {
            if (solutionFileLine.Contains(ProjectNameWithQuotes))
            {
                var curlyBracketStartIndex = solutionFileLine.LastIndexOf("{", StringComparison.OrdinalIgnoreCase);
                var curlyBracketEndIndex = solutionFileLine.LastIndexOf("}", StringComparison.OrdinalIgnoreCase);
                return solutionFileLine.Substring(curlyBracketStartIndex + 1, curlyBracketEndIndex - curlyBracketStartIndex - 1);
            }
        }

        return null;
    }

    private void SetSolutionAndProjectPathsIfNull(ProjectBuildContext context)
    {
        if (_solutionFilePath == null)
        {
            _solutionFilePath = context.FindFile("/aspnet-core/MyCompanyName.MyProjectName.sln")?.Name ??
                                context.FindFile("/MyCompanyName.MyProjectName.sln")?.Name ??
                                context.FindFile("/MyCompanyName.MyProjectName.MicroserviceName.sln")?.Name;
        }
        if (_projectFolderPath == null)
        {
            _projectFolderPath = context.FindFile("/aspnet-core/src/" + _projectName.EnsureEndsWith('/'))?.Name ??
                                 context.FindFile("/src/" + _projectName.EnsureEndsWith('/'))?.Name ??
                                 context.FindFile("/aspnet-core/" + _projectName.EnsureEndsWith('/'))?.Name;
        }
    }
}
