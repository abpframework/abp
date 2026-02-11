using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using NuGet.Versioning;
using NUglify.Helpers;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification;

public class SolutionPackageVersionFinder : ITransientDependency
{
    public string FindByDllVersion(string solutionFile, string dllName = "Volo.Abp*")
    {
        var projectFilesUnderSrc = GetProjectFilesOfSolution(solutionFile);
        foreach (var projectFile in projectFilesUnderSrc)
        {
            var dllFiles = Directory.GetFiles(Path.GetDirectoryName(projectFile)!, $"{dllName}.dll", SearchOption.AllDirectories);

            if (dllFiles.Any())
            {
                var version =  FileVersionInfo.GetVersionInfo(dllFiles.First());

                return $"{version.FileMajorPart}.{version.FileMinorPart}.{version.FileBuildPart}";
            }
        }

        return null;
    }
    
    public string FindByCsprojVersion(string solutionFile, string packagePrefix = "Volo.Abp", string excludedKeywords = "LeptonX", string includedKeyword = null)
    {
        var projectFilesUnderSrc = GetProjectFilesOfSolution(solutionFile);
        foreach (var projectFile in projectFilesUnderSrc)
        {
            var content = File.ReadAllText(projectFile);
            if (TryParseVersionFromCsprojViaXmlDocument(content, out var s, packagePrefix, excludedKeywords, includedKeyword))
            {
                return s;
            }
        }

        return null;
    }

    private static bool TryParseVersionFromCsprojViaXmlDocument(string content, out string version, string packagePrefix, string excludedKeywords, string includedKeyword)
    {
        var doc = new XmlDocument() { PreserveWhitespace = true };
        using (var stream = StreamHelper.GenerateStreamFromString(content))
        {
            doc.Load(stream);
            var nodes = doc.SelectNodes($"/Project/ItemGroup/PackageReference[starts-with(@Include, '{packagePrefix}')]");

            var targetNodes = new List<XmlNode>();

            foreach (XmlNode node in nodes!)
            {
                var packageId = node!.Attributes["Include"]?.Value;

                if (!excludedKeywords.IsNullOrWhiteSpace() && excludedKeywords.Split(',').Any(ek => packageId!.Contains(ek)))
                {
                    continue;
                }

                if (!includedKeyword.IsNullOrWhiteSpace() && !packageId!.Contains(includedKeyword))
                {
                    continue;
                }

                targetNodes.Add(node);
            }

            if (!targetNodes.Any())
            {
                version = null;
                return false;
            }
            
            var value = targetNodes.First().Attributes?["Version"]?.Value;
            if (value == null)
            {
                version = null;
                return false;
            }

            version = value;
            return true;
        }
    }

    public static bool TryParseVersionFromCsprojFile(string csprojContent, out string version, string packagePrefix = "Volo.Abp", string excludedKeywords = "LeptonX", string includedKeywords = null)
    {
        return TryParseVersionFromCsprojViaXmlDocument(csprojContent, out version, packagePrefix, excludedKeywords, includedKeywords);
    }


    public static bool TryParseSemanticVersionFromCsprojFile(string csprojContent, out SemanticVersion version, string packagePrefix = "Volo.Abp", string excludedKeywords = "LeptonX", string includedKeywords = null)
    {
        try
        {
            if (TryParseVersionFromCsprojViaXmlDocument(csprojContent, out var versionText, packagePrefix, excludedKeywords, includedKeywords))
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
