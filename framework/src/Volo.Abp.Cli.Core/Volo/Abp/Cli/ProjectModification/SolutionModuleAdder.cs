﻿using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Commands;
using Volo.Abp.Cli.Commands.Services;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.Cli.ProjectBuilding.Templates.MvcModule;
using Volo.Abp.Cli.Utils;
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
        public NewCommand NewCommand { get; }

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
            AngularModuleSourceCodeAdder angularModuleSourceCodeAdder,
            NewCommand newCommand)
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
            NewCommand = newCommand;
            Logger = NullLogger<SolutionModuleAdder>.Instance;
        }

        public virtual async Task AddAsync(
            [NotNull] string solutionFile,
            [NotNull] string moduleName,
            string startupProject,
            string version,
            bool skipDbMigrations = false,
            bool withSourceCode = false,
            bool addSourceCodeToSolutionFile = false,
            bool newTemplate = false,
            bool newProTemplate = false)
        {
            Check.NotNull(solutionFile, nameof(solutionFile));
            Check.NotNull(moduleName, nameof(moduleName));

            var module = await GetModuleInfoAsync(moduleName, newTemplate, newProTemplate);

            Logger.LogInformation(
                $"Installing module '{module.Name}' to the solution '{Path.GetFileNameWithoutExtension(solutionFile)}'");

            var projectFiles = ProjectFinder.GetProjectFiles(solutionFile);

            await AddNugetAndNpmReferences(module, projectFiles, !(newTemplate || newProTemplate));

            if (withSourceCode || newTemplate || newProTemplate)
            {
                var modulesFolderInSolution = Path.Combine(Path.GetDirectoryName(solutionFile), "modules");
                await DownloadSourceCodesToSolutionFolder(module, modulesFolderInSolution, version, newTemplate,
                    newProTemplate);
                await RemoveUnnecessaryProjectsAsync(Path.GetDirectoryName(solutionFile), module, projectFiles);

                if (addSourceCodeToSolutionFile)
                {
                    await SolutionFileModifier.AddModuleToSolutionFileAsync(module, solutionFile);
                }

                if (newTemplate || newProTemplate)
                {
                    await NugetPackageToLocalReferenceConverter.Convert(module, solutionFile, $"{module.Name}.");
                }
                else
                {
                    await NugetPackageToLocalReferenceConverter.Convert(module, solutionFile);
                }

                await AddAngularSourceCode(modulesFolderInSolution, solutionFile);
            }
            else
            {
                await AddAngularPackages(solutionFile, module);
            }

            ModifyDbContext(projectFiles, module, startupProject, skipDbMigrations);
        }

        private async Task RemoveUnnecessaryProjectsAsync(string solutionDirectory, ModuleWithMastersInfo module,
            string[] projectFiles)
        {
            var moduleDirectory = Path.Combine(solutionDirectory, "modules", module.Name);
            var moduleSolutionFile =
                Directory.GetFiles(moduleDirectory, "*.sln", SearchOption.TopDirectoryOnly).First();
            var isProjectTiered = await IsProjectTiered(projectFiles);

            if (!projectFiles.Any(p => p.EndsWith(".Blazor.csproj")))
            {
                await RemoveProjectByTarget(module, moduleSolutionFile, NuGetPackageTarget.Blazor, isProjectTiered);
            }

            if (!projectFiles.Any(p => p.EndsWith(".Web.csproj")))
            {
                await RemoveProjectByTarget(module, moduleSolutionFile, NuGetPackageTarget.Web, isProjectTiered);
            }

            if (!projectFiles.Any(p => p.EndsWith(".MongoDB.csproj")))
            {
                await RemoveProjectByTarget(module, moduleSolutionFile, NuGetPackageTarget.MongoDB, isProjectTiered);
                await RemoveProjectByPostFix(module, moduleSolutionFile, "test", ".MongoDB.Tests");
            }

            if (!projectFiles.Any(p => p.EndsWith(".EntityFrameworkCore.csproj")))
            {
                await RemoveProjectByTarget(module, moduleSolutionFile, NuGetPackageTarget.EntityFrameworkCore, isProjectTiered);
                await RemoveProjectByPostFix(module, moduleSolutionFile, "test", ".EntityFrameworkCore.Tests");
            }
        }

        private async Task RemoveProjectByTarget(ModuleWithMastersInfo module, string moduleSolutionFile,
            NuGetPackageTarget target, bool isTieredProject)
        {
            var packages = module.NugetPackages.Where(n =>
                (isTieredProject && n.TieredTarget != NuGetPackageTarget.Undefined
                    ? n.TieredTarget
                    : n.Target) == target
            ).ToList();

            foreach (var package in packages)
            {
                await SolutionFileModifier.RemoveProjectFromSolutionFileAsync(moduleSolutionFile, package.Name);

                var projectPath = Path.Combine(Path.GetDirectoryName(moduleSolutionFile), "src", package.Name);
                if (Directory.Exists(projectPath))
                {
                    Directory.Delete(projectPath, true);
                }
            }
        }

        private async Task RemoveProjectByPostFix(ModuleWithMastersInfo module, string moduleSolutionFile, string targetFolder,
            string postFix)
        {
            var srcPath = Path.Combine(Path.GetDirectoryName(moduleSolutionFile), targetFolder);
            var projectFolderPath = Directory.GetDirectories(srcPath).FirstOrDefault(d=> d.EndsWith(postFix));
            await SolutionFileModifier.RemoveProjectFromSolutionFileAsync(moduleSolutionFile, Path.GetDirectoryName(projectFolderPath));

            if (Directory.Exists(projectFolderPath))
            {
                Directory.Delete(projectFolderPath, true);
            }
        }

        private async Task AddAngularPackages(string solutionFilePath, ModuleWithMastersInfo module)
        {
            var angularPath = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(solutionFilePath)), "angular");

            if (!Directory.Exists(angularPath))
            {
                return;
            }

            var angularPackages = module.NpmPackages?.Where(p => p.ApplicationType.HasFlag(NpmApplicationType.Angular))
                .ToList();

            if (!angularPackages.IsNullOrEmpty())
            {
                foreach (var npmPackage in angularPackages)
                {
                    await ProjectNpmPackageAdder.AddAsync(angularPath, npmPackage, true);
                }
            }
        }

        private async Task AddAngularSourceCode(string modulesFolderInSolution, string solutionFilePath)
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

        private async Task DownloadSourceCodesToSolutionFolder(ModuleWithMastersInfo module,
            string modulesFolderInSolution, string version = null, bool newTemplate = false,
            bool newProTemplate = false)
        {
            var targetModuleFolder = Path.Combine(modulesFolderInSolution, module.Name);

            if (newTemplate || newProTemplate)
            {
                await CreateNewModuleAsync(module, modulesFolderInSolution, version, newProTemplate);
            }
            else
            {
                await SourceCodeDownloadService.DownloadAsync(
                    module.Name,
                    targetModuleFolder,
                    version,
                    null,
                    null,
                    null
                );
            }

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

        private async Task CreateNewModuleAsync(ModuleWithMastersInfo module, string modulesFolderInSolution,
            string version, bool newProTemplate = false)
        {
            var args = new CommandLineArgs("new", module.Name);

            args.Options.Add("t", newProTemplate ? ModuleProTemplate.TemplateName : ModuleTemplate.TemplateName);
            args.Options.Add("v", version);
            args.Options.Add("o", Path.Combine(modulesFolderInSolution, module.Name));

            await NewCommand.ExecuteAsync(args);
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

            var hostFolder = Path.Combine(targetModuleFolder, "host");
            if (Directory.Exists(hostFolder))
            {
                Directory.Delete(hostFolder, true);
            }
        }

        private async Task AddNugetAndNpmReferences(ModuleWithMastersInfo module, string[] projectFiles,
            bool useDotnetCliToInstall)
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

                await ProjectNugetPackageAdder.AddAsync(targetProjectFile, nugetPackage, null, useDotnetCliToInstall);
            }

            var mvcNpmPackages = module.NpmPackages?.Where(p => p.ApplicationType.HasFlag(NpmApplicationType.Mvc))
                .ToList();

            if (!mvcNpmPackages.IsNullOrEmpty())
            {
                var targetProjects = ProjectFinder.FindNpmTargetProjectFile(projectFiles);
                if (targetProjects.Any())
                {
                    NpmGlobalPackagesChecker.Check();

                    foreach (var targetProject in targetProjects)
                    {
                        foreach (var npmPackage in mvcNpmPackages)
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

        protected void ModifyDbContext(string[] projectFiles, ModuleInfo module, string startupProject,
            bool skipDbMigrations = false)
        {
            if (string.IsNullOrWhiteSpace(module.EfCoreConfigureMethodName))
            {
                if (!skipDbMigrations)
                {
                    RunMigrator(projectFiles);
                }

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

                if (!skipDbMigrations)
                {
                    RunMigrator(projectFiles);
                }

                return;
            }

            var dbContextFile = DerivedClassFinder.Find(dbMigrationsProject, "AbpDbContext").FirstOrDefault();

            if (dbContextFile == null)
            {
                Logger.LogDebug(
                    $"{dbMigrationsProject} project doesn't have a class that is derived from \"AbpDbContext\".");
                return;
            }

            var addedNewBuilder =
                DbContextFileBuilderConfigureAdder.Add(dbContextFile, module.EfCoreConfigureMethodName);

            if (!skipDbMigrations)
            {
                if (addedNewBuilder)
                {
                    EfCoreMigrationAdder.AddMigration(dbMigrationsProject, module.Name, startupProject);
                }

                RunMigrator(projectFiles);
            }
        }

        protected virtual async Task RunMigrator(string[] projectFiles)
        {
            var dbMigratorProject = projectFiles.FirstOrDefault(p => p.EndsWith(".DbMigrator.csproj"));

            if (!string.IsNullOrEmpty(dbMigratorProject))
            {
                CmdHelper.RunCmd("cd \"" + Path.GetDirectoryName(dbMigratorProject) + "\" && dotnet run");
            }
        }

        protected virtual async Task<ModuleWithMastersInfo> GetModuleInfoAsync(string moduleName, bool newTemplate,
            bool newProTemplate = false)
        {
            if (newTemplate || newProTemplate)
            {
                return await GetEmptyModuleProjectInfoAsync(moduleName, newProTemplate);
            }

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

        private async Task<ModuleWithMastersInfo> GetEmptyModuleProjectInfoAsync(string moduleName,
            bool newProTemplate = false)
        {
            var module = new ModuleWithMastersInfo();

            module.Name = moduleName;
            module.DisplayName = moduleName;
            module.EfCoreConfigureMethodName = $"{module.Name}.EntityFrameworkCore:Configure{module.Name}";
            module.MasterModuleInfos = new List<ModuleWithMastersInfo>();

            module.NugetPackages = new List<NugetPackageInfo>
            {
                new NugetPackageInfo
                {
                    Name = $"{module.Name}.Application",
                    ModuleClass = $"{module.Name}.{module.Name}ApplicationModule",
                    Target = NuGetPackageTarget.Application
                },
                new NugetPackageInfo
                {
                    Name = $"{module.Name}.Application.Contracts",
                    ModuleClass = $"{module.Name}.{module.Name}ApplicationContractsModule",
                    Target = NuGetPackageTarget.ApplicationContracts
                },
                new NugetPackageInfo
                {
                    Name = $"{module.Name}.Blazor",
                    ModuleClass = $"{module.Name}.Blazor.{module.Name}BlazorModule",
                    Target = NuGetPackageTarget.Blazor
                },
                new NugetPackageInfo
                {
                    Name = $"{module.Name}.Domain",
                    ModuleClass = $"{module.Name}.{module.Name}DomainModule",
                    Target = NuGetPackageTarget.Domain
                },
                new NugetPackageInfo
                {
                    Name = $"{module.Name}.Domain.Shared",
                    ModuleClass = $"{module.Name}.{module.Name}DomainSharedModule",
                    Target = NuGetPackageTarget.DomainShared
                },
                new NugetPackageInfo
                {
                    Name = $"{module.Name}.EntityFrameworkCore",
                    ModuleClass = $"{module.Name}.EntityFrameworkCore.{module.Name}EntityFrameworkCoreModule",
                    Target = NuGetPackageTarget.EntityFrameworkCore
                },
                new NugetPackageInfo
                {
                    Name = $"{module.Name}.HttpApi",
                    ModuleClass = $"{module.Name}.{module.Name}HttpApiModule",
                    Target = NuGetPackageTarget.HttpApi
                },
                new NugetPackageInfo
                {
                    Name = $"{module.Name}.HttpApi.Client",
                    ModuleClass = $"{module.Name}.{module.Name}HttpApiClientModule",
                    Target = NuGetPackageTarget.HttpApiClient
                },
                new NugetPackageInfo
                {
                    Name = $"{module.Name}.MongoDB",
                    ModuleClass = $"{module.Name}.MongoDB.{module.Name}MongoDbModule",
                    Target = NuGetPackageTarget.MongoDB
                },
                new NugetPackageInfo
                {
                    Name = $"{module.Name}.Web",
                    ModuleClass = $"{module.Name}.Web.{module.Name}WebModule",
                    Target = NuGetPackageTarget.Web
                },
            };

            module.NpmPackages = new List<NpmPackageInfo>();

            return module;
        }

        protected virtual async Task<bool> IsProjectTiered(string[] projectFiles)
        {
            return projectFiles.Select(ProjectFileNameHelper.GetAssemblyNameFromProjectPath)
                .Any(p => p.EndsWith(".IdentityServer") || p.EndsWith(".HttpApi.Host"));
        }
    }
}
