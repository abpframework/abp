using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification
{
    public class NugetPackageToLocalReferenceConverter : ITransientDependency
    {
        public async Task Convert(ModuleWithMastersInfo module, string solutionFile)
        {
            var nugetPackageList = GetNugetPackages(module);

            var projectFilesUnderModules = GetProjectFilesUnder(Path.Combine(Path.GetDirectoryName(solutionFile), "modules"));
            var projectFilesUnderSrc = GetProjectFilesUnder(Path.Combine(Path.GetDirectoryName(solutionFile), "src"));
            var projectFilesUnderTest = GetProjectFilesUnder(Path.Combine(Path.GetDirectoryName(solutionFile), "test"));

            foreach (var projectFile in projectFilesUnderModules)
            {
                ConvertToLocalReference(projectFile, nugetPackageList, "..\\..\\..\\");
            }
            foreach (var projectFile in projectFilesUnderSrc)
            {
                ConvertToLocalReference(projectFile, nugetPackageList, "..\\..\\modules\\");
            }
            foreach (var projectFile in projectFilesUnderTest)
            {
                ConvertToLocalReference(projectFile, nugetPackageList, "..\\..\\modules\\", "test");
            }
        }

        private void ConvertToLocalReference(string projectFile, List<NugetPackageInfoWithModuleName> nugetPackageList, string localPathPrefix, string sourceFile = "src")
        {
            var content = File.ReadAllText(projectFile);
            var doc = new XmlDocument() { PreserveWhitespace = true };

            doc.Load(StreamHelper.GenerateStreamFromString(content));

            var convertedProject =  ProcessReferenceNodes(doc, nugetPackageList, localPathPrefix, sourceFile);

            File.WriteAllText(projectFile, convertedProject);
        }


        private string ProcessReferenceNodes(XmlDocument doc, List<NugetPackageInfoWithModuleName> nugetPackageList, string localPathPrefix, string sourceFile = "src")
        {
            var nodes = doc.SelectNodes("/Project/ItemGroup/PackageReference[@Include]");

            if (nodes == null)
            {
                return doc.OuterXml;
            }

            foreach (XmlNode oldNode in nodes)
            {
                var oldNodeIncludeValue = oldNode?.Attributes?["Include"]?.Value;

                var nugetPackage = nugetPackageList.FirstOrDefault(n => n.NugetPackage.Name == oldNodeIncludeValue);

                if (nugetPackage == null)
                {
                    continue;
                }

                var referenceProjectPath =
                    $"{localPathPrefix}{nugetPackage.ModuleName}\\{sourceFile}\\{nugetPackage.NugetPackage.Name}\\{nugetPackage.NugetPackage.Name}.csproj";

                XmlNode newNode =  GetNewReferenceNode(doc, referenceProjectPath);

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
}
