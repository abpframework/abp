using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Volo.Abp.Cli.ProjectModification
{
    public static class ProjectFinder
    {
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
                default:
                    throw new ApplicationException($"{nameof(NuGetPackageTarget)}.{target} has not implemented!");
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
            var baseProjectFolder = GetBaseProjectFolder(solutionFile);
            return Directory.GetFiles(baseProjectFolder, "*.csproj", SearchOption.AllDirectories);
        }

        public static string[] GetAssemblyNames(string[] projectFiles)
        {
            return projectFiles.Select(GetAssemblyName).ToArray();
        }

        public static string GetAssemblyName(string projectFile)
        {
            return projectFile
                .Substring(projectFile.LastIndexOf(Path.DirectorySeparatorChar) + 1)
                .RemovePostFix(StringComparison.OrdinalIgnoreCase, ".csproj");
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

        private static string GetBaseProjectFolder(string solutionFile)
        {
            var baseFolder = Path.GetDirectoryName(solutionFile);
            var srcFolder = Path.Combine(baseFolder, "src");
            if (Directory.Exists(srcFolder))
            {
                baseFolder = srcFolder;
            }

            return baseFolder;
        }
    }
}
