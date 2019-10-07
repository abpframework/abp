using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps
{
    public class NugetReferenceReplaceStep : ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            var nugetPackageVersion = context.TemplateFile.Version;
            if (IsBranchName(nugetPackageVersion))
            {
                nugetPackageVersion = context.TemplateFile.LatestVersion;
            }

            new NugetReferenceReplacer(
                context.Files,
                "MyCompanyName.MyProjectName",
                nugetPackageVersion,
                context.BuildArgs.ExtraProperties.ContainsKey("local-framework-ref"),
                context.BuildArgs.GitHubLocalRepositoryPath
            ).Run();
        }

        private bool IsBranchName(string versionOrBranchName)
        {
            Check.NotNullOrWhiteSpace(versionOrBranchName, nameof(versionOrBranchName));

            if (char.IsDigit(versionOrBranchName[0]))
            {
                return false;
            }

            if (versionOrBranchName[0].IsIn('v','V') &&
                versionOrBranchName.Length > 1 && 
                char.IsDigit(versionOrBranchName[1]))
            {
                return false;
            }

            return true;
        }

        private class NugetReferenceReplacer
        {
            private readonly List<FileEntry> _entries;
            private readonly string _companyAndProjectNamePlaceHolder;
            private readonly string _nugetPackageVersion;
            private readonly bool _localReferences;
            private readonly string _gitHubLocalRepositoryPath;

            public NugetReferenceReplacer(
                List<FileEntry> entries, 
                string companyAndProjectNamePlaceHolder, 
                string nugetPackageVersion,
                bool localReferences = false,
                string gitHubLocalRepositoryPath = null)
            {
                _entries = entries;
                _companyAndProjectNamePlaceHolder = companyAndProjectNamePlaceHolder;
                _nugetPackageVersion = nugetPackageVersion;
                _localReferences = localReferences;
                _gitHubLocalRepositoryPath = gitHubLocalRepositoryPath;
            }

            public void Run()
            {
                foreach (var fileEntry in _entries)
                {
                    if (fileEntry.Name.EndsWith(".csproj"))
                    {
                        fileEntry.SetContent(ProcessFileContent(fileEntry.Content));
                    }
                }
            }

            private string ProcessFileContent(string content)
            {
                Check.NotNull(content, nameof(content));

                var doc = new XmlDocument() { PreserveWhitespace = true };

                doc.Load(GenerateStreamFromString(content));

                return ProcessReferenceNodes(doc, content);
            }

            private string ProcessReferenceNodes(XmlDocument doc, string content)
            {
                Check.NotNull(content, nameof(content));

                var nodes = doc.SelectNodes("/Project/ItemGroup/ProjectReference[@Include]");

                foreach (XmlNode oldNode in nodes)
                {
                    var oldNodeIncludeValue = oldNode.Attributes["Include"].Value;

                    // ReSharper disable once PossibleNullReferenceException : Can not be null because nodes are selected with include attribute filter in previous method
                    if (oldNodeIncludeValue.Contains($"{_companyAndProjectNamePlaceHolder}"))
                    {
                        continue;
                    }

                    XmlNode newNode = null;

                    newNode = _localReferences ?
                        GetLocalReferenceNode(doc, oldNodeIncludeValue) :
                        GetNugetReferenceNode(doc, oldNodeIncludeValue);

                    oldNode.ParentNode.ReplaceChild(newNode, oldNode);
                }

                return doc.OuterXml;
            }

            private XmlElement GetNugetReferenceNode(XmlDocument doc, string oldNodeIncludeValue)
            {
                var newNode = doc.CreateElement("PackageReference");

                var includeAttr = doc.CreateAttribute("Include");
                includeAttr.Value = ConvertToNugetReference(oldNodeIncludeValue);
                newNode.Attributes.Append(includeAttr);

                var versionAttr = doc.CreateAttribute("Version");
                versionAttr.Value = _nugetPackageVersion;
                newNode.Attributes.Append(versionAttr);
                return newNode;
            }

            private XmlElement GetLocalReferenceNode(XmlDocument doc, string oldNodeIncludeValue)
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

                includeValue = _gitHubLocalRepositoryPath.EnsureEndsWith('\\') + includeValue;

                return includeValue;
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

            private static Stream GenerateStreamFromString(string s)
            {
                var stream = new MemoryStream();
                var writer = new StreamWriter(stream);
                writer.Write(s);
                writer.Flush();
                stream.Position = 0;
                return stream;
            }
        }
    }
}
