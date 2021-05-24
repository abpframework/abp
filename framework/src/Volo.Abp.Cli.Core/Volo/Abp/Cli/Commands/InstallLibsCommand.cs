using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.LIbs;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands
{
    public class InstallLibsCommand : IConsoleCommand, ITransientDependency
    {
        public ILogger<InstallLibsCommand> Logger { get; set; }

        protected IInstallLibsService InstallLibsService { get; }

        public InstallLibsCommand(IInstallLibsService installLibsService)
        {
            InstallLibsService = installLibsService;
            Logger = NullLogger<InstallLibsCommand>.Instance;
        }

        public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            var workingDirectoryArg = commandLineArgs.Options.GetOrNull(
                Options.WorkingDirectory.Short,
                Options.WorkingDirectory.Long
            );

            var workingDirectory = workingDirectoryArg ?? Directory.GetCurrentDirectory();

            if (!Directory.Exists(workingDirectory))
            {
                throw new CliUsageException(
                    "Specified directory does not exist." +
                    Environment.NewLine + Environment.NewLine +
                    GetUsageInfo()
                );
            }

            await InstallLibsService.InstallLibs(workingDirectory);
        }

        public string GetUsageInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine("");
            sb.AppendLine("Usage:");
            sb.AppendLine("");
            sb.AppendLine("  abp install-libs");
            sb.AppendLine("");
            sb.AppendLine("Options:");
            sb.AppendLine("");
            sb.AppendLine("-wd|--working-directory <directory-path>                (default: empty)");
            sb.AppendLine("");
            sb.AppendLine("See the documentation for more info: https://docs.abp.io/en/abp/latest/CLI");

            return sb.ToString();
        }

        public string GetShortDescription()
        {
            return "Bundles all third party styles and scripts required by modules and updates index.html file.";
        }

        public static class Options
        {
            public static class WorkingDirectory
            {
                public const string Short = "wd";
                public const string Long = "working-directory";
            }
        }
    }
}
