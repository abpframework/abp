using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification;

public class NugetPackageToLocalReferenceConverter : ITransientDependency
{
    public ILogger<NugetPackageToLocalReferenceConverter> Logger { get; set; }

    public NugetPackageToLocalReferenceConverter(){
        Logger = NullLogger<NugetPackageToLocalReferenceConverter>.Instance;
    }
    
    public async Task Convert(ModuleWithMastersInfo module, string solutionFile, string modulePrefix = "Volo.")
    {
        var nugetPackageList = GetNugetPackages(module);
        var modulesFolder = Path.Combine(Path.GetDirectoryName(solutionFile), "modules");
        var srcFolder = Path.Combine(Path.GetDirectoryName(solutionFile), "src");
        var testFolder = Path.Combine(Path.GetDirectoryName(solutionFile), "test");

        ConvertToLocalReference(modulesFolder, nugetPackageList, "..\\..\\..\\", "src", modulePrefix);
        ConvertToLocalReference(srcFolder, nugetPackageList, "..\\..\\modules\\", "src", modulePrefix);
        ConvertToLocalReference(testFolder, nugetPackageList, "..\\..\\modules\\", "test", modulePrefix);
    }

    private void ConvertToLocalReference(string folder, List<NugetPackageInfoWithModuleName> nugetPackageList, string localPathPrefix, string sourceFile, string modulePrefix)
    {
        var projectFiles = GetProjectFilesUnder(folder);

        foreach (var projectFile in projectFiles)
        {
            if (!File.Exists(projectFile))
            {
                Logger.LogWarning($"{projectFile} could not be found, skipping...");
                return;
            }
            
            var content = File.ReadAllText(projectFile);
            using (var stream = StreamHelper.GenerateStreamFromString(content))
            {
                var doc = new XmlDocument() { PreserveWhitespace = true };
                doc.Load(stream);
                var convertedProject = ProcessReferenceNodes(folder, doc, nugetPackageList, localPathPrefix, sourceFile, modulePrefix);
                File.WriteAllText(projectFile, convertedProject);
            }
        }
    }

    private string ProcessReferenceNodes(string folder, XmlDocument doc, List<NugetPackageInfoWithModuleName> nugetPackageList, string localPathPrefix, string sourceFile, string modulePrefix)
    {
        var nodes = doc.SelectNodes($"/Project/ItemGroup/PackageReference[starts-with(@Include, '{modulePrefix}')]");

        if (nodes == null)
        {
            return doc.OuterXml;
        }

        foreach (XmlNode oldNode in nodes)
        {
            var tempSourceFile = sourceFile;
            var oldNodeIncludeValue = oldNode?.Attributes?["Include"]?.Value;

            var moduleName = nugetPackageList.FirstOrDefault(n => n.NugetPackage.Name == oldNodeIncludeValue)?.ModuleName;

            if (moduleName == null)
            {
                var localProject = GetProjectFilesUnder(folder).FirstOrDefault(f => f.EndsWith($"{oldNodeIncludeValue}.csproj"));

                if (localProject != null)
                {
                    moduleName = Directory.GetParent(Directory.GetParent(Path.GetDirectoryName(localProject)).FullName).Name;

                    if (oldNodeIncludeValue.EndsWith(".test", StringComparison.InvariantCultureIgnoreCase) ||
                        oldNodeIncludeValue.EndsWith(".tests", StringComparison.InvariantCultureIgnoreCase) ||
                        oldNodeIncludeValue.EndsWith(".testbase", StringComparison.InvariantCultureIgnoreCase) ||
                        oldNodeIncludeValue.EndsWith(".Demo", StringComparison.InvariantCultureIgnoreCase))
                    {
                        tempSourceFile = "test";
                    }
                    else
                    {
                        tempSourceFile = "src";
                    }
                }
                else
                {
                    continue;
                }
            }

            var referenceProjectPath =
                $"{localPathPrefix}{moduleName}\\{tempSourceFile}\\{oldNodeIncludeValue}\\{oldNodeIncludeValue}.csproj";

            XmlNode newNode = GetNewReferenceNode(doc, referenceProjectPath);

            oldNode?.ParentNode?.ReplaceChild(newNode, oldNode);
        }

        return doc.OuterXml;
    }

    protected XmlElement GetNewReferenceNode(XmlDocument doc, string newValue)
    {
        var newNode = doc.CreateElement("ProjectReference");

        var includeAttr = doc.CreateAttribute("Include");
        includeAttr.Value = newValue;
        newNode.Attributes.Append(includeAttr);

        return newNode;
    }

    public List<NugetPackageInfoWithModuleName> GetNugetPackages(ModuleWithMastersInfo module)
    {
        var list = new List<NugetPackageInfoWithModuleName>();

        list.AddRange(module.NugetPackages.Select(n => new NugetPackageInfoWithModuleName
        {
            ModuleName = module.Name,
            NugetPackage = n
        }));

        if (module.MasterModuleInfos != null)
        {
            foreach (var masterModule in module.MasterModuleInfos)
            {
                list.AddRange(GetNugetPackages(masterModule));
            }
        }

        return list;
    }

    private static string[] GetProjectFilesUnder(string path)
    {
        return Directory.GetFiles(path,
            "*.csproj",
            SearchOption.AllDirectories);
    }

    public class NugetPackageInfoWithModuleName
    {
        public NugetPackageInfo NugetPackage { get; set; }

        public string ModuleName { get; set; }
    }
}
