using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Commands;
using Volo.Abp.Cli.Commands.Services;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IO;
using Volo.Abp.Json;

namespace Volo.Abp.Cli.ProjectModification
{
    public class ProjectNugetPackageAdder : ITransientDependency
    {
        public ILogger<ProjectNugetPackageAdder> Logger { get; set; }
        public BundleCommand BundleCommand { get; }
        public SourceCodeDownloadService SourceCodeDownloadService { get; }
        public SolutionFileModifier SolutionFileModifier { get; }

        protected IJsonSerializer JsonSerializer { get; }
        protected ProjectNpmPackageAdder NpmPackageAdder { get; }
        protected DerivedClassFinder ModuleClassFinder { get; }
        protected ModuleClassDependcyAdder ModuleClassDependcyAdder { get; }
        protected IRemoteServiceExceptionHandler RemoteServiceExceptionHandler { get; }

        private readonly CliHttpClientFactory _cliHttpClientFactory;

        public ProjectNugetPackageAdder(
            IJsonSerializer jsonSerializer,
            ProjectNpmPackageAdder npmPackageAdder,
            DerivedClassFinder moduleClassFinder,
            ModuleClassDependcyAdder moduleClassDependcyAdder,
            IRemoteServiceExceptionHandler remoteServiceExceptionHandler,
            BundleCommand bundleCommand,
            CliHttpClientFactory cliHttpClientFactory,
            SourceCodeDownloadService sourceCodeDownloadService,
            SolutionFileModifier solutionFileModifier)
        {
            JsonSerializer = jsonSerializer;
            NpmPackageAdder = npmPackageAdder;
            ModuleClassFinder = moduleClassFinder;
            ModuleClassDependcyAdder = moduleClassDependcyAdder;
            RemoteServiceExceptionHandler = remoteServiceExceptionHandler;
            BundleCommand = bundleCommand;
            SourceCodeDownloadService = sourceCodeDownloadService;
            SolutionFileModifier = solutionFileModifier;
            _cliHttpClientFactory = cliHttpClientFactory;
            Logger = NullLogger<ProjectNugetPackageAdder>.Instance;
        }

        public async Task AddAsync(
            string projectFile,
            string packageName,
            string version = null,
            bool useDotnetCliToInstall = true,
            bool withSourceCode = false,
            bool addSourceCodeToSolutionFile = false)
        {
            await AddAsync(
                projectFile,
                await FindNugetPackageInfoAsync(packageName),
                version,
                useDotnetCliToInstall,
                withSourceCode,
                addSourceCodeToSolutionFile
            );
        }

        public async Task AddAsync(
            string projectFile,
            NugetPackageInfo package,
            string version = null,
            bool useDotnetCliToInstall = true,
            bool withSourceCode = false,
            bool addSourceCodeToSolutionFile = false)
        {
            if (version == null)
            {
                version = GetAbpVersionOrNull(projectFile);
            }

            await AddAsPackageReference(projectFile, package, version, useDotnetCliToInstall);

            if (withSourceCode)
            {
                await AddSourceCode(projectFile, package, version);
                await ConvertPackageReferenceToProjectReference(projectFile, package);

                if (addSourceCodeToSolutionFile)
                {
                    await SolutionFileModifier.AddPackageToSolutionFileAsync(package, FindSolutionFile(projectFile));
                }
            }
        }

        private async Task ConvertPackageReferenceToProjectReference(string projectFile, NugetPackageInfo package)
        {
            var content = File.ReadAllText(projectFile);
            var doc = new XmlDocument() {PreserveWhitespace = true};

            doc.Load(StreamHelper.GenerateStreamFromString(content));

            var nodes = doc.SelectNodes(
                $"/Project/ItemGroup/PackageReference[starts-with(@Include, '{package.Name}')]");

            if (nodes == null)
            {
                return;
            }

            var downloadedProjectPath = FindRelativeFolderToDownloadPackage(projectFile, package);
            var oldNodeIncludeValue = nodes[0]?.Attributes?["Include"]?.Value;

            if (package.Name == oldNodeIncludeValue)
            {
                var referenceProjectPath = $"{downloadedProjectPath}\\{package.Name}.csproj";

                var newNode = doc.CreateElement("ProjectReference");
                var includeAttr = doc.CreateAttribute("Include");
                includeAttr.Value = referenceProjectPath;
                newNode.Attributes.Append(includeAttr);

                nodes[0]?.ParentNode?.ReplaceChild(newNode, nodes[0]);
            }

            File.WriteAllText(projectFile, doc.OuterXml);
        }

        private async Task AddSourceCode(string projectFile, NugetPackageInfo package, string version = null)
        {
            var targetFolder = FindFolderToDownloadPackage(projectFile, package);

            if (Directory.Exists(targetFolder))
            {
                Directory.Delete(targetFolder, true);
            }

            await DownloadSourceCode(targetFolder, package, version);
        }

        private string FindFolderToDownloadPackage(string projectFile, NugetPackageInfo package)
        {
            return Path.Combine(FindSolutionFolder(projectFile), "packages", package.Name);
        }

        private string FindRelativeFolderToDownloadPackage(string projectFile, NugetPackageInfo package)
        {
            var folder =  Path.Combine(FindSolutionFolder(projectFile), "packages", package.Name);

            return new Uri(projectFile).MakeRelativeUri(new Uri(folder)).ToString().Replace("/", "\\");
        }

        private async Task DownloadSourceCode(string targetFolder, NugetPackageInfo package, string version = null)
        {
            await SourceCodeDownloadService.DownloadPackageAsync(
                package.Name,
                targetFolder,
                version
            );
        }

        private string FindSolutionFile(string projectFile)
        {
            var folder = FindSolutionFolder(projectFile);

            return Directory.GetFiles(folder, "*.sln", SearchOption.TopDirectoryOnly).FirstOrDefault();
        }

        private string FindSolutionFolder(string projectFile)
        {
            var targetFolder = Path.GetDirectoryName(projectFile);

            do
            {
                if (Directory.GetParent(targetFolder) != null)
                {
                    targetFolder = Directory.GetParent(targetFolder).FullName;
                }
                else
                {
                    return Path.GetDirectoryName(projectFile);
                }

                if (Directory.GetFiles(targetFolder, "*.sln", SearchOption.TopDirectoryOnly).Any())
                {
                    break;
                }
            } while (targetFolder != null);

            return targetFolder;
        }

        private async Task AddAsPackageReference(string projectFile, NugetPackageInfo package, string version,
            bool useDotnetCliToInstall)
        {
            var projectFileContent = File.ReadAllText(projectFile);

            if (projectFileContent.Contains($"\"{package.Name}\""))
            {
                return;
            }

            using (DirectoryHelper.ChangeCurrentDirectory(Path.GetDirectoryName(projectFile)))
            {
                Logger.LogInformation(
                    $"Installing '{package.Name}' package to the project '{Path.GetFileNameWithoutExtension(projectFile)}'...");

                if (useDotnetCliToInstall)
                {
                    AddUsingDotnetCli(package, version);
                }
                else
                {
                    AddToCsprojManuallyAsync(projectFile, package, version);
                }

                var moduleFiles = ModuleClassFinder.Find(projectFile, "AbpModule");
                if (moduleFiles.Count == 0)
                {
                    throw new CliUsageException(
                        $"Could not find a class derived from AbpModule in the project {projectFile}");
                }

                if (moduleFiles.Count > 1)
                {
                    throw new CliUsageException(
                        $"There are multiple classes derived from AbpModule in the project {projectFile}: " +
                        moduleFiles.JoinAsString(", "));
                }

                ModuleClassDependcyAdder.Add(moduleFiles.First(), package.ModuleClass);
            }

            if (useDotnetCliToInstall && (package.Target == NuGetPackageTarget.Blazor ||
                                          package.Target == NuGetPackageTarget.BlazorServer ||
                                          package.Target == NuGetPackageTarget.BlazorWebAssembly))
            {
                await RunBundleForBlazorAsync(projectFile);
            }

            Logger.LogInformation("Successfully installed.");
        }

        private Task AddUsingDotnetCli(NugetPackageInfo package, string version = null)
        {
            var versionOption = version == null ? "" : $" -v {version}";

            CmdHelper.Run("dotnet", $"add package {package.Name}{versionOption}");

            return Task.CompletedTask;
        }

        private Task AddToCsprojManuallyAsync(string projectFile, NugetPackageInfo package, string version = null)
        {
            var projectFileContent = File.ReadAllText(projectFile);
            var doc = new XmlDocument() {PreserveWhitespace = true};
            doc.Load(StreamHelper.GenerateStreamFromString(projectFileContent));

            var itemGroupNodes = doc.SelectNodes("/Project/ItemGroup");
            XmlNode itemGroupNode = null;

            if (itemGroupNodes == null || itemGroupNodes.Count < 1)
            {
                var projectNodes = doc.SelectNodes("/Project");
                var projectNode = projectNodes[0];

                itemGroupNode = doc.CreateElement("ItemGroup");
                projectNode.AppendChild(itemGroupNode);
            }
            else
            {
                itemGroupNode = itemGroupNodes[0];
            }

            var packageReferenceNode = doc.CreateElement("PackageReference");

            var includeAttr = doc.CreateAttribute("Include");
            includeAttr.Value = package.Name;
            packageReferenceNode.Attributes.Append(includeAttr);

            if (version != null)
            {
                var versionAttr = doc.CreateAttribute("Version");
                versionAttr.Value = version;
                packageReferenceNode.Attributes.Append(versionAttr);
            }

            itemGroupNode.AppendChild(packageReferenceNode);

            File.WriteAllText(projectFile, doc.OuterXml);

            return Task.CompletedTask;
        }

        private string GetAbpVersionOrNull(string projectFile)
        {
            var projectFileContent = File.ReadAllText(projectFile);

            var doc = new XmlDocument() {PreserveWhitespace = true};

            doc.Load(StreamHelper.GenerateStreamFromString(projectFileContent));

            var nodes = doc.SelectNodes("/Project/ItemGroup/PackageReference[starts-with(@Include, 'Volo.')]");

            return nodes?[0]?.Attributes?["Version"]?.Value;
        }

        protected virtual async Task<NugetPackageInfo> FindNugetPackageInfoAsync(string packageName)
        {
            var url = $"{CliUrls.WwwAbpIo}api/app/nugetPackage/byName/?name=" + packageName;
            var client = _cliHttpClientFactory.CreateClient();

            using (var response = await client.GetAsync(url, _cliHttpClientFactory.GetCancellationToken()))
            {
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new CliUsageException($"'{packageName}' nuget package could not be found!");
                    }

                    await RemoteServiceExceptionHandler.EnsureSuccessfulHttpResponseAsync(response);
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<NugetPackageInfo>(responseContent);
            }
        }

        protected virtual async Task RunBundleForBlazorAsync(string projectFile)
        {
            var args = new CommandLineArgs("bundle");

            args.Options.Add(BundleCommand.Options.WorkingDirectory.Short, Path.GetDirectoryName(projectFile));
            args.Options.Add(BundleCommand.Options.ForceBuild.Short, string.Empty);

            await BundleCommand.ExecuteAsync(args);
        }
    }
}
