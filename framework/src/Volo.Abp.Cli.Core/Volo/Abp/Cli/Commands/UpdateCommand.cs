using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.ProjectBuilding.Github;
using Volo.Abp.Cli.ProjectModification;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands
{
    public class UpdateCommand : IConsoleCommand, ITransientDependency
    {
        public ILogger<UpdateCommand> Logger { get; set; }

        private readonly VoloPackagesVersionUpdater _packagesVersionUpdater;

        public UpdateCommand(VoloPackagesVersionUpdater packagesVersionUpdater)
        {
            _packagesVersionUpdater = packagesVersionUpdater;

            Logger = NullLogger<UpdateCommand>.Instance;
        }

        public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            var includePreviews = commandLineArgs.Options.GetOrNull(Options.IncludePreviews.Short, Options.IncludePreviews.Long) != null;

            var solution = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.sln").FirstOrDefault();

            if (solution != null)
            {
                var version = await GetLatestAbpVersion(includePreviews);
                var solutionName = Path.GetFileName(solution).RemovePostFix(".sln");

                _packagesVersionUpdater.UpdateSolution(solution, version);

                Logger.LogInformation($"Volo packages are updated to {version} in {solutionName} solution.");
                return;
            }

            var project = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csproj").FirstOrDefault();

            if (project != null)
            {
                var version = await GetLatestAbpVersion(includePreviews);
                var projectName = Path.GetFileName(project).RemovePostFix(".csproj");

                _packagesVersionUpdater.UpdateProject(project, version);

                Logger.LogInformation($"Volo packages are updated to {version} in {projectName} project.");
                return;
            }

            Logger.LogError("No solution or project found in this directory.");
        }

        protected virtual async Task<string> GetLatestAbpVersion(bool includePreviews)
        {
            var url = "https://api.github.com/repos/abpframework/abp/releases";

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(30);
                client.DefaultRequestHeaders.UserAgent.ParseAdd("MyAgent/1.0");
                var result = await client.GetStringAsync(url);

                var releases = JsonConvert.DeserializeObject<List<GithubRelease>>(result);

                if (!includePreviews)
                {
                    releases.RemoveAll(v => v.IsPrerelease);
                }

                return releases.First().Name.RemovePreFix("v");
            }
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
