using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
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

            var bundle = commandLineArgs.Options.ContainsKey(Options.Bundle.Short) ||
                             commandLineArgs.Options.ContainsKey(Options.Bundle.Long);

            var minify = commandLineArgs.Options.ContainsKey(Options.Minify.Short) ||
                             commandLineArgs.Options.ContainsKey(Options.Minify.Long);

            var name = commandLineArgs.Options.GetOrNull(
                Options.Name.Short,
                Options.Name.Long
            );

            if ((minify || bundle) && name.IsNullOrEmpty())
            {
                throw new CliUsageException(
                    "Please specify a bundle name." +
                    Environment.NewLine + Environment.NewLine +
                    GetUsageInfo()
                );
            }

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
                await BundlingService.BundleAsync(workingDirectory, forceBuild, bundle, minify, name);
            }
            catch (BundlingException ex)
            {
                Logger.LogError(ex.Message);
            }
        }

        public string GetShortDescription()
        {
            return "Bundles all third party styles and scripts required by modules and updates index.html file.";
        }

        public string GetUsageInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine("");
            sb.AppendLine("Usage:");
            sb.AppendLine("");
            sb.AppendLine("  abp bundle [options]");
            sb.AppendLine("");
            sb.AppendLine("Options:");
            sb.AppendLine("");
            sb.AppendLine("-wd|--working-directory <directory-path>                (default: empty)");
            sb.AppendLine("-f | --force                                            (default: false)");
            sb.AppendLine("-f | --force                                            (default: false)");
            sb.AppendLine("-b | --bundle                                           (default: false)");
            sb.AppendLine("-m | --minify                                           (default: false)");
            sb.AppendLine("-n | --name                                             (default: empty)");
            sb.AppendLine("");
            sb.AppendLine("See the documentation for more info: https://docs.abp.io/en/abp/latest/CLI");

            return sb.ToString();
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

            public static class Bundle
            {
                public const string Short = "b";
                public const string Long = "bundle";
            }

            public static class Minify
            {
                public const string Short = "m";
                public const string Long = "minify";
            }

            public static class Name
            {
                public const string Short = "n";
                public const string Long = "name";
            }
        }
    }
}
