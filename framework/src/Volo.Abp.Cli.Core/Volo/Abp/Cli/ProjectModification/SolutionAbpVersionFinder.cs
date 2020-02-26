using System.IO;
using System.Xml;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification
{
    public class SolutionAbpVersionFinder : ITransientDependency
    {
        public string Find(string solutionFile)
        {
            var projectFilesUnderSrc = Directory.GetFiles(Path.GetDirectoryName(solutionFile),
                "*.csproj",
                SearchOption.AllDirectories);

            foreach (var projectFile in projectFilesUnderSrc)
            {
                var content = File.ReadAllText(projectFile);
                var doc = new XmlDocument() { PreserveWhitespace = true };

                doc.Load(StreamHelper.GenerateStreamFromString(content));

                var nodes = doc.SelectNodes("/Project/ItemGroup/PackageReference[starts-with(@Include, 'Volo.Abp')]");

                var value = nodes?[0]?.Attributes?["Version"]?.Value;

                if (value != null)
                {
                    return value;
                }
            }

            return null;
        }
    }
}
