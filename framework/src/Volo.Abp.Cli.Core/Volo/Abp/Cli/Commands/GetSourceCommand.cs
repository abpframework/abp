using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands
{
    public class GetSourceCommand : IConsoleCommand, ITransientDependency
    {
        public ModuleProjectBuilder ModuleProjectBuilder { get; }

        public ILogger<NewCommand> Logger { get; set; }

        public GetSourceCommand(ModuleProjectBuilder moduleProjectBuilder)
        {
            ModuleProjectBuilder = moduleProjectBuilder;
            Logger = NullLogger<NewCommand>.Instance;
        }

        public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            if (commandLineArgs.Target == null)
            {
                throw new CliUsageException(
                    "Module name is missing!" +
                    Environment.NewLine + Environment.NewLine +
                    GetUsageInfo()
                );
            }

            Logger.LogInformation("Downloading source code of " + commandLineArgs.Target);

            var version = commandLineArgs.Options.GetOrNull(Options.Version.Short, Options.Version.Long);
            if (version != null)
            {
                Logger.LogInformation("Version: " + version);
            }

            var outputFolder = commandLineArgs.Options.GetOrNull(Options.OutputFolder.Short, Options.OutputFolder.Long);
            if (outputFolder != null)
            {
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }

                outputFolder = Path.GetFullPath(outputFolder);
            }
            else
            {
                outputFolder = Directory.GetCurrentDirectory();
            }

            Logger.LogInformation("Output folder: " + outputFolder);

            var gitHubLocalRepositoryPath = commandLineArgs.Options.GetOrNull(Options.GitHubLocalRepositoryPath.Long);
            if (gitHubLocalRepositoryPath != null)
            {
                Logger.LogInformation("GitHub Local Repository Path: " + gitHubLocalRepositoryPath);
            }

            commandLineArgs.Options.Add(CliConsts.Command, commandLineArgs.Command);

            var result = await ModuleProjectBuilder.BuildAsync(
                new ProjectBuildArgs(
                    SolutionName.Parse(commandLineArgs.Target),
                    commandLineArgs.Target,
                    version,
                    DatabaseProvider.NotSpecified,
                    UiFramework.NotSpecified,
                    gitHubLocalRepositoryPath,
                    commandLineArgs.Options
                )
            );

            using (var templateFileStream = new MemoryStream(result.ZipContent))
            {
                using (var zipInputStream = new ZipInputStream(templateFileStream))
                {
                    var zipEntry = zipInputStream.GetNextEntry();
                    while (zipEntry != null)
                    {
                        var fullZipToPath = Path.Combine(outputFolder, zipEntry.Name);
                        var directoryName = Path.GetDirectoryName(fullZipToPath);

                        if (!string.IsNullOrEmpty(directoryName))
                        {
                            Directory.CreateDirectory(directoryName);
                        }

                        var fileName = Path.GetFileName(fullZipToPath);
                        if (fileName.Length == 0)
                        {
                            zipEntry = zipInputStream.GetNextEntry();
                            continue;
                        }

                        var buffer = new byte[4096]; // 4K is optimum
                        using (var streamWriter = File.Create(fullZipToPath))
                        {
                            StreamUtils.Copy(zipInputStream, streamWriter, buffer);
                        }

                        zipEntry = zipInputStream.GetNextEntry();
                    }
                }
            }

            Logger.LogInformation($"'{commandLineArgs.Target}' has been successfully downloaded to '{outputFolder}'");
        }

        public string GetUsageInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine("");
            sb.AppendLine("Usage:");
            sb.AppendLine("");
            sb.AppendLine("  abp get-source <module-name> [options]");
            sb.AppendLine("");
            sb.AppendLine("Options:");
            sb.AppendLine("");
            sb.AppendLine("-o|--output-folder <output-folder>          (default: current folder)");
            sb.AppendLine("-v|--version <version>                      (default: latest version)");
            sb.AppendLine("");
            sb.AppendLine("Examples:");
            sb.AppendLine("");
            sb.AppendLine("  abp get-source Volo.Blogging");
            sb.AppendLine("  abp get-source Volo.Blogging -o d:\\my-project");
            sb.AppendLine("");
            sb.AppendLine("See the documentation for more info: https://docs.abp.io/en/abp/latest/CLI");

            return sb.ToString();
        }

        public string GetShortDescription()
        {
            return "Downloads the source code of the specified module.";
        }

        public static class Options
        {
            public static class OutputFolder
            {
                public const string Short = "o";
                public const string Long = "output-folder";
            }

            public static class GitHubLocalRepositoryPath
            {
                public const string Long = "abp-path";
            }

            public static class Version
            {
                public const string Short = "v";
                public const string Long = "version";
            }
        }
    }
}
