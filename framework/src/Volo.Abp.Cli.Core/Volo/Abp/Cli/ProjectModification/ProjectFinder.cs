using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;

namespace Volo.Abp.Cli.ProjectModification
{
    public static class ProjectFinder
    {
        [CanBeNull]
        public static string FindNuGetTargetProjectFile(string[] projectFiles, NuGetPackageTarget target)
        {
            if (!projectFiles.Any())
            {
                return null;
            }

            var assemblyNames = GetAssemblyNames(projectFiles);

            switch (target)
            {
                case NuGetPackageTarget.Web:
                    return FindProjectEndsWith(projectFiles, assemblyNames, ".Web");
                case NuGetPackageTarget.IdentityServer:
                    return FindProjectEndsWith(projectFiles, assemblyNames, ".IdentityServer");
                case NuGetPackageTarget.EntityFrameworkCore:
                    return FindProjectEndsWith(projectFiles, assemblyNames, ".EntityFrameworkCore");
                case NuGetPackageTarget.MongoDB:
                    return FindProjectEndsWith(projectFiles, assemblyNames, ".MongoDB");
                case NuGetPackageTarget.Application:
                    return FindProjectEndsWith(projectFiles, assemblyNames, ".Application") ??
                           FindProjectEndsWith(projectFiles, assemblyNames, ".Web");
                case NuGetPackageTarget.ApplicationContracts:
                    return FindProjectEndsWith(projectFiles, assemblyNames, ".Application.Contracts");
                case NuGetPackageTarget.Domain:
                    return FindProjectEndsWith(projectFiles, assemblyNames, ".Domain") ??
                           FindProjectEndsWith(projectFiles, assemblyNames, ".Application") ??
                           FindProjectEndsWith(projectFiles, assemblyNames, ".Web");
                case NuGetPackageTarget.DomainShared:
                    return FindProjectEndsWith(projectFiles, assemblyNames, ".Domain.Shared");
                case NuGetPackageTarget.HttpApi:
                    return FindProjectEndsWith(projectFiles, assemblyNames, ".HttpApi");
                case NuGetPackageTarget.HttpApiClient:
                    return FindProjectEndsWith(projectFiles, assemblyNames, ".HttpApi.Client");
                case NuGetPackageTarget.SignalR:
                    return FindProjectEndsWith(projectFiles, assemblyNames, ".SignalR") ??
                           FindProjectEndsWith(projectFiles, assemblyNames, ".Web") ??
                           FindProjectEndsWith(projectFiles, assemblyNames, ".HttpApi.Host");
                case NuGetPackageTarget.Blazor:
                    return FindProjectEndsWith(projectFiles, assemblyNames, ".Blazor");
                case NuGetPackageTarget.BlazorWebAssembly:
                    var BlazorWebAssemblyTargetProject = FindProjectEndsWith(projectFiles, assemblyNames, ".Blazor");
                    return !BlazorProjectTypeChecker.IsBlazorServerProject(BlazorWebAssemblyTargetProject) ? BlazorWebAssemblyTargetProject : null;
                case NuGetPackageTarget.BlazorServer:
                    var BlazorServerTargetProject = FindProjectEndsWith(projectFiles, assemblyNames, ".Blazor");
                    return BlazorProjectTypeChecker.IsBlazorServerProject(BlazorServerTargetProject) ? BlazorServerTargetProject : null;
                default:
                    return null;
            }
        }

        public static string[] FindNpmTargetProjectFile(string[] projectFiles)
        {
            if (!projectFiles.Any())
            {
                return Array.Empty<string>();
            }

            var assemblyNames = GetAssemblyNames(projectFiles);

            var projects = new List<string>();

            var project = FindProjectEndsWith(projectFiles, assemblyNames, ".Web.Host");
            if (project != null)
            {
                projects.Add(project);
            }

            project = FindProjectEndsWith(projectFiles, assemblyNames, ".Host", ".HttpApi.Host");
            if (project != null)
            {
                projects.Add(project);
            }

            if (projects.Any())
            {
                return projects.ToArray();
            }

            project = FindProjectEndsWith(projectFiles, assemblyNames, ".Web");
            if (project != null)
            {
                projects.Add(project);
            }

            return projects.ToArray();
        }

        public static string[] GetProjectFiles(string solutionFile)
        {
            return GetBaseProjectFolders(solutionFile)
                .Select(baseProjectFolder =>
                    Directory.GetFiles(baseProjectFolder, "*.csproj", SearchOption.AllDirectories))
                .SelectMany(files => files)
                .ToArray();
        }

        public static string[] GetAssemblyNames(string[] projectFiles)
        {
            return projectFiles.Select(ProjectFileNameHelper.GetAssemblyNameFromProjectPath).ToArray();
        }

        private static string FindProjectEndsWith(
            string[] projectFiles,
            string[] assemblyNames,
            string postfix,
            string excludePostfix = null)
        {
            for (var i = 0; i < assemblyNames.Length; i++)
            {
                var assemblyName = assemblyNames[i];
                if(assemblyName.EndsWith(postfix, StringComparison.OrdinalIgnoreCase) &&
                   (excludePostfix == null || !assemblyName.EndsWith(excludePostfix)))
                {
                    return projectFiles[i];
                }
            }

            return null;
        }

        private static string[] GetBaseProjectFolders(string solutionFile)
        {
            var projectFolders = new List<string>();
            var baseFolder = Path.GetDirectoryName(solutionFile);
            if (baseFolder == null)
            {
                return projectFolders.ToArray();
            }

            var hostFolder = Path.Combine(baseFolder, "host");
            if (Directory.Exists(hostFolder))
            {
                projectFolders.Add(hostFolder);
            }

            var srcFolder = Path.Combine(baseFolder, "src");
            if (Directory.Exists(srcFolder))
            {
                projectFolders.Add(srcFolder);
            }

            var testFolder = Path.Combine(baseFolder, "test");
            if (Directory.Exists(testFolder))
            {
                projectFolders.Add(testFolder);
            }

            if (!projectFolders.Any())
            {
                projectFolders.Add(baseFolder);
            }

            return projectFolders.ToArray();
        }
    }
}
