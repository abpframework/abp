using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps
{
    public class NugetReferenceReplaceStep : ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            new NugetReferenceReplacer(
                context.Files,
                "MyCompanyName",
                "MyProjectName",
                context.TemplateFile.Version
            ).Run();
        }

        private class NugetReferenceReplacer
        {
            private readonly List<FileEntry> _entries;
            private readonly string _companyNamePlaceHolder;
            private readonly string _projectNamePlaceHolder;
            private readonly string _latestNugetPackageVersion;

            public NugetReferenceReplacer(List<FileEntry> entries, string companyNamePlaceHolder, string projectNamePlaceHolder, string latestNugetPackageVersion)
            {
                _entries = entries;
                _companyNamePlaceHolder = companyNamePlaceHolder;
                _projectNamePlaceHolder = projectNamePlaceHolder;
                _latestNugetPackageVersion = latestNugetPackageVersion;
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

                var doc = new HtmlDocument();

                doc.Load(GenerateStreamFromString(content));

                var nodes = doc.DocumentNode.SelectNodes("//projectreference[@include]");

                if (nodes == null)
                {
                    return content;
                }

                return ProcessReferenceNodes(nodes, content);
            }

            private string ProcessReferenceNodes(HtmlNodeCollection nodes, string content)
            {
                Check.NotNull(nodes, nameof(nodes));
                Check.NotNull(content, nameof(content));

                foreach (var node in nodes)
                {
                    var valueAttr = node.Attributes.FirstOrDefault(a => a.Name.ToLower() == "include");

                    // ReSharper disable once PossibleNullReferenceException : Can not be null because nodes are selected with include attribute filter in previous method
                    if (valueAttr.Value.Contains($"{_companyNamePlaceHolder}.{_projectNamePlaceHolder}"))
                    {
                        continue;
                    }

                    var newValue = ConvertToNugetReference(valueAttr.Value);

                    var oldLine = $"<ProjectReference Include=\"{valueAttr.Value}\"";
                    var oldLineAlt = $"<ProjectReference  Include=\"{valueAttr.Value}\"";
                    var newLine = $"<PackageReference Include=\"{newValue}\" Version=\"{_latestNugetPackageVersion}\"";

                    content = content.Replace(oldLine, newLine);
                    content = content.Replace(oldLineAlt, newLine);
                }

                return content;
            }

            private string ConvertToNugetReference(string oldValue)
            {
                var newValue = Regex.Match(oldValue, @"\\((?!.+?\\).+?)\.csproj");
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
