using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.NuGet;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification
{
    public class PackageSourceSwitcher : ITransientDependency
    {
        private readonly MyGetPackageListFinder _myGetPackageListFinder;
        private readonly NuGetService _nuGetService;
        private readonly PackageSourceAdder _packageSourceAdder;

        public ILogger<PackageSourceSwitcher> Logger { get; set; }

        public PackageSourceSwitcher(MyGetPackageListFinder myGetPackageListFinder, NuGetService nuGetService, PackageSourceAdder packageSourceAdder)
        {
            _myGetPackageListFinder = myGetPackageListFinder;
            _nuGetService = nuGetService;
            _packageSourceAdder = packageSourceAdder;
            Logger = NullLogger<PackageSourceSwitcher>.Instance;
        }

        public async Task SwitchToPreview(CommandLineArgs commandLineArgs)
        {
            _packageSourceAdder.Add("ABP Nightly", "https://www.myget.org/F/abp-nightly/api/v3/index.json");

            await Switch(commandLineArgs, SwitchTarget.Preview);
        }

        public async Task SwitchToStable(CommandLineArgs commandLineArgs)
        {
            await Switch(commandLineArgs, SwitchTarget.Stable);
        }

        private async Task Switch(CommandLineArgs commandLineArgs, SwitchTarget target)
        {
            var solutionPath = GetSolutionPath(commandLineArgs);
            var latestStableAbpVersion = target == SwitchTarget.Stable ? (await _nuGetService.GetLatestVersionOrNullAsync("Volo.Abp")).ToString() : null;

            Logger.LogInformation($"Packages on solution \"{Path.GetFileName(solutionPath)}\" are being switched to {target}.");
            Logger.LogInformation("");

            var projectFiles = GetCsprojFiles(solutionPath);

            foreach (var projectFile in projectFiles)
            {
                Logger.LogInformation($"Packages on project \"{Path.GetFileName(projectFile)}\" are being switched to {target}.");

                var content = File.ReadAllText(projectFile);

                var doc = new XmlDocument() { PreserveWhitespace = true };

                doc.Load(GenerateStreamFromString(content));

                doc = await SwitchPackagesInDocument(doc, content, target, latestStableAbpVersion);

                File.WriteAllText(projectFile, doc.OuterXml);
            }

            Logger.LogInformation("");
            Logger.LogInformation($"Packages on solution \"{Path.GetFileName(solutionPath)}\" are switched to {target}.");
        }

        private async Task<XmlDocument> SwitchPackagesInDocument(XmlDocument doc, string content, SwitchTarget target, string latestStableAbpVersion = null)
        {
            Check.NotNull(content, nameof(content));

            var nodes = doc.SelectNodes("/Project/ItemGroup/PackageReference[@Include]");

            foreach (XmlNode node in nodes)
            {
                var packageName = node.Attributes["Include"].Value;

                if (!packageName.StartsWith("Volo.Abp"))
                {
                    continue;
                }

                var version = node.Attributes["Version"].Value;

                XmlNode newNode = null;

                if (target == SwitchTarget.Stable)
                {
                    newNode = GetNewStableReferenceNode(doc, packageName, version, latestStableAbpVersion);
                }
                else if (target == SwitchTarget.Preview)
                {
                    var packageList = (await _myGetPackageListFinder.GetPackages()).Packages;
                    newNode = GetNewPreviewReferenceNode(doc, packageName, version, packageList);
                }

                node.ParentNode?.ReplaceChild(newNode, node);
            }

            return doc;
        }

        private XmlElement GetNewStableReferenceNode(XmlDocument doc, string packageName, string version, string newVersion)
        {
            var newNode = doc.CreateElement("PackageReference");

            var includeAttr = doc.CreateAttribute("Include");
            includeAttr.Value = packageName;
            newNode.Attributes.Append(includeAttr);

            var versionAttr = doc.CreateAttribute("Version");
            versionAttr.Value = newVersion;
            newNode.Attributes.Append(versionAttr);
            return newNode;
        }

        private XmlElement GetNewPreviewReferenceNode(XmlDocument doc, string packageName, string version, List<MyGetPackage> packageList)
        {
            var newVersion = packageList.FirstOrDefault(p => p.Id == packageName)?.Versions.LastOrDefault() ?? version;

            var newNode = doc.CreateElement("PackageReference");

            var includeAttr = doc.CreateAttribute("Include");
            includeAttr.Value = packageName;
            newNode.Attributes.Append(includeAttr);

            var versionAttr = doc.CreateAttribute("Version");
            versionAttr.Value = newVersion;
            newNode.Attributes.Append(versionAttr);
            return newNode;
        }

        private List<string> GetCsprojFiles(string slnPath)
        {
            return Directory.GetFiles(Path.GetDirectoryName(slnPath), "*.csproj", SearchOption.AllDirectories).ToList();
        }

        private string GetSolutionPath(CommandLineArgs commandLineArgs)
        {
            var solutionPath = commandLineArgs.Options.GetOrNull(Options.SolutionPath.Short, Options.SolutionPath.Long);

            if (solutionPath == null)
            {
                try
                {
                    solutionPath = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.sln").Single();
                }
                catch (Exception)
                {
                    Logger.LogError("There is no solution or more that one solution in current directory.");
                    throw;
                }
            }

            return solutionPath;
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

        public static class Options
        {
            public static class SolutionPath
            {
                public const string Short = "sp";
                public const string Long = "solution-path";
            }
        }

        enum SwitchTarget
        {
            Preview,
            Stable
        }
    }
}
