using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps
{
    public class RemoveProjectFromSolutionStep : ProjectBuildPipelineStep
    {
        private readonly string _projectName;
        private readonly string _solutionFilePath;
        private readonly string _projectFolderPath;

        private string ProjectNameWithQuotes => $"\"{_projectName}\"";

        public RemoveProjectFromSolutionStep(
            string projectName,
            string solutionFilePath = null,
            string projectFolderPath = null)
        {
            _projectName = projectName;
            _solutionFilePath = solutionFilePath ?? "/aspnet-core/MyCompanyName.MyProjectName.sln";
            _projectFolderPath = projectFolderPath ?? ("/aspnet-core/src/" + projectName);
        }

        public override void Execute(ProjectBuildContext context)
        {
            new RemoveFolderStep(_projectFolderPath).Execute(context);
            var solutionFile = context.GetFile(_solutionFilePath);
            solutionFile.NormalizeLineEndings();
            solutionFile.SetLines(RemoveProject(solutionFile.GetLines().ToList()));
        }

        private List<string> RemoveProject(List<string> solutionFileLines)
        {
            var projectKey = FindProjectKey(solutionFileLines);
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

            throw new ApplicationException($"The solution file '{_solutionFilePath}' does not contain a project '{_projectName}'");
        }
    }
}