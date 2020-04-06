using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Volo.Abp.Cli.Commands.Services;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace Volo.Abp.Cli.ProjectModification
{
    public class SolutionModuleAdder : ITransientDependency
    {
        public ILogger<SolutionModuleAdder> Logger { get; set; }

        protected IJsonSerializer JsonSerializer { get; }
        protected ProjectNugetPackageAdder ProjectNugetPackageAdder { get; }
        protected DbContextFileBuilderConfigureAdder DbContextFileBuilderConfigureAdder { get; }
        protected EfCoreMigrationAdder EfCoreMigrationAdder { get; }
        protected DerivedClassFinder DerivedClassFinder { get; }
        protected ProjectNpmPackageAdder ProjectNpmPackageAdder { get; }
        protected NpmGlobalPackagesChecker NpmGlobalPackagesChecker { get; }
        protected IRemoteServiceExceptionHandler RemoteServiceExceptionHandler { get; }
        public SourceCodeDownloadService SourceCodeDownloadService { get; }
        public SolutionFileModifier SolutionFileModifier { get; }
        public NugetPackageToLocalReferenceConverter NugetPackageToLocalReferenceConverter { get; }
        public AngularModuleSourceCodeAdder AngularModuleSourceCodeAdder { get; }

        public SolutionModuleAdder(
            IJsonSerializer jsonSerializer,
            ProjectNugetPackageAdder projectNugetPackageAdder,
            DbContextFileBuilderConfigureAdder dbContextFileBuilderConfigureAdder,
            EfCoreMigrationAdder efCoreMigrationAdder,
            DerivedClassFinder derivedClassFinder,
            ProjectNpmPackageAdder projectNpmPackageAdder,
            NpmGlobalPackagesChecker npmGlobalPackagesChecker, 
            IRemoteServiceExceptionHandler remoteServiceExceptionHandler,
            SourceCodeDownloadService sourceCodeDownloadService,
            SolutionFileModifier solutionFileModifier,
            NugetPackageToLocalReferenceConverter nugetPackageToLocalReferenceConverter,
            AngularModuleSourceCodeAdder angularModuleSourceCodeAdder)
        {
            JsonSerializer = jsonSerializer;
            ProjectNugetPackageAdder = projectNugetPackageAdder;
            DbContextFileBuilderConfigureAdder = dbContextFileBuilderConfigureAdder;
            EfCoreMigrationAdder = efCoreMigrationAdder;
            DerivedClassFinder = derivedClassFinder;
            ProjectNpmPackageAdder = projectNpmPackageAdder;
            NpmGlobalPackagesChecker = npmGlobalPackagesChecker;
            RemoteServiceExceptionHandler = remoteServiceExceptionHandler;
            SourceCodeDownloadService = sourceCodeDownloadService;
            SolutionFileModifier = solutionFileModifier;
            NugetPackageToLocalReferenceConverter = nugetPackageToLocalReferenceConverter;
            AngularModuleSourceCodeAdder = angularModuleSourceCodeAdder;
            Logger = NullLogger<SolutionModuleAdder>.Instance;
        }

        public virtual async Task AddAsync(
            [NotNull] string solutionFile,
            [NotNull] string moduleName,
            string startupProject,
            string version,
            bool skipDbMigrations = false,
            bool withSourceCode = false)
        {
            Check.NotNull(solutionFile, nameof(solutionFile));
            Check.NotNull(moduleName, nameof(moduleName));

            var module = await FindModuleInfoAsync(moduleName);

            Logger.LogInformation($"Installing module '{module.Name}' to the solution '{Path.GetFileNameWithoutExtension(solutionFile)}'");

            var projectFiles = ProjectFinder.GetProjectFiles(solutionFile);

            await AddNugetAndNpmReferences(module, projectFiles);

            if (withSourceCode)
            {
                var modulesFolderInSolution = Path.Combine(Path.GetDirectoryName(solutionFile), "modules");
                await DownloadSourceCodesToSolutionFolder(module, modulesFolderInSolution, version);
                await SolutionFileModifier.AddModuleToSolutionFileAsync(module, solutionFile);
                await NugetPackageToLocalReferenceConverter.Convert(module, solutionFile);

                await HandleAngularProject(modulesFolderInSolution, solutionFile);
            }

            ModifyDbContext(projectFiles, module, startupProject, skipDbMigrations);
        }

        private async Task HandleAngularProject(string modulesFolderInSolution, string solutionFilePath)
        {
            var angularPath = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(solutionFilePath)), "angular");

            if (!Directory.Exists(angularPath))
            {
                DeleteAngularDirectoriesInModulesFolder(modulesFolderInSolution);
                return;
            }

            await AngularModuleSourceCodeAdder.AddAsync(solutionFilePath, angularPath);
        }

        private static void DeleteAngularDirectoriesInModulesFolder(string modulesFolderInSolution)
        {
            var moduleFolders = Directory.GetDirectories(modulesFolderInSolution);

            foreach (var moduleFolder in moduleFolders)
            {
                var angDir = Path.Combine(moduleFolder, "angular");
                if (Directory.Exists(angDir))
                {
                    Directory.Delete(angDir, true);
                }
            }
        }

        private async Task DownloadSourceCodesToSolutionFolder(ModuleWithMastersInfo module, string modulesFolderInSolution, string version = null)
        {
            var targetModuleFolder = Path.Combine(modulesFolderInSolution, module.Name);

            await SourceCodeDownloadService.DownloadAsync(
                module.Name,
                targetModuleFolder,
                version,
                null,
                null
            );

            await DeleteAppAndDemoFolderAsync(targetModuleFolder);

            if (module.MasterModuleInfos == null)
            {
                return;
            }

            foreach (var masterModule in module.MasterModuleInfos)
            {
                await DownloadSourceCodesToSolutionFolder(masterModule, modulesFolderInSolution, version);
            }
        }

        private async Task DeleteAppAndDemoFolderAsync(string targetModuleFolder)
        {
            var appFolder = Path.Combine(targetModuleFolder, "app");
            if (Directory.Exists(appFolder))
            {
                Directory.Delete(appFolder, true);
            }

            var demoFolder = Path.Combine(targetModuleFolder, "demo");
            if (Directory.Exists(demoFolder))
            {
                Directory.Delete(demoFolder, true);
            }
        }

        private async Task AddNugetAndNpmReferences(ModuleWithMastersInfo module, string[] projectFiles)
        {
            foreach (var nugetPackage in module.NugetPackages)
            {
                var nugetTarget =
                    await IsProjectTiered(projectFiles) && nugetPackage.TieredTarget != NuGetPackageTarget.Undefined
                        ? nugetPackage.TieredTarget
                        : nugetPackage.Target;

                var targetProjectFile = ProjectFinder.FindNuGetTargetProjectFile(projectFiles, nugetTarget);
                if (targetProjectFile == null)
                {
                    Logger.LogDebug($"Target project is not available for this NuGet package '{nugetPackage.Name}'");
                    continue;
                }

                await ProjectNugetPackageAdder.AddAsync(targetProjectFile, nugetPackage);
            }

            if (!module.NpmPackages.IsNullOrEmpty())
            {
                var targetProjects = ProjectFinder.FindNpmTargetProjectFile(projectFiles);
                if (targetProjects.Any())
                {
                    NpmGlobalPackagesChecker.Check();

                    foreach (var targetProject in targetProjects)
                    {
                        foreach (var npmPackage in module.NpmPackages.Where(p =>
                            p.ApplicationType.HasFlag(NpmApplicationType.Mvc)))
                        {
                            await ProjectNpmPackageAdder.AddAsync(Path.GetDirectoryName(targetProject), npmPackage);
                        }
                    }
                }
                else
                {
                    Logger.LogDebug("Target project is not available for NPM packages.");
                }
            }
        }

        protected void ModifyDbContext(string[] projectFiles, ModuleInfo module, string startupProject, bool skipDbMigrations = false)
        {
            if (string.IsNullOrWhiteSpace(module.EfCoreConfigureMethodName))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(startupProject))
            {
                startupProject = projectFiles.FirstOrDefault(p => p.EndsWith(".DbMigrator.csproj"));
            }

            var dbMigrationsProject = projectFiles.FirstOrDefault(p => p.EndsWith(".DbMigrations.csproj"));
            
            if (dbMigrationsProject == null)
            {
                Logger.LogDebug("Solution doesn't have a \".DbMigrations\" project.");
                return;
            }

            var dbContextFile = DerivedClassFinder.Find(dbMigrationsProject, "AbpDbContext").FirstOrDefault();

            if (dbContextFile == null)
            {
                Logger.LogDebug($"{dbMigrationsProject} project doesn't have a class that is derived from \"AbpDbContext\".");
                return;
            }

            var addedNewBuilder = DbContextFileBuilderConfigureAdder.Add(dbContextFile, module.EfCoreConfigureMethodName);

            if (addedNewBuilder && !skipDbMigrations)
            {
                EfCoreMigrationAdder.AddMigration(dbMigrationsProject, module.Name, startupProject); 
            }
        }

        protected virtual async Task<ModuleWithMastersInfo> FindModuleInfoAsync(string moduleName)
        {
            using (var client = new CliHttpClient())
            {
                var url = $"{CliUrls.WwwAbpIo}api/app/module/byNameWithDetails/?name=" + moduleName;

                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new CliUsageException($"ERROR: '{moduleName}' module could not be found!");
                    }

                    await RemoteServiceExceptionHandler.EnsureSuccessfulHttpResponseAsync(response);
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ModuleWithMastersInfo>(responseContent);
            }
        }

        protected virtual async Task<bool> IsProjectTiered(string[] projectFiles)
        {
            return projectFiles.Select(ProjectFileNameHelper.GetAssemblyNameFromProjectPath)
                .Any(p => p.EndsWith(".IdentityServer") || p.EndsWith(".HttpApi.Host"));
        }
    }
}