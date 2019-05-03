using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.SolutionTemplating.Files;

namespace Volo.Abp.SolutionTemplating.Building.Steps
{
    public class SolutionRenameStep : ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            new SolutionRenamer(
                context.Files,
                "MyCompanyName",
                "MyProjectName",
                context.BuildArgs.SolutionName.CompanyName,
                context.BuildArgs.SolutionName.ProjectName
            ).Run();
        }

        private class SolutionRenamer
        {
            private readonly List<FileEntry> _entries;
            private readonly string _companyNamePlaceHolder;
            private readonly string _projectNamePlaceHolder;

            private readonly string _companyName;
            private readonly string _projectName;

            public SolutionRenamer(List<FileEntry> entries, string companyNamePlaceHolder, string projectNamePlaceHolder, string companyName, string projectName)
            {
                if (string.IsNullOrWhiteSpace(companyName))
                {
                    companyName = null;
                }

                if (companyNamePlaceHolder == null && companyName != null)
                {
                    throw new UserFriendlyException($"Can not set {nameof(companyName)} if {nameof(companyNamePlaceHolder)} is null.");
                }

                _entries = entries;

                _companyNamePlaceHolder = companyNamePlaceHolder;
                _projectNamePlaceHolder = projectNamePlaceHolder ?? throw new ArgumentNullException(nameof(projectNamePlaceHolder));

                _companyName = companyName;
                _projectName = projectName ?? throw new ArgumentNullException(nameof(projectName));
            }

            public void Run()
            {
                if (_companyNamePlaceHolder != null && _companyName != null)
                {
                    RenameDirectoryRecursively(_companyNamePlaceHolder, _companyName);
                    RenameAllFiles(_companyNamePlaceHolder, _companyName);
                    ReplaceContent(_companyNamePlaceHolder, _companyName);
                }
                else if (_companyNamePlaceHolder != null)
                {
                    RenameDirectoryRecursively(_companyNamePlaceHolder + "." + _projectNamePlaceHolder, _projectNamePlaceHolder);
                    RenameAllFiles(_companyNamePlaceHolder + "." + _projectNamePlaceHolder, _projectNamePlaceHolder);
                    ReplaceContent(_companyNamePlaceHolder + "." + _projectNamePlaceHolder, _projectNamePlaceHolder);
                }

                RenameDirectoryRecursively(_projectNamePlaceHolder, _projectName);
                RenameAllFiles(_projectNamePlaceHolder, _projectName);
                ReplaceContent(_projectNamePlaceHolder, _projectName);
            }

            private void RenameDirectoryRecursively(string placeHolder, string name)
            {
                foreach (var entry in _entries.Where(e => e.IsDirectory))
                {
                    if (entry.Name.Contains(placeHolder))
                    {
                        entry.SetName(entry.Name.Replace(placeHolder, name));
                    }
                }
            }

            private void RenameAllFiles(string placeHolder, string name)
            {
                foreach (var entry in _entries.Where(e => !e.IsDirectory))
                {
                    if (entry.Name.Contains(placeHolder))
                    {
                        entry.SetName(entry.Name.Replace(placeHolder, name));
                    }
                }
            }

            private void ReplaceContent(string placeHolder, string name)
            {
                foreach (var entry in _entries.Where(e => !e.IsDirectory))
                {
                    if (entry.Content.Length < placeHolder.Length)
                    {
                        continue;
                    }

                    if (entry.IsBinaryFile)
                    {
                        continue;
                    }

                    var newContent = entry.Content.Replace(placeHolder, name);
                    if (newContent != entry.Content)
                    {
                        entry.SetContent(newContent);
                    }
                }
            }
        }
    }
}