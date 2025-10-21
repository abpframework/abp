using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Newtonsoft.Json.Linq;
using Volo.Abp.Cli.ProjectBuilding.Files;
using Formatting = Newtonsoft.Json.Formatting;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

public class RemoveProjectFromSolutionStep : ProjectBuildPipelineStep
{
    private readonly string _projectName;
    private string _solutionFilePathWithoutFileExtension;
    private string _projectFolderPath;

    private string ProjectNameWithQuotes => $"\"{_projectName}\"";

    public RemoveProjectFromSolutionStep(
        string projectName,
        string solutionFilePathWithoutFileExtension = null,
        string projectFolderPath = null)
    {
        _projectName = projectName;
        _projectFolderPath = projectFolderPath;
        
        if (solutionFilePathWithoutFileExtension != null && solutionFilePathWithoutFileExtension.EndsWith(".sln"))
        {
            _solutionFilePathWithoutFileExtension = solutionFilePathWithoutFileExtension.RemovePostFix(".sln");
        }
        
        if (solutionFilePathWithoutFileExtension != null && solutionFilePathWithoutFileExtension.EndsWith(".slnx"))
        {
            _solutionFilePathWithoutFileExtension = solutionFilePathWithoutFileExtension.RemovePostFix(".slnx");
        }
        else
        {
            _solutionFilePathWithoutFileExtension = solutionFilePathWithoutFileExtension;
        }
    }

    public override void Execute(ProjectBuildContext context)
    {
        SetSolutionAndProjectPathsIfNull(context);

        if (_solutionFilePathWithoutFileExtension == null || _projectFolderPath == null)
        {
            return;
        }

        new RemoveFolderStep(_projectFolderPath).Execute(context);
        var solutionFile = context.FindFile(_solutionFilePathWithoutFileExtension + ".sln")
            ?? context.GetFile(_solutionFilePathWithoutFileExtension + ".slnx");

        if (solutionFile.Name.EndsWith(".sln"))
        {
            RemoveProjectFromSlnFile(solutionFile);
        }
        else
        {
            RemoveProjectFromSlnxFile(solutionFile);
        }

        RemoveProjectFromAbpmdlFile(context);
    }

    private void RemoveProjectFromSlnxFile(FileEntry solutionFile)
    {
        var document = new XmlDocument { PreserveWhitespace = true };
        document.LoadXml(solutionFile.Content);
        var projectNodes = document.SelectNodes("//Project");

        if (projectNodes == null || projectNodes.Count < 1)
        {
            return;
        }
        
        var nodesToBeRemoved = new List<XmlNode>();
        foreach (XmlNode projectNode in projectNodes)
        {
            var pathAttr = projectNode.Attributes?["Path"]?.Value;
            if (string.IsNullOrWhiteSpace(pathAttr))
            {
                continue;
            }

            var normalized = pathAttr.Replace('\\', '/');
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(normalized);

            if (string.Equals(fileNameWithoutExtension, _projectName, StringComparison.OrdinalIgnoreCase))
            {
                nodesToBeRemoved.Add(projectNode);
            }
        }

        foreach (var node in nodesToBeRemoved)
        {
            node.ParentNode!.RemoveChild(node);
        }
        
        solutionFile.SetContent(
            document.OuterXml
            .SplitToLines()
            .Where(x=> !x.Trim().Equals(string.Empty))
            .JoinAsString(Environment.NewLine));
    }

    private void RemoveProjectFromSlnFile(FileEntry solutionFile)
    {
        solutionFile.NormalizeLineEndings();
        solutionFile.SetLines(RemoveProject(solutionFile.GetLines().ToList()));
    }

    private void RemoveProjectFromAbpmdlFile(ProjectBuildContext context)
    {
        var abpmdlFile = context.FindFile(_solutionFilePathWithoutFileExtension + ".abpmdl");

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
        if (_solutionFilePathWithoutFileExtension == null)
        {
            _solutionFilePathWithoutFileExtension = context.FindFile("/aspnet-core/MyCompanyName.MyProjectName.sln")?.Name.RemovePostFix(".sln") ??
                                context.FindFile("/aspnet-core/MyCompanyName.MyProjectName.slnx")?.Name.RemovePostFix(".slnx") ??
                                context.FindFile("/MyCompanyName.MyProjectName.sln")?.Name.RemovePostFix(".sln") ??
                                context.FindFile("/MyCompanyName.MyProjectName.slnx")?.Name.RemovePostFix(".slnx") ??
                                context.FindFile("/MyCompanyName.MyProjectName.MicroserviceName.sln")?.Name.RemovePostFix(".sln") ??
                                context.FindFile("/MyCompanyName.MyProjectName.MicroserviceName.slnx")?.Name.RemovePostFix(".slnx");
        }
        if (_projectFolderPath == null)
        {
            _projectFolderPath = context.FindFile("/aspnet-core/src/" + _projectName.EnsureEndsWith('/'))?.Name ??
                                 context.FindFile("/src/" + _projectName.EnsureEndsWith('/'))?.Name ??
                                 context.FindFile("/aspnet-core/" + _projectName.EnsureEndsWith('/'))?.Name;
        }
    }
}
