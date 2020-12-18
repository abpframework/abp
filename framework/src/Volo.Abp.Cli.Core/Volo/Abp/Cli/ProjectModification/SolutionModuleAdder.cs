using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Bundling;
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
        public BundleCommand BundleCommand { get; }

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
            NewCommand newCommand,
            BundleCommand bundleCommand)
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
            BundleCommand = bundleCommand;
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

            await RunBundleForBlazorAsync(projectFiles, module);

            ModifyDbContext(projectFiles, module, startupProject, skipDbMigrations);
        }

        private async Task RunBundleForBlazorAsync(string[] projectFiles, ModuleWithMastersInfo module)
        {
            var blazorProject = projectFiles.FirstOrDefault(f => f.EndsWith(".Blazor.csproj"));

            if (blazorProject == null || !module.NugetPackages.Any(np=> np.Target == NuGetPackageTarget.Blazor))
            {
                return;
            }

            var args = new CommandLineArgs("bundle");

            args.Options.Add(BundleCommand.Options.WorkingDirectory.Short, Path.GetDirectoryName(blazorProject));
            args.Options.Add(BundleCommand.Options.ForceBuild.Short, string.Empty);

            await BundleCommand.ExecuteAsync(args);
        }

        private async Task RemoveUnnecessaryProjectsAsync(string solutionDirectory, ModuleWithMastersInfo module,
            string[] projectFiles)
        {
            var moduleDirectory = Path.Combine(solutionDirectory, "modules", module.Name);
            var moduleSolutionFile = Directory.GetFiles(moduleDirectory, "*.sln", SearchOption.TopDirectoryOnly).First();
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
                await ChangeDomainTestReferenceToMongoDB(module, moduleSolutionFile);
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
                if (target == NuGetPackageTarget.Web && package.Name.StartsWith("Volo.Abp.Account"))
                {
                    continue;
                }

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
            await SolutionFileModifier.RemoveProjectFromSolutionFileAsync(moduleSolutionFile, new DirectoryInfo(projectFolderPath).Name);

            if (Directory.Exists(projectFolderPath))
            {
                Directory.Delete(projectFolderPath, true);
            }

        }

        private async Task ChangeDomainTestReferenceToMongoDB(ModuleWithMastersInfo module, string moduleSolutionFile)
        {
            var testPath = Path.Combine(Path.GetDirectoryName(moduleSolutionFile), "test");

            if (!Directory.Exists(testPath))
            {
                return;
            }

            var projectFolderPath = Directory.GetDirectories(testPath).FirstOrDefault(d=> d.EndsWith("Domain.Tests"));

            if (projectFolderPath == null)
            {
                return;
            }

            var csprojFile = Directory.GetFiles(projectFolderPath).FirstOrDefault(p => p.EndsWith(".csproj"));
            var moduleFile = Directory.GetFiles(projectFolderPath).FirstOrDefault(p => p.EndsWith("DomainTestModule.cs"));

            if (csprojFile == null || moduleFile == null)
            {
                return;
            }

            File.WriteAllText(csprojFile, File.ReadAllText(csprojFile).Replace("EntityFrameworkCore","MongoDB"));
            File.WriteAllText(moduleFile, File.ReadAllText(moduleFile)
                .Replace(".EntityFrameworkCore;",".MongoDB;")
                .Replace("EntityFrameworkCoreTestModule","MongoDbTestModule"));
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

            await DeleteRedundantHostProjects(targetModuleFolder,"app");
            await DeleteRedundantHostProjects(targetModuleFolder,"demo");
            await DeleteRedundantHostProjects(targetModuleFolder,"host");

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

        private async Task DeleteRedundantHostProjects(string targetModuleFolder, string folderName)
        {
            var moduleSolutionFile = Directory.GetFiles(targetModuleFolder, "*.sln", SearchOption.TopDirectoryOnly).First();

            var folder = Path.Combine(targetModuleFolder, folderName);
            if (Directory.Exists(folder))
            {
                var projects = Directory.GetDirectories(folder);

                foreach (var project in projects)
                {
                    await SolutionFileModifier.RemoveProjectFromSolutionFileAsync(moduleSolutionFile,
                        new DirectoryInfo(project).Name);
                }

                Directory.Delete(folder, true);
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
            var module = new ModuleWithMastersInfo
            {
                Name = moduleName,
                DisplayName = moduleName,
                MasterModuleInfos = new List<ModuleWithMastersInfo>()
            };

            var moduleProjectName = module.Name.Split('.').Last();
            module.EfCoreConfigureMethodName = $"{module.Name}.EntityFrameworkCore:Configure{moduleProjectName}";

            module.NugetPackages = new List<NugetPackageInfo>
            {
                new NugetPackageInfo
                {
                    Name = $"{module.Name}.Application",
                    ModuleClass = $"{module.Name}.{moduleProjectName}ApplicationModule",
                    Target = NuGetPackageTarget.Application
                },
                new NugetPackageInfo
                {
                    Name = $"{module.Name}.Application.Contracts",
                    ModuleClass = $"{module.Name}.{moduleProjectName}ApplicationContractsModule",
                    Target = NuGetPackageTarget.ApplicationContracts
                },
                new NugetPackageInfo
                {
                    Name = $"{module.Name}.Blazor",
                    ModuleClass = $"{module.Name}.Blazor.{moduleProjectName}BlazorModule",
                    Target = NuGetPackageTarget.Blazor
                },
                new NugetPackageInfo
                {
                    Name = $"{module.Name}.Domain",
                    ModuleClass = $"{module.Name}.{moduleProjectName}DomainModule",
                    Target = NuGetPackageTarget.Domain
                },
                new NugetPackageInfo
                {
                    Name = $"{module.Name}.Domain.Shared",
                    ModuleClass = $"{module.Name}.{moduleProjectName}DomainSharedModule",
                    Target = NuGetPackageTarget.DomainShared
                },
                new NugetPackageInfo
                {
                    Name = $"{module.Name}.EntityFrameworkCore",
                    ModuleClass = $"{module.Name}.EntityFrameworkCore.{moduleProjectName}EntityFrameworkCoreModule",
                    Target = NuGetPackageTarget.EntityFrameworkCore
                },
                new NugetPackageInfo
                {
                    Name = $"{module.Name}.HttpApi",
                    ModuleClass = $"{module.Name}.{moduleProjectName}HttpApiModule",
                    Target = NuGetPackageTarget.HttpApi
                },
                new NugetPackageInfo
                {
                    Name = $"{module.Name}.HttpApi.Client",
                    ModuleClass = $"{module.Name}.{moduleProjectName}HttpApiClientModule",
                    Target = NuGetPackageTarget.HttpApiClient
                },
                new NugetPackageInfo
                {
                    Name = $"{module.Name}.MongoDB",
                    ModuleClass = $"{module.Name}.MongoDB.{moduleProjectName}MongoDbModule",
                    Target = NuGetPackageTarget.MongoDB
                },
                new NugetPackageInfo
                {
                    Name = $"{module.Name}.Web",
                    ModuleClass = $"{module.Name}.Web.{moduleProjectName}WebModule",
                    Target = NuGetPackageTarget.Web
                },
            };

            module.NpmPackages = new List<NpmPackageInfo>();

            return module;
        }

        protected virtual async Task<bool> IsProjectTiered(string[] projectFiles)
        {
            return projectFiles.Select(ProjectFileNameHelper.GetAssemblyNameFromProjectPath)
                .Any(p =>p.EndsWith(".HttpApi.Host"))
                && projectFiles.Select(ProjectFileNameHelper.GetAssemblyNameFromProjectPath)
                .Any(p => p.EndsWith(".IdentityServer"));
        }
    }
}
