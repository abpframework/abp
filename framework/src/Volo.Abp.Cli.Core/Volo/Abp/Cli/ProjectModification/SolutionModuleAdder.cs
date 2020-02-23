using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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

        public SolutionModuleAdder(
            IJsonSerializer jsonSerializer,
            ProjectNugetPackageAdder projectNugetPackageAdder,
            DbContextFileBuilderConfigureAdder dbContextFileBuilderConfigureAdder,
            EfCoreMigrationAdder efCoreMigrationAdder,
            DerivedClassFinder derivedClassFinder,
            ProjectNpmPackageAdder projectNpmPackageAdder,
            NpmGlobalPackagesChecker npmGlobalPackagesChecker, 
            IRemoteServiceExceptionHandler remoteServiceExceptionHandler)
        {
            JsonSerializer = jsonSerializer;
            ProjectNugetPackageAdder = projectNugetPackageAdder;
            DbContextFileBuilderConfigureAdder = dbContextFileBuilderConfigureAdder;
            EfCoreMigrationAdder = efCoreMigrationAdder;
            DerivedClassFinder = derivedClassFinder;
            ProjectNpmPackageAdder = projectNpmPackageAdder;
            NpmGlobalPackagesChecker = npmGlobalPackagesChecker;
            RemoteServiceExceptionHandler = remoteServiceExceptionHandler;
            Logger = NullLogger<SolutionModuleAdder>.Instance;
        }

        public virtual async Task AddAsync(
            [NotNull] string solutionFile,
            [NotNull] string moduleName,
            string startupProject,
            bool skipDbMigrations = false)
        {
            Check.NotNull(solutionFile, nameof(solutionFile));
            Check.NotNull(moduleName, nameof(moduleName));

            var module = await FindModuleInfoAsync(moduleName);

            Logger.LogInformation($"Installing module '{module.Name}' to the solution '{Path.GetFileNameWithoutExtension(solutionFile)}'");

            var projectFiles = ProjectFinder.GetProjectFiles(solutionFile);

            foreach (var nugetPackage in module.NugetPackages)
            {
                var targetProjectFile = ProjectFinder.FindNuGetTargetProjectFile(projectFiles, nugetPackage.Target);
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
                        foreach (var npmPackage in module.NpmPackages.Where(p => p.ApplicationType.HasFlag(NpmApplicationType.Mvc)))
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

            ModifyDbContext(projectFiles, module, startupProject, skipDbMigrations);
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

            DbContextFileBuilderConfigureAdder.Add(dbContextFile, module.EfCoreConfigureMethodName);


            if (!skipDbMigrations)
            {
                EfCoreMigrationAdder.AddMigration(dbMigrationsProject, module.Name, startupProject); 
            }
        }

        protected virtual async Task<ModuleInfo> FindModuleInfoAsync(string moduleName)
        {
            using (var client = new CliHttpClient())
            {
                var url = $"{CliUrls.WwwAbpIo}api/app/module/byName/?name=" + moduleName;

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
                return JsonSerializer.Deserialize<ModuleInfo>(responseContent);
            }
        }
    }
}