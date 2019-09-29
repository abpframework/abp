﻿using System;
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
                nugetPackageVersion
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

            public NugetReferenceReplacer(
                List<FileEntry> entries, 
                string companyAndProjectNamePlaceHolder, 
                string nugetPackageVersion)
            {
                _entries = entries;
                _companyAndProjectNamePlaceHolder = companyAndProjectNamePlaceHolder;
                _nugetPackageVersion = nugetPackageVersion;
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

                    var newNode = doc.CreateElement("PackageReference");

                    var includeAttr = doc.CreateAttribute("Include");
                    includeAttr.Value = ConvertToNugetReference(oldNodeIncludeValue);
                    newNode.Attributes.Append(includeAttr);

                    var versionAttr = doc.CreateAttribute("Version");
                    versionAttr.Value = _nugetPackageVersion;
                    newNode.Attributes.Append(versionAttr);

                    oldNode.ParentNode.ReplaceChild(newNode, oldNode);
                }

                return doc.OuterXml;
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
