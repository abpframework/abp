using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using Volo.Abp;
using Volo.Utils.SolutionTemplating.Files;

namespace Volo.Utils.SolutionTemplating.Building.Steps
{
    public class NugetReferenceReplaceStep : ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            new NugetReferenceReplacer(
                context.Files,
                "MyCompanyName",
                "MyProjectName"
            ).Run();
        }

        private class NugetReferenceReplacer
        {
            private readonly List<FileEntry> _entries;
            private readonly string _companyNamePlaceHolder;
            private readonly string _projectNamePlaceHolder;
            private readonly string _latestNugetPackageVersion;

            public NugetReferenceReplacer(List<FileEntry> entries, string companyNamePlaceHolder, string projectNamePlaceHolder)
            {
                _entries = entries;
                _companyNamePlaceHolder = companyNamePlaceHolder;
                _projectNamePlaceHolder = projectNamePlaceHolder;
                _latestNugetPackageVersion = GetLatestNugetPackageVersion();
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

                return ProcessReferenceNodes(doc.DocumentNode.SelectNodes("//projectreference[@include]"), content);
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
                var directory = new DirectoryInfo(oldValue);

                var newValue = directory.Name.Replace(".csproj", "");

                return newValue;
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

            private string GetLatestNugetPackageVersion()
            {
                //TODO: This should get it from the related release! Not always from the master!

                var commonPropsUrl = "https://raw.githubusercontent.com/abpframework/abp/master/common.props";
                var content = "";
                using (var webClient = new WebClient())
                {
                    try
                    {
                        content = webClient.DownloadString(commonPropsUrl);
                    }
                    catch (Exception)
                    {
                        throw new Exception("The Common.pros doesn't exist on github or removed to anywhere else.");
                    }
                }

                var doc = new HtmlDocument();

                doc.Load(GenerateStreamFromString(content));

                try
                {
                    return doc.DocumentNode.SelectNodes("//version").FirstOrDefault().InnerHtml.Trim();
                }
                catch (Exception e)
                {
                    return "";
                }
            }
        }
    }
}
