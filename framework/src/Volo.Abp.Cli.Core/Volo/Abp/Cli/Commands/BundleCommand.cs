using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Bundling;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands
{
    public class BundleCommand : IConsoleCommand, ITransientDependency
    {
        public ILogger<BundleCommand> Logger { get; set; }

        public IBundlingService BundlingService { get; set; }


        public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            var workingDirectoryArg = commandLineArgs.Options.GetOrNull(
                Options.WorkingDirectory.Short,
                Options.WorkingDirectory.Long
            );

            var workingDirectory = workingDirectoryArg ?? Directory.GetCurrentDirectory();

            var forceBuild = commandLineArgs.Options.ContainsKey(Options.ForceBuild.Short) ||
                             commandLineArgs.Options.ContainsKey(Options.ForceBuild.Long);

            if (!Directory.Exists(workingDirectory))
            {
                throw new CliUsageException(
                    "Specified directory does not exist." +
                    Environment.NewLine + Environment.NewLine +
                    GetUsageInfo()
                );
            }

            try
            {
                await BundlingService.BundleAsync(workingDirectory, forceBuild);
            }
            catch (BundlingException ex)
            {
                Logger.LogError(ex.Message);
            }
        }

        public string GetShortDescription()
        {
            throw new NotImplementedException();
        }

        public string GetUsageInfo()
        {
            throw new NotImplementedException();
        }

        public static class Options
        {
            public static class WorkingDirectory
            {
                public const string Short = "wd";
                public const string Long = "working-directory";
            }

            public static class ForceBuild
            {
                public const string Short = "f";
                public const string Long = "force";
            }
        }
    }
}
