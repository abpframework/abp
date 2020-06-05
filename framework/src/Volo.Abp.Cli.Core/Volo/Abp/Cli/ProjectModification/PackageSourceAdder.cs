using System;
using System.IO;
using System.Xml;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification
{
    public class PackageSourceAdder: ITransientDependency
    {
        public ILogger<PackageSourceAdder> Logger { get; set; }

        public PackageSourceAdder()
        {
            Logger = NullLogger<PackageSourceAdder>.Instance;
        }

        public void Add(string sourceKey, string sourceValue)
        {
            var nugetConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "NuGet", "NuGet.Config");

            if (!File.Exists(nugetConfigPath))
            {
                return;
            }

            var fileContent = File.ReadAllText(nugetConfigPath);

            if (fileContent.Contains($"\"{sourceValue}\""))
            {
                return;
            }

            Logger.LogInformation($"Adding \"{sourceValue}\" ({sourceKey}) to nuget sources...");

            try
            {
                var doc = new XmlDocument() { PreserveWhitespace = true };

                doc.Load(GenerateStreamFromString(fileContent));

                var sourceNodes = doc.SelectNodes("/configuration/packageSources");

                var newNode = doc.CreateElement("add");

                var includeAttr = doc.CreateAttribute("key");
                includeAttr.Value = sourceKey;
                newNode.Attributes.Append(includeAttr);

                var versionAttr = doc.CreateAttribute("value");
                versionAttr.Value = sourceValue;
                newNode.Attributes.Append(versionAttr);

                sourceNodes?[0]?.AppendChild(newNode);

                File.WriteAllText(nugetConfigPath, doc.OuterXml);
            }
            catch
            {
                Logger.LogWarning($"Adding \"{sourceValue}\" ({sourceKey}) to nuget sources FAILED.");
            }
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
