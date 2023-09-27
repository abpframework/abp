using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using Volo.Abp.Cli.ProjectBuilding.Files;
using Volo.Abp.Cli.ProjectBuilding.Templates.Microservice;
using Volo.Abp.Cli.Utils;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

public class ProjectReferenceReplaceStep : ProjectBuildPipelineStep
{
    public override void Execute(ProjectBuildContext context)
    {
        if (context.BuildArgs.ExtraProperties.ContainsKey("local-framework-ref"))
        {
            var localAbpRepoPath = context.BuildArgs.AbpGitHubLocalRepositoryPath;

            if (string.IsNullOrWhiteSpace(localAbpRepoPath))
            {
                return;
            }

            var localVoloRepoPath = context.BuildArgs.VoloGitHubLocalRepositoryPath;

            new ProjectReferenceReplacer.LocalProjectPathReferenceReplacer(
                context,
                context.Module?.Namespace ?? "MyCompanyName.MyProjectName",
                localAbpRepoPath,
                localVoloRepoPath
            ).Run();
        }
        else
        {
            var nugetPackageVersion = context.TemplateFile.RepositoryNugetVersion;

            if (IsBranchName(nugetPackageVersion))
            {
                nugetPackageVersion = context.TemplateFile.LatestVersion;
            }

            new ProjectReferenceReplacer.NugetReferenceReplacer(
                context,
                context.Module?.Namespace ?? "MyCompanyName.MyProjectName",
                nugetPackageVersion
            ).Run();
        }
    }

    private bool IsBranchName(string versionOrBranchName)
    {
        Check.NotNullOrWhiteSpace(versionOrBranchName, nameof(versionOrBranchName));

        if (char.IsDigit(versionOrBranchName[0]))
        {
            return false;
        }

        if (versionOrBranchName[0].IsIn('v', 'V') &&
            versionOrBranchName.Length > 1 &&
            char.IsDigit(versionOrBranchName[1]))
        {
            return false;
        }

        return true;
    }

    private abstract class ProjectReferenceReplacer
    {
        private readonly List<FileEntry> _entries;
        private readonly bool _isMicroserviceServiceTemplate;
        private readonly string _projectName;

        protected ProjectReferenceReplacer(
            ProjectBuildContext context,
            string projectName)
        {
            _entries = context.Files;
            _isMicroserviceServiceTemplate = MicroserviceServiceTemplateBase.IsMicroserviceServiceTemplate(context.Template?.Name);
            _projectName = projectName;
        }

        public void Run()
        {
            //get Directory.Packages.props file
            var cpmFile = _entries.Where(x => x.Name.EndsWith("Directory.Packages.props"))
                .Select(x => new {FileEntry = x, Depth = x.Name.Split('/').Length}).OrderBy(x => x.Depth)
                .Select(x => x.FileEntry).FirstOrDefault();

            XmlDocument nugetCpmXmlDoc = null;
            XmlNode nugetCpmXmlNode = null;
            if (cpmFile != null && !cpmFile.Content.IsNullOrWhiteSpace())
            {
                using var stream = StreamHelper.GenerateStreamFromString(cpmFile.Content);
                nugetCpmXmlDoc = new XmlDocument { PreserveWhitespace = true };
                nugetCpmXmlDoc.Load(stream);
                nugetCpmXmlNode = nugetCpmXmlDoc.SelectSingleNode("/Project/ItemGroup");
            }

            var csprojFiles = _entries.Where(fileEntry => fileEntry.Name.EndsWith(".csproj"));

            foreach (var fileEntry in csprojFiles)
            {
                fileEntry.SetContent(ProcessFileContent(fileEntry.Content, nugetCpmXmlDoc, nugetCpmXmlNode));
            }

            if (nugetCpmXmlNode != null)
            {
                cpmFile.SetContent(nugetCpmXmlDoc.OuterXml);
            }
        }

        private string ProcessFileContent(string content, XmlDocument nugetCpmXmlDoc, XmlNode nugetCpmXmlNode)
        {
            Check.NotNull(content, nameof(content));

            using var stream = StreamHelper.GenerateStreamFromString(content);
            var doc = new XmlDocument { PreserveWhitespace = true };
            doc.Load(stream);
            return ProcessReferenceNodes(doc, content, nugetCpmXmlDoc, nugetCpmXmlNode);
        }

        private string ProcessReferenceNodes(XmlDocument doc, string content, XmlDocument nugetCpmXmlDoc, XmlNode nugetCpmXmlNode)
        {
            Check.NotNull(content, nameof(content));

            var nodes = doc.SelectNodes("/Project/ItemGroup/ProjectReference[@Include]");

            foreach (XmlNode oldNode in nodes)
            {
                var oldNodeIncludeValue = oldNode.Attributes["Include"].Value;

                // ReSharper disable once PossibleNullReferenceException : Can not be null because nodes are selected with include attribute filter in previous method
                if (oldNodeIncludeValue.Contains(_projectName) && _isMicroserviceServiceTemplate)
                {
                    continue;
                }
                if(_entries.Any(e => e.Name.EndsWith(GetProjectNameWithExtensionFromProjectReference(oldNodeIncludeValue))))
                {
                    continue;
                }

                XmlNode newNode = GetNewReferenceNode(doc, oldNodeIncludeValue, nugetCpmXmlDoc, nugetCpmXmlNode);

                oldNode.ParentNode.ReplaceChild(newNode, oldNode);
            }

            return doc.OuterXml;
        }

        private string GetProjectNameWithExtensionFromProjectReference(string oldNodeIncludeValue)
        {
            if (string.IsNullOrWhiteSpace(oldNodeIncludeValue))
            {
                return oldNodeIncludeValue;
            }

            return oldNodeIncludeValue.Split('\\', '/').Last();
        }

        protected abstract XmlElement GetNewReferenceNode(XmlDocument doc, string oldNodeIncludeValue, XmlDocument nugetCpmXmlDoc, XmlNode nugetCpmXmlNode);


        public class NugetReferenceReplacer : ProjectReferenceReplacer
        {
            private readonly string _nugetPackageVersion;

            public NugetReferenceReplacer(ProjectBuildContext context, string projectName, string nugetPackageVersion)
                : base(context, projectName)
            {
                _nugetPackageVersion = nugetPackageVersion;
            }

            protected override XmlElement GetNewReferenceNode(XmlDocument doc, string oldNodeIncludeValue, XmlDocument nugetCpmXmlDoc, XmlNode nugetCpmXmlNode)
            {
                var newNode = doc.CreateElement("PackageReference");
                
                var includeAttr = doc.CreateAttribute("Include");
                includeAttr.Value = ConvertToNugetReference(oldNodeIncludeValue);
                newNode.Attributes.Append(includeAttr);

                if (nugetCpmXmlNode==null)
                {
                    var versionAttr = doc.CreateAttribute("Version");
                    versionAttr.Value = _nugetPackageVersion;
                    newNode.Attributes.Append(versionAttr);
                }
                else
                {
                    // use nuget CPM
                    if (nugetCpmXmlDoc.SelectSingleNode($"/Project/ItemGroup/PackageVersion[@Include='{includeAttr.Value}']") == null)
                    {
                        var newPackageVersionNode = nugetCpmXmlDoc.CreateElement("PackageVersion");

                        var newPackageVersionNodeIncludeAttr = nugetCpmXmlDoc.CreateAttribute("Include");
                        newPackageVersionNodeIncludeAttr.Value = includeAttr.Value;
                        newPackageVersionNode.Attributes.Append(newPackageVersionNodeIncludeAttr);

                        var newPackageVersionNodeVersionAttr = nugetCpmXmlDoc.CreateAttribute("Version");
                        newPackageVersionNodeVersionAttr.Value = _nugetPackageVersion;
                        newPackageVersionNode.Attributes.Append(newPackageVersionNodeVersionAttr);

                        nugetCpmXmlNode.AppendChild(newPackageVersionNode);
                    }
                }

                return newNode;
            }

            private string ConvertToNugetReference(string oldValue)
            {
                var newValue = Regex.Match(oldValue, @"\\((?!.+?\\).+?)\.csproj", RegexOptions.CultureInvariant | RegexOptions.Compiled);
                if (newValue.Success && newValue.Groups.Count == 2)
                {
                    return newValue.Groups[1].Value;
                }

                return oldValue;
            }
        }


        public class LocalProjectPathReferenceReplacer : ProjectReferenceReplacer
        {
            private readonly string _gitHubAbpLocalRepositoryPath;
            private readonly string _gitHubVoloLocalRepositoryPath;

            public LocalProjectPathReferenceReplacer(ProjectBuildContext context, string projectName, string gitHubAbpLocalRepositoryPath, string gitHubVoloLocalRepositoryPath)
                : base(context, projectName)
            {
                _gitHubAbpLocalRepositoryPath = gitHubAbpLocalRepositoryPath;
                _gitHubVoloLocalRepositoryPath = gitHubVoloLocalRepositoryPath;
            }

            protected override XmlElement GetNewReferenceNode(XmlDocument doc, string oldNodeIncludeValue, XmlDocument nugetCpmXmlDoc, XmlNode nugetCpmXmlNode)
            {
                var newNode = doc.CreateElement("ProjectReference");

                var includeAttr = doc.CreateAttribute("Include");
                includeAttr.Value = SetGithubPath(oldNodeIncludeValue);
                newNode.Attributes.Append(includeAttr);

                return newNode;
            }

            private string SetGithubPath(string includeValue)
            {
                while (includeValue.StartsWith("..\\"))
                {
                    includeValue = includeValue.TrimStart('.');
                    includeValue = includeValue.TrimStart('\\');
                }

                if (!string.IsNullOrWhiteSpace(_gitHubVoloLocalRepositoryPath))
                {
                    if (includeValue.StartsWith("abp\\", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return _gitHubAbpLocalRepositoryPath.EnsureEndsWith('\\') + includeValue.Substring("abp\\".Length);
                    }

                    return _gitHubVoloLocalRepositoryPath.EnsureEndsWith('\\') + "abp\\" + includeValue;
                }

                return _gitHubAbpLocalRepositoryPath.EnsureEndsWith('\\') + includeValue;
            }
        }
    }
}
