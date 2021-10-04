using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using NuGet.Versioning;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification
{
    public class SolutionAbpVersionFinder : ITransientDependency
    {
        public string Find(string solutionFile)
        {
            var projectFilesUnderSrc = GetProjectFilesOfSolution(solutionFile);
            foreach (var projectFile in projectFilesUnderSrc)
            {
                var content = File.ReadAllText(projectFile);
                if (TryParseVersionFromCsprojViaXmlDocument(content, out var s))
                {
                    return s;
                }
            }

            return null;
        }

        private static bool TryParseVersionFromCsprojViaXmlDocument(string content, out string version)
        {
            var doc = new XmlDocument() { PreserveWhitespace = true };
            using (var stream = StreamHelper.GenerateStreamFromString(content))
            {
                doc.Load(stream);
                var nodes = doc.SelectNodes("/Project/ItemGroup/PackageReference[starts-with(@Include, 'Volo.Abp')]");
                var value = nodes?[0]?.Attributes?["Version"]?.Value;
                if (value == null)
                {
                    version = null;
                    return false;
                }

                version = value;
                return true;
            }
        }

        public static bool TryParseVersionFromCsprojFile(string csprojContent, out string version)
        {
            try
            {
                var matches = Regex.Matches(csprojContent,
                      @"PackageReference\s*Include\s*=\s*\""Volo.Abp(.*?)\""\s*Version\s*=\s*\""(.*?)\""",
                      RegexOptions.IgnoreCase |
                      RegexOptions.IgnorePatternWhitespace |
                      RegexOptions.Singleline | RegexOptions.Multiline);

                foreach (Match match in matches)
                {
                    if (match.Groups.Count > 2)
                    {
                        version = match.Groups[2].Value;
                        return true;
                    }
                }
            }
            catch
            {
                //ignored
            }

            version = null;
            return false;
        }


        public static bool TryParseSemanticVersionFromCsprojFile(string csprojContent, out SemanticVersion version)
        {
            try
            {
                if (TryParseVersionFromCsprojFile(csprojContent, out var versionText))
                {
                    return SemanticVersion.TryParse(versionText, out version);
                }
            }
            catch
            {
                //ignored
            }

            version = null;
            return false;
        }

        public static bool TryFind(string solutionFile, out string version)
        {
            var projectFiles = GetProjectFilesOfSolution(solutionFile);
            foreach (var projectFile in projectFiles)
            {
                var csprojContent = File.ReadAllText(projectFile);
                if (TryParseVersionFromCsprojFile(csprojContent, out var parsedVersion))
                {
                    version = parsedVersion;
                    return true;
                }
            }

            version = null;
            return false;
        }

        public static bool TryFindSemanticVersion(string solutionFile, out SemanticVersion version)
        {
            var projectFiles = GetProjectFilesOfSolution(solutionFile);
            foreach (var projectFile in projectFiles)
            {
                var csprojContent = File.ReadAllText(projectFile);
                if (TryParseSemanticVersionFromCsprojFile(csprojContent, out var parsedVersion))
                {
                    version = parsedVersion;
                    return true;
                }
            }

            version = null;
            return false;
        }

        //public static bool TryFindSemanticVersion(string solutionFile, out SemanticVersion version)
        //{
        //    if (TryFind(solutionFile, out var versionText))
        //    {
        //        return SemanticVersion.TryParse(versionText, out version);
        //    }

        //    version = null;
        //    return false;
        //}

        private static string[] GetProjectFilesOfSolution(string solutionFile)
        {
            var solutionDirectory = Path.GetDirectoryName(solutionFile);
            if (solutionDirectory == null)
            {
                return new string[] { };
            }

            return Directory.GetFiles(solutionDirectory, "*.csproj", SearchOption.AllDirectories);
        }
    }
}
