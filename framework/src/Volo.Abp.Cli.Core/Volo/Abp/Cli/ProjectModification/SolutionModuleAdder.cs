using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
        protected ProjectNpmPackageAdder ProjectNpmPackageAdder { get; }
        protected DerivedClassFinder DerivedClassFinder { get; }

        public SolutionModuleAdder(
            IJsonSerializer jsonSerializer,
            ProjectNugetPackageAdder projectNugetPackageAdder,
            DbContextFileBuilderConfigureAdder dbContextFileBuilderConfigureAdder,
            EfCoreMigrationAdder efCoreMigrationAdder,
            DerivedClassFinder derivedClassFinder,
            ProjectNpmPackageAdder projectNpmPackageAdder)
        {
            EfCoreMigrationAdder = efCoreMigrationAdder;
            DerivedClassFinder = derivedClassFinder;
            JsonSerializer = jsonSerializer;
            ProjectNugetPackageAdder = projectNugetPackageAdder;
            DbContextFileBuilderConfigureAdder = dbContextFileBuilderConfigureAdder;
            ProjectNpmPackageAdder = projectNpmPackageAdder;
            Logger = NullLogger<SolutionModuleAdder>.Instance;
        }

        public virtual async Task AddAsync(
            [NotNull] string solutionFile,
            [NotNull] string moduleName,
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
                    Logger.LogDebug($"Target project is not available for NuGet package '{nugetPackage.Name}'");
                    continue;
                }

                await ProjectNugetPackageAdder.AddAsync(targetProjectFile, nugetPackage);
            }

            if (!module.NpmPackages.IsNullOrEmpty())
            {
                var targetProjects = ProjectFinder.FindNpmTargetProjectFile(projectFiles);
                if (targetProjects.Any())
                {
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

            ModifyDbContext(projectFiles, module, skipDbMigrations);
        }

        protected void ModifyDbContext(string[] projectFiles, ModuleInfo module, bool skipDbMigrations = false)
        {
            if (string.IsNullOrWhiteSpace(module.EfCoreConfigureMethodName))
            {
                return;
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
                EfCoreMigrationAdder.AddMigration(dbMigrationsProject, module.Name); 
            }
        }

        protected virtual async Task<ModuleInfo> FindModuleInfoAsync(string moduleName)
        {
            using (var client = new HttpClient())
            {
                var url = "https://localhost:44328/api/app/module/byName/?name=" + moduleName;

                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new CliUsageException($"ERROR: '{moduleName}' module could not be found!");
                    }

                    throw new Exception($"ERROR: Remote server returns '{response.StatusCode}'");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ModuleInfo>(responseContent);
            }
        }
    }
}