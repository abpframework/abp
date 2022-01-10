using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using NuGet.Versioning;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Bundling;
using Volo.Abp.Cli.Commands;
using Volo.Abp.Cli.Commands.Services;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.Cli.ProjectBuilding.Files;
using Volo.Abp.Cli.ProjectBuilding.Templates.MvcModule;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace Volo.Abp.Cli.ProjectModification;

public class SolutionModuleAdder : ITransientDependency
{
    public ILogger<SolutionModuleAdder> Logger { get; set; }
    public SourceCodeDownloadService SourceCodeDownloadService { get; }
    public SolutionFileModifier SolutionFileModifier { get; }
    public NugetPackageToLocalReferenceConverter NugetPackageToLocalReferenceConverter { get; }
    public AngularSourceCodeAdder AngularSourceCodeAdder { get; }
    public NewCommand NewCommand { get; }
    public BundleCommand BundleCommand { get; }
    public ICmdHelper CmdHelper { get; }

    protected IJsonSerializer JsonSerializer { get; }
    protected ProjectNugetPackageAdder ProjectNugetPackageAdder { get; }
    protected DbContextFileBuilderConfigureAdder DbContextFileBuilderConfigureAdder { get; }
    protected EfCoreMigrationManager EfCoreMigrationManager { get; }
    protected DerivedClassFinder DerivedClassFinder { get; }
    protected ProjectNpmPackageAdder ProjectNpmPackageAdder { get; }
    protected NpmGlobalPackagesChecker NpmGlobalPackagesChecker { get; }
    protected IRemoteServiceExceptionHandler RemoteServiceExceptionHandler { get; }

    private readonly CliHttpClientFactory _cliHttpClientFactory;


    public SolutionModuleAdder(
        IJsonSerializer jsonSerializer,
        ProjectNugetPackageAdder projectNugetPackageAdder,
        DbContextFileBuilderConfigureAdder dbContextFileBuilderConfigureAdder,
        EfCoreMigrationManager efCoreMigrationManager,
        DerivedClassFinder derivedClassFinder,
        ProjectNpmPackageAdder projectNpmPackageAdder,
        NpmGlobalPackagesChecker npmGlobalPackagesChecker,
        IRemoteServiceExceptionHandler remoteServiceExceptionHandler,
        SourceCodeDownloadService sourceCodeDownloadService,
        SolutionFileModifier solutionFileModifier,
        NugetPackageToLocalReferenceConverter nugetPackageToLocalReferenceConverter,
        AngularSourceCodeAdder angularSourceCodeAdder,
        NewCommand newCommand,
        BundleCommand bundleCommand,
        CliHttpClientFactory cliHttpClientFactory,
        ICmdHelper cmdHelper)
    {
        JsonSerializer = jsonSerializer;
        ProjectNugetPackageAdder = projectNugetPackageAdder;
        DbContextFileBuilderConfigureAdder = dbContextFileBuilderConfigureAdder;
        EfCoreMigrationManager = efCoreMigrationManager;
        DerivedClassFinder = derivedClassFinder;
        ProjectNpmPackageAdder = projectNpmPackageAdder;
        NpmGlobalPackagesChecker = npmGlobalPackagesChecker;
        RemoteServiceExceptionHandler = remoteServiceExceptionHandler;
        SourceCodeDownloadService = sourceCodeDownloadService;
        SolutionFileModifier = solutionFileModifier;
        NugetPackageToLocalReferenceConverter = nugetPackageToLocalReferenceConverter;
        AngularSourceCodeAdder = angularSourceCodeAdder;
        NewCommand = newCommand;
        BundleCommand = bundleCommand;
        CmdHelper = cmdHelper;
        _cliHttpClientFactory = cliHttpClientFactory;
        Logger = NullLogger<SolutionModuleAdder>.Instance;
    }

    public virtual async Task<ModuleWithMastersInfo> AddAsync([NotNull] string solutionFile,
        [NotNull] string moduleName,
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
        module = RemoveIncompatiblePackages(module, version);

        Logger.LogInformation($"Installing module '{module.Name}' to the solution '{Path.GetFileNameWithoutExtension(solutionFile)}'");

        var projectFiles = ProjectFinder.GetProjectFiles(solutionFile);

        await AddNugetAndNpmReferences(module, projectFiles, !(newTemplate || newProTemplate));

        if (withSourceCode || newTemplate || newProTemplate)
        {
            var modulesFolderInSolution = Path.Combine(Path.GetDirectoryName(solutionFile), "modules");
            await DownloadSourceCodesToSolutionFolder(module, modulesFolderInSolution, version, newTemplate, newProTemplate);
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

            await AddAngularSourceCode(modulesFolderInSolution, solutionFile, module.Name, newTemplate || newProTemplate);
        }
        else
        {
            await AddAngularPackages(solutionFile, module);
        }

        await RunBundleForBlazorAsync(projectFiles, module);

        ModifyDbContext(projectFiles, module, skipDbMigrations);

        var documentationLink = module.GetFirstDocumentationLinkOrNull();
        if (documentationLink != null)
        {
            CmdHelper.OpenWebPage(documentationLink);
        }

        return module;
    }

    private ModuleWithMastersInfo RemoveIncompatiblePackages(ModuleWithMastersInfo module, string version)
    {
        module.NugetPackages.RemoveAll(np => IsPackageInCompatible(np.MinVersion, np.MaxVersion, version));
        module.NpmPackages.RemoveAll(np => IsPackageInCompatible(np.MinVersion, np.MaxVersion, version));
        return module;
    }

    private static bool IsPackageInCompatible(string minVersion, string maxVersion, string version)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(minVersion))
            {
                if (SemanticVersion.Parse(minVersion) > SemanticVersion.Parse(version))
                {
                    return true;
                }
            }
            if (!string.IsNullOrWhiteSpace(maxVersion))
            {
                if (SemanticVersion.Parse(maxVersion) < SemanticVersion.Parse(version))
                {
                    return true;
                }
            }

            return false;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }

    private async Task RunBundleForBlazorAsync(string[] projectFiles, ModuleWithMastersInfo module)
    {
        var blazorProject = projectFiles.FirstOrDefault(f => f.EndsWith(".Blazor.csproj"));

        if (blazorProject == null || !module.NugetPackages.Any(np => np.Target == NuGetPackageTarget.Blazor))
        {
            return;
        }
        // return if project is blazor-server
        var document = new XmlDocument();
        document.Load(blazorProject);
        var sdk = document.DocumentElement.GetAttribute("Sdk");
        if (sdk != BundlingConsts.SupportedWebAssemblyProjectType)
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
        var projectsToRemove = new List<string>();
        var moduleDirectory = Path.Combine(solutionDirectory, "modules", module.Name);
        var moduleSolutionFile = Directory.GetFiles(moduleDirectory, "*.sln", SearchOption.TopDirectoryOnly).First();
        var isProjectTiered = await IsProjectTiered(projectFiles);
        var webPackagesWillBeAddedToBlazorServerProject = false;

        var blazorProject = projectFiles.FirstOrDefault(p => p.EndsWith(".Blazor.csproj"));
        if (blazorProject == null)
        {
            projectsToRemove.AddRange(await FindProjectsToRemoveByTarget(module, NuGetPackageTarget.Blazor, isProjectTiered));
            projectsToRemove.AddRange(await FindProjectsToRemoveByTarget(module, NuGetPackageTarget.BlazorServer, isProjectTiered));
            projectsToRemove.AddRange(await FindProjectsToRemoveByTarget(module, NuGetPackageTarget.BlazorWebAssembly, isProjectTiered));
            projectsToRemove.AddRange(await FindProjectsToRemoveByPostFix(moduleDirectory, "src", ".Blazor"));
        }
        else
        {
            var isBlazorServer = BlazorProjectTypeChecker.IsBlazorServerProject(blazorProject);

            if (isBlazorServer)
            {
                projectsToRemove.AddRange(await FindProjectsToRemoveByTarget(module, NuGetPackageTarget.BlazorWebAssembly, isProjectTiered));

                webPackagesWillBeAddedToBlazorServerProject = module.NugetPackages.All(np => np.Target != NuGetPackageTarget.BlazorServer && np.TieredTarget != NuGetPackageTarget.BlazorServer);
            }
            else
            {
                projectsToRemove.AddRange(await FindProjectsToRemoveByTarget(module, NuGetPackageTarget.BlazorServer, isProjectTiered));
            }
        }

        if (!projectFiles.Any(p => p.EndsWith(".Web.csproj")) && !webPackagesWillBeAddedToBlazorServerProject)
        {
            projectsToRemove.AddRange(await FindProjectsToRemoveByTarget(module, NuGetPackageTarget.Web, isProjectTiered));
        }

        if (!projectFiles.Any(p => p.EndsWith(".MongoDB.csproj")))
        {
            projectsToRemove.AddRange(await FindProjectsToRemoveByTarget(module, NuGetPackageTarget.MongoDB, isProjectTiered));
            projectsToRemove.AddRange(await FindProjectsToRemoveByPostFix(moduleDirectory, "test", ".MongoDB.Tests"));
        }

        if (!projectFiles.Any(p => p.EndsWith(".EntityFrameworkCore.csproj")))
        {
            projectsToRemove.AddRange(await FindProjectsToRemoveByTarget(module, NuGetPackageTarget.EntityFrameworkCore, isProjectTiered));
            projectsToRemove.AddRange(await FindProjectsToRemoveByPostFix(moduleDirectory, "test", ".EntityFrameworkCore.Tests"));
            projectsToRemove.AddRange(await FindProjectsToRemoveByPostFix(moduleDirectory, "test", ".Application.Tests"));
            ChangeDomainTestReferenceToMongoDB(module, moduleSolutionFile);
        }

        foreach (var projectToRemove in projectsToRemove)
        {
            if (IsReferencedByAnotherProject(solutionDirectory, projectsToRemove, projectToRemove))
            {
                continue;
            }

            RemoveProjectFromSolutionAsync(moduleSolutionFile, projectToRemove);
        }
    }

    private bool IsReferencedByAnotherProject(string solutionDirectory, List<string> projectsToRemove, string projectToRemove)
    {
        var projects = Directory.GetFiles(solutionDirectory, "*.csproj", SearchOption.AllDirectories);
        var projectsToKeep = projects.Where(mp => !projectsToRemove.Contains(Path.GetFileName(mp).RemovePostFix(".csproj"))).ToList();
        return projectsToKeep.Select(File.ReadAllText).Any(content => content.Contains($"\"{projectToRemove}\""));
    }

    private async Task<List<string>> FindProjectsToRemoveByTarget(ModuleWithMastersInfo module,
        NuGetPackageTarget target, bool isTieredProject)
    {
        var projectsToRemove = new List<string>();

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

            projectsToRemove.Add(package.Name);
        }

        return projectsToRemove;
    }

    private async Task<List<string>> FindProjectsToRemoveByPostFix(string moduleDirectory, string targetFolder,
        string postFix)
    {
        var projectsToRemove = new List<string>();
        var srcPath = Path.Combine(moduleDirectory, targetFolder);

        if (!Directory.Exists(srcPath))
        {
            return projectsToRemove;
        }

        var projectFolderPaths = Directory.GetDirectories(srcPath).Where(d => d.EndsWith(postFix)).ToList();

        foreach (var projectFolderPath in projectFolderPaths)
        {
            projectsToRemove.Add(new DirectoryInfo(projectFolderPath).Name);
        }

        return projectsToRemove;
    }

    private async Task RemoveProjectFromSolutionAsync(string moduleSolutionFile, string projectName)
    {
        await SolutionFileModifier.RemoveProjectFromSolutionFileAsync(moduleSolutionFile, projectName);

        var projectFolderPath = Path.Combine(Path.GetDirectoryName(moduleSolutionFile), "src", projectName);
        if (Directory.Exists(projectFolderPath))
        {
            Directory.Delete(projectFolderPath, true);
        }
        else
        {
            projectFolderPath = Path.Combine(Path.GetDirectoryName(moduleSolutionFile), "test", projectName);
            if (Directory.Exists(projectFolderPath))
            {
                Directory.Delete(projectFolderPath, true);
            }
        }
    }

    private void ChangeDomainTestReferenceToMongoDB(ModuleWithMastersInfo module, string moduleSolutionFile)
    {
        var testPath = Path.Combine(Path.GetDirectoryName(moduleSolutionFile), "test");

        if (!Directory.Exists(testPath))
        {
            return;
        }

        var projectFolderPath = Directory.GetDirectories(testPath).FirstOrDefault(d => d.EndsWith("Domain.Tests"));

        if (projectFolderPath == null)
        {
            return;
        }

        var csprojFile = Directory.GetFiles(projectFolderPath, "*.csproj", SearchOption.AllDirectories).FirstOrDefault();
        var moduleFile = Directory.GetFiles(projectFolderPath, "*DomainTestModule.cs", SearchOption.AllDirectories).FirstOrDefault();

        if (csprojFile == null || moduleFile == null)
        {
            return;
        }

        File.WriteAllText(csprojFile, File.ReadAllText(csprojFile).Replace("EntityFrameworkCore", "MongoDB"));
        File.WriteAllText(moduleFile, File.ReadAllText(moduleFile)
            .Replace(".EntityFrameworkCore;", ".MongoDB;")
            .Replace("EntityFrameworkCoreTestModule", "MongoDbTestModule"));
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
                await ProjectNpmPackageAdder.AddAngularPackageAsync(angularPath, npmPackage);
            }
        }
    }

    private async Task AddAngularSourceCode(string modulesFolderInSolution, string solutionFilePath, string moduleName, bool newTemplate)
    {
        var angularPath = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(solutionFilePath)), "angular");

        if (!Directory.Exists(angularPath))
        {
            DeleteAngularDirectoriesInModulesFolder(modulesFolderInSolution);
            return;
        }

        if (newTemplate)
        {
            MoveAngularFolderInNewTemplate(modulesFolderInSolution, moduleName);
        }

        await AngularSourceCodeAdder.AddFromModuleAsync(solutionFilePath, angularPath);
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

    private static void MoveAngularFolderInNewTemplate(string modulesFolderInSolution, string moduleName)
    {
        var moduleAngularFolder = Path.Combine(modulesFolderInSolution, moduleName, "angular");

        if (!Directory.Exists(moduleAngularFolder))
        {
            return;
        }

        var files = Directory.GetFiles(moduleAngularFolder);
        var folders = Directory.GetDirectories(moduleAngularFolder);

        Directory.CreateDirectory(Path.Combine(moduleAngularFolder, moduleName));

        foreach (var file in files)
        {
            File.Move(file, Path.Combine(moduleAngularFolder, moduleName, Path.GetFileName(file)));
        }
        foreach (var folder in folders)
        {
            Directory.Move(folder, Path.Combine(moduleAngularFolder, moduleName, Path.GetFileName(folder)));
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
            await SourceCodeDownloadService.DownloadModuleAsync(
                module.Name,
                targetModuleFolder,
                version,
                null,
                null,
                null
            );
        }

        await DeleteRedundantHostProjects(targetModuleFolder, "app");
        await DeleteRedundantHostProjects(targetModuleFolder, "demo");
        await DeleteRedundantHostProjects(targetModuleFolder, "host");

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
        var webPackagesWillBeAddedToBlazorServerProject = SouldWebPackagesBeAddedToBlazorServerProject(module, projectFiles);

        foreach (var nugetPackage in module.NugetPackages)
        {
            var isProjectTiered = await IsProjectTiered(projectFiles);

            var nugetTarget =
                isProjectTiered && nugetPackage.TieredTarget != NuGetPackageTarget.Undefined
                    ? nugetPackage.TieredTarget
                    : nugetPackage.Target;

            if (webPackagesWillBeAddedToBlazorServerProject)
            {
                if ( nugetTarget == NuGetPackageTarget.Web)
                {
                    nugetTarget = NuGetPackageTarget.BlazorServer;
                }
                else if (!isProjectTiered && nugetTarget == NuGetPackageTarget.SignalR)
                {
                    nugetTarget = NuGetPackageTarget.BlazorServer;
                }
            }

            var targetProjectFile = ProjectFinder.FindNuGetTargetProjectFile(projectFiles, nugetTarget);
            if (targetProjectFile == null)
            {
                Logger.LogDebug($"Target project is not available for this NuGet package '{nugetPackage.Name}'");
                continue;
            }

            await ProjectNugetPackageAdder.AddAsync(null, targetProjectFile, nugetPackage, null, useDotnetCliToInstall);
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
                        await ProjectNpmPackageAdder.AddMvcPackageAsync(Path.GetDirectoryName(targetProject), npmPackage);
                    }
                }
            }
            else
            {
                Logger.LogDebug("Target project is not available for NPM packages.");
            }
        }
    }

    private static bool SouldWebPackagesBeAddedToBlazorServerProject(ModuleWithMastersInfo module, string[] projectFiles)
    {
        var blazorProject = projectFiles.FirstOrDefault(p => p.EndsWith(".Blazor.csproj"));

        if (blazorProject == null)
        {
            return false;
        }

        var isBlazorServerProject = BlazorProjectTypeChecker.IsBlazorServerProject(blazorProject);
        return isBlazorServerProject && module.NugetPackages.All(np => np.Target != NuGetPackageTarget.BlazorServer && np.TieredTarget != NuGetPackageTarget.BlazorServer);
    }

    protected void ModifyDbContext(string[] projectFiles, ModuleInfo module, bool skipDbMigrations = false)
    {
        if (string.IsNullOrWhiteSpace(module.EfCoreConfigureMethodName))
        {
            if (!skipDbMigrations)
            {
                RunMigrator(projectFiles);
            }

            return;
        }

        var dbMigrationsProject = projectFiles.FirstOrDefault(p => p.EndsWith(".DbMigrations.csproj"))
            ?? projectFiles.FirstOrDefault(p => p.EndsWith(".EntityFrameworkCore.csproj")) ;

        if (dbMigrationsProject == null)
        {
            Logger.LogDebug("Solution doesn't have a Migrations project.");

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
                EfCoreMigrationManager.AddMigration(dbMigrationsProject, module.Name);
            }

            RunMigrator(projectFiles);
        }
    }

    protected virtual void RunMigrator(string[] projectFiles)
    {
        var dbMigratorProject = projectFiles.FirstOrDefault(p => p.EndsWith(".DbMigrator.csproj"));

        if (!string.IsNullOrEmpty(dbMigratorProject))
        {
            CmdHelper.RunCmd("cd \"" + Path.GetDirectoryName(dbMigratorProject) + "\" && dotnet run", out int exitCode);
        }
    }

    protected virtual async Task<ModuleWithMastersInfo> GetModuleInfoAsync(string moduleName, bool newTemplate,
        bool newProTemplate = false)
    {
        if (newTemplate || newProTemplate)
        {
            return GetEmptyModuleProjectInfo(moduleName, newProTemplate);
        }

        var url = $"{CliUrls.WwwAbpIo}api/app/module/byNameWithDetails/?name=" + moduleName;
        var client = _cliHttpClientFactory.CreateClient();

        using (var response = await client.GetAsync(url, _cliHttpClientFactory.GetCancellationToken()))
        {
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

    private ModuleWithMastersInfo GetEmptyModuleProjectInfo(string moduleName,
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
                Name = $"{module.Name}.Blazor.WebAssembly",
                ModuleClass = $"{module.Name}.Blazor.{moduleProjectName}BlazorWebAssemblyModule",
                Target = NuGetPackageTarget.BlazorWebAssembly
            },
            new NugetPackageInfo
            {
                Name = $"{module.Name}.Blazor.Server",
                ModuleClass = $"{module.Name}.Blazor.{moduleProjectName}BlazorServerModule",
                Target = NuGetPackageTarget.BlazorServer
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
            .Any(p => p.EndsWith(".HttpApi.Host"))
            && projectFiles.Select(ProjectFileNameHelper.GetAssemblyNameFromProjectPath)
            .Any(p => p.EndsWith(".IdentityServer"));
    }
}
