using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Cli.Commands;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.ProjectModification
{
    public class ModuleAdder : ITransientDependency
    {
        public ILogger<ModuleAdder> Logger { get; set; }

        protected ProjectNugetPackageAdder ProjectNugetPackageAdder { get; }
        protected SolutionModuleAdder SolutionModuleAdder { get; }

        public ModuleAdder(ProjectNugetPackageAdder projectNugetPackageAdder, SolutionModuleAdder solutionModuleAdder)
        {
            ProjectNugetPackageAdder = projectNugetPackageAdder;
            SolutionModuleAdder = solutionModuleAdder;

            Logger = NullLogger<ModuleAdder>.Instance;
        }

        public async Task AddModuleAsync(AddModuleArgs args)
        {
            ValidateArgs(args);
            NormalizeArgs(args);

            if (args.ProjectFile.IsNullOrEmpty() && args.SolutionFile.IsNullOrEmpty())
            {
                if (!TryToFillProjectOrSolutionName(args))
                {
                    Logger.LogWarning("Could not find any project (.csproj) or solution (.sln) file in the current directory!");
                    AddCommandHelper.WriteUsage(Logger);
                    return;
                }
            }

            if (!args.ProjectFile.IsNullOrEmpty())
            {
                await ProjectNugetPackageAdder.AddAsync(args.ProjectFile, args.ModuleName);
            }

            if (!args.SolutionFile.IsNullOrEmpty())
            {
                await SolutionModuleAdder.AddAsync(args.SolutionFile, args.ModuleName);
            }
        }

        private bool TryToFillProjectOrSolutionName(AddModuleArgs args)
        {
            var projectFiles = FindFiles(Directory.GetCurrentDirectory(), ".csproj");
            if (projectFiles.Length == 1)
            {
                args.ProjectFile = projectFiles.First();
                return true;
            }

            if (projectFiles.Length > 1)
            {
                Logger.LogWarning(
                    "There are multiple project (.csproj) files in the current directory. Please specify one of the files below:");
                foreach (var projectFile in projectFiles)
                {
                    Logger.LogWarning("* " + projectFile);
                }

                Logger.LogWarning("Example:");
                Logger.LogWarning($"abp add {args.ModuleName} -p {projectFiles[0]}");
                return false;
            }

            var solutionFiles = FindFiles(Directory.GetCurrentDirectory(), ".sln");
            if (solutionFiles.Length == 1)
            {
                args.SolutionFile = solutionFiles.First();
                return true;
            }

            if (solutionFiles.Length > 1)
            {
                Logger.LogWarning(
                    "There are multiple solution (.sln) files in the current directory. Please specify one of the files below:");
                foreach (var solutionFile in solutionFiles)
                {
                    Logger.LogWarning("* " + solutionFile);
                }

                Logger.LogWarning("Example:");
                Logger.LogWarning($"abp add {args.ModuleName} -s {solutionFiles[0]}");
            }

            return false;
        }

        private void ValidateArgs(AddModuleArgs args)
        {
            if (!args.ProjectFile.IsNullOrEmpty() &&
                !args.SolutionFile.IsNullOrEmpty())
            {
                throw new ArgumentException("Can not specify a solution name and project name together.");
            }
        }

        private void NormalizeArgs(AddModuleArgs args)
        {
            args.ProjectFile = NormalizePath(args.ProjectFile);
            args.SolutionFile = NormalizePath(args.SolutionFile);
        }

        private static string NormalizePath([CanBeNull] string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }

            if (Path.IsPathRooted(path))
            {
                return path;
            }

            return Path.Combine(Directory.GetCurrentDirectory(), path);
        }


        private string[] FindFiles(string directory, string fileExtension)
        {
            return Directory.GetFiles(directory, "*" + fileExtension);
        }
    }
}
