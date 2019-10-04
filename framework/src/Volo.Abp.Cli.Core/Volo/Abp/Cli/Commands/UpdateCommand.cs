﻿using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.ProjectBuilding.Analyticses;
using Volo.Abp.Cli.ProjectModification;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.Threading;

namespace Volo.Abp.Cli.Commands
{
    public class UpdateCommand : IConsoleCommand, ITransientDependency
    {
        public ILogger<UpdateCommand> Logger { get; set; }

        private readonly VoloNugetPackagesVersionUpdater _nugetPackagesVersionUpdater;
        private readonly NpmPackagesUpdater _npmPackagesUpdater;
        private readonly ICliAnalyticsCollect _cliAnalyticsCollect;
        private readonly CliOptions _options;
        private readonly IJsonSerializer _jsonSerializer;

        public UpdateCommand(VoloNugetPackagesVersionUpdater nugetPackagesVersionUpdater,
            NpmPackagesUpdater npmPackagesUpdater,
            ICliAnalyticsCollect cliAnalyticsCollect, 
            IJsonSerializer jsonSerializer, 
            IOptions<CliOptions> options)
        {
            _nugetPackagesVersionUpdater = nugetPackagesVersionUpdater;
            _npmPackagesUpdater = npmPackagesUpdater;
            _cliAnalyticsCollect = cliAnalyticsCollect;
            _jsonSerializer = jsonSerializer;
            _options = options.Value;

            Logger = NullLogger<UpdateCommand>.Instance;
        }

        public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            await UpdateNugetPackages(commandLineArgs);
            UpdateNpmPackages();

            var options = commandLineArgs.Options
                .Select(x => x.Key).ToList();
            await _cliAnalyticsCollect.CollectAsync(new CliAnalyticsCollectInputDto
            {
                Tool = _options.ToolName,
                Command = commandLineArgs.Command,
                Options = _jsonSerializer.Serialize(options)
            });
        }

        private void UpdateNpmPackages()
        {
            _npmPackagesUpdater.Update(Directory.GetCurrentDirectory());
        }

        private async Task UpdateNugetPackages(CommandLineArgs commandLineArgs)
        {
            var includePreviews =
                commandLineArgs.Options.GetOrNull(Options.IncludePreviews.Short, Options.IncludePreviews.Long) != null;

            var solution = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.sln").FirstOrDefault();

            if (solution != null)
            {
                var solutionName = Path.GetFileName(solution).RemovePostFix(".sln");

                await _nugetPackagesVersionUpdater.UpdateSolutionAsync(solution, includePreviews);

                Logger.LogInformation($"Volo packages are updated in {solutionName} solution.");
                return;
            }

            var project = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csproj").FirstOrDefault();

            if (project != null)
            {
                var projectName = Path.GetFileName(project).RemovePostFix(".csproj");

                await _nugetPackagesVersionUpdater.UpdateProjectAsync(project, includePreviews);

                Logger.LogInformation($"Volo packages are updated in {projectName} project.");
                return;
            }

            throw new CliUsageException(
                "No solution or project found in this directory." +
                Environment.NewLine + Environment.NewLine +
                GetUsageInfo()
            );
        }

        public string GetUsageInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine("");
            sb.AppendLine("Usage:");
            sb.AppendLine("  abp update  [-p|--include-previews]");
            sb.AppendLine("");
            sb.AppendLine("Options:");
            sb.AppendLine("--include-previews                          (if supported by the template)");
            sb.AppendLine("");
            sb.AppendLine("Some examples:");
            sb.AppendLine("  abp update");
            sb.AppendLine("  abp update --include-previews");
            sb.AppendLine("");
            sb.AppendLine("See the documentation for more info.");

            return sb.ToString();
        }

        public string GetShortDescription()
        {
            return "Automatically updates all ABP related NuGet packages and NPM packages in a" +
                   " solution or project to the latest versions";
        }

        public static class Options
        {
            public static class IncludePreviews
            {
                public const string Short = "p";
                public const string Long = "include-previews";
            }
        }
    }
}
