using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Build;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands
{
    public class BuildCommand : IConsoleCommand, ITransientDependency
    {
        public ILogger<BuildCommand> Logger { get; set; }

        public IDotNetProjectDependencyFiller DotNetProjectDependencyFiller { get; set; }

        public IChangedProjectFinder ChangedProjectFinder { get; set; }

        public IDotNetProjectBuilder DotNetProjectBuilder { get; set; }

        public IRepositoryBuildStatusStore RepositoryBuildStatusStore { get; set; }

        public IDotNetProjectBuildConfigReader DotNetProjectBuildConfigReader { get; set; }

        public IBuildStatusGenerator BuildStatusGenerator { get; set; }

        public Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            var sw = new Stopwatch();
            sw.Start();

            var workingDirectory = commandLineArgs.Options.GetOrNull(
                Options.WorkingDirectory.Short,
                Options.WorkingDirectory.Long
            );

            var maxParallelBuild = commandLineArgs.Options.GetOrNull(
                Options.MaxParallelBuild.Short,
                Options.MaxParallelBuild.Long
            );

            var dotnetBuildArguments = commandLineArgs.Options.GetOrNull(
                Options.DotnetBuildArguments.Short,
                Options.DotnetBuildArguments.Long
            );

            var buildName = commandLineArgs.Options.GetOrNull(
                Options.BuildName.Short,
                Options.BuildName.Long
            );

            var forceBuild = commandLineArgs.Options.ContainsKey(Options.ForceBuild.Long);

            var buildConfig = DotNetProjectBuildConfigReader.Read(workingDirectory ?? Directory.GetCurrentDirectory());
            buildConfig.BuildName = buildName;
            buildConfig.ForceBuild = forceBuild;

            var changedProjectFiles = ChangedProjectFinder.Find(buildConfig);

            DotNetProjectDependencyFiller.Fill(changedProjectFiles);

            var sortedProjects = changedProjectFiles.SortByDependencies(p => p.Dependencies);

            var buildSucceededProjects = DotNetProjectBuilder.Build(
                sortedProjects,
                string.IsNullOrEmpty(maxParallelBuild) ? 1 : Convert.ToInt32(maxParallelBuild),
                dotnetBuildArguments ?? ""
            );

            var buildStatus = BuildStatusGenerator.Generate(
                buildConfig.GitRepository,
                changedProjectFiles,
                buildSucceededProjects
            );

            RepositoryBuildStatusStore.Set(buildName, buildStatus);

            sw.Stop();
            Console.WriteLine("Build operation is completed in " + sw.ElapsedMilliseconds + " (ms)");

            return Task.CompletedTask;
        }

        public string GetUsageInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine("");
            sb.AppendLine("Usage:");
            sb.AppendLine("");
            sb.AppendLine("  abp build [options]");
            sb.AppendLine("");
            sb.AppendLine("Options:");
            sb.AppendLine("");
            // TODO: Explain extra parameters
            sb.AppendLine("");
            sb.AppendLine("See the documentation for more info: https://docs.abp.io/en/abp/latest/CLI");

            return sb.ToString();
        }

        public string GetShortDescription()
        {
            return "Builds a dotnet repository and dependent repositories or a solution.";
        }

        public static class Options
        {
            public static class WorkingDirectory
            {
                public const string Short = "wd";
                public const string Long = "working-directory";
            }

            public static class MaxParallelBuild
            {
                public const string Short = "m";
                public const string Long = "max-paralel-builds";
            }

            public static class DotnetBuildArguments
            {
                public const string Short = "a";
                public const string Long = "dotnet-build-arguments";
            }

            public static class BuildName
            {
                public const string Short = "n";
                public const string Long = "build-name";
            }

            public static class ForceBuild
            {
                public const string Long = "force";
            }
        }
    }
}
