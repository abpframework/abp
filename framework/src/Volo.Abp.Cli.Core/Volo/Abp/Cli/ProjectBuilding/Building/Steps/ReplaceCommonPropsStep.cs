using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps
{
    public class ReplaceCommonPropsStep : ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            new CommonPropsReplacer(context.Files).Run();
        }

        private class CommonPropsReplacer
        {
            private readonly List<FileEntry> _entries;

            public CommonPropsReplacer(
                List<FileEntry> entries)
            {
                _entries = entries;
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

                var importNodes = doc.SelectNodes("/Project/Import[@Project]");

                if (importNodes == null)
                {
                    return doc.OuterXml;
                }

                foreach (XmlNode node in importNodes)
                {
                    if (!(node.Attributes?["Project"]?.Value?.EndsWith("\\common.props") ?? false))
                    {
                        continue;
                    }

                    node.ParentNode?.RemoveChild(node);
                }

                var propertyGroupNodes = doc.SelectNodes("/Project/PropertyGroup");

                if (propertyGroupNodes == null || propertyGroupNodes.Count < 1)
                {
                    return doc.OuterXml;
                }

                var firstPropertyGroupNode = propertyGroupNodes.Item(0);
                var langNode = doc.CreateElement("LangVersion");
                langNode.InnerText = "latest";
                firstPropertyGroupNode?.PrependChild(langNode);

                return doc.OuterXml;
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
