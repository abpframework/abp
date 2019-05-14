using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace Volo.Abp.Cli.ProjectModification
{
    public class SolutionModuleAdder : ITransientDependency
    {
        public ILogger<SolutionModuleAdder> Logger { get; set; }

        protected IJsonSerializer JsonSerializer { get; }
        protected ProjectNugetPackageAdder ProjectNugetPackageAdder { get; }

        public SolutionModuleAdder(
            IJsonSerializer jsonSerializer, 
            ProjectNugetPackageAdder projectNugetPackageAdder)
        {
            JsonSerializer = jsonSerializer;
            ProjectNugetPackageAdder = projectNugetPackageAdder;
            Logger = NullLogger<SolutionModuleAdder>.Instance;
        }

        public virtual async Task AddAsync(
            [NotNull] string solutionFile, 
            [NotNull] string moduleName)
        {
            Check.NotNull(solutionFile, nameof(solutionFile));
            Check.NotNull(moduleName, nameof(moduleName));

            var module = await FindModuleInfoAsync(moduleName);
            var projectFiles = GetProjectFiles(solutionFile);

            foreach (var nugetPackage in module.NugetPackages)
            {
                var targetProjectFile = FindTargetProjectFile(projectFiles, nugetPackage.Target);
                if (targetProjectFile == null)
                {
                    continue;
                }

                await ProjectNugetPackageAdder.AddAsync(targetProjectFile, nugetPackage);
            }
        }

        private string FindTargetProjectFile(string[] projectFiles, NuGetPackageTarget target)
        {
            if (!projectFiles.Any())
            {
                return null;
            }

            var assemblyNames = projectFiles
                .Select(
                    filePath => filePath
                        .Replace("\\", "/")
                        .Substring(filePath.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1)
                        .RemovePostFix(StringComparison.OrdinalIgnoreCase, ".csproj")
                ).ToArray();

            switch (target)
            {
                case NuGetPackageTarget.Web:
                    return FindProjectEndsWith(assemblyNames, ".Web");
                case NuGetPackageTarget.EntityFrameworkCore:
                    return FindProjectEndsWith(assemblyNames, ".EntityFrameworkCore") ??
                           FindProjectEndsWith(assemblyNames, ".Web");
                case NuGetPackageTarget.MongoDB:
                    return FindProjectEndsWith(assemblyNames, ".MongoDB") ??
                           FindProjectEndsWith(assemblyNames, ".Web");
                case NuGetPackageTarget.Application:
                    return FindProjectEndsWith(assemblyNames, ".Application") ??
                           FindProjectEndsWith(assemblyNames, ".Web");
                case NuGetPackageTarget.ApplicationContracts:
                    return FindProjectEndsWith(assemblyNames, ".Application.Contracts");
                case NuGetPackageTarget.Domain:
                    return FindProjectEndsWith(assemblyNames, ".Domain") ??
                           FindProjectEndsWith(assemblyNames, ".Application") ??
                           FindProjectEndsWith(assemblyNames, ".Web");
                case NuGetPackageTarget.DomainShared:
                    return FindProjectEndsWith(assemblyNames, ".Domain.Shared");
                case NuGetPackageTarget.HttpApi:
                    return FindProjectEndsWith(assemblyNames, ".HttpApi") ??
                           FindProjectEndsWith(assemblyNames, ".Web");
                case NuGetPackageTarget.HttpApiClient:
                    return FindProjectEndsWith(assemblyNames, ".HttpApi.Client");
                default:
                    throw new ApplicationException($"{nameof(NuGetPackageTarget)} has not implemented: {target}");
            }
        }

        private static string FindProjectEndsWith(string[] projectAssemblyNames, string postfix)
        {
            return projectAssemblyNames.FirstOrDefault(p => p.EndsWith(postfix, StringComparison.OrdinalIgnoreCase));
        }

        protected virtual string[] GetProjectFiles(string solutionFile)
        {
            var baseProjectFolder = GetBaseProjectFolder(solutionFile);
            return Directory.GetFiles(baseProjectFolder, "*.csproj", SearchOption.AllDirectories);
        }

        protected virtual string GetBaseProjectFolder(string solutionFile)
        {
            var baseFolder = Path.GetDirectoryName(solutionFile);
            var srcFolder = Path.Combine(baseFolder, "src");
            if (Directory.Exists(srcFolder))
            {
                baseFolder = srcFolder;
            }

            return baseFolder;
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