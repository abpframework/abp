using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Commands;
using Volo.Abp.Cli.NuGet;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli
{
    public class CliService : ITransientDependency
    {
        public ILogger<CliService> Logger { get; set; }
        protected ICommandLineArgumentParser CommandLineArgumentParser { get; }
        protected ICommandSelector CommandSelector { get; }
        protected IServiceScopeFactory ServiceScopeFactory { get; }
        protected NuGetService NuGetService { get; }

        public CliService(
            ICommandLineArgumentParser commandLineArgumentParser,
            ICommandSelector commandSelector,
            IServiceScopeFactory serviceScopeFactory,
            NuGetService nugetService)
        {
            CommandLineArgumentParser = commandLineArgumentParser;
            CommandSelector = commandSelector;
            ServiceScopeFactory = serviceScopeFactory;
            NuGetService = nugetService;

            Logger = NullLogger<CliService>.Instance;
        }

        public async Task RunAsync(string[] args)
        {
            Logger.LogInformation("ABP CLI (https://abp.io)");
            
            var commandLineArgs = CommandLineArgumentParser.Parse(args);

#if !DEBUG
            if (!commandLineArgs.Options.ContainsKey("skip-cli-version-check"))
            {
                await CheckCliVersionAsync();
            }
#endif
            try
            {
                if (commandLineArgs.IsCommand("prompt"))
                {
                    await RunPromptAsync();
                }
                else if (commandLineArgs.IsCommand("batch"))
                {
                    await RunBatchAsync(commandLineArgs);
                }
                else
                {
                    await RunInternalAsync(commandLineArgs);
                }
            }
            catch (CliUsageException usageException)
            {
                Logger.LogWarning(usageException.Message);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private async Task RunPromptAsync()
        {
            string GetPromptInput()
            {
                Console.WriteLine("Enter the command to execute or `exit` to exit the prompt model");
                Console.Write("> ");
                return Console.ReadLine();
            }

            var promptInput = GetPromptInput();
            do
            {
                try
                {
                    var commandLineArgs = CommandLineArgumentParser.Parse(promptInput.Split(" ").Where(x => !x.IsNullOrWhiteSpace()).ToArray());

                    if (commandLineArgs.IsCommand("batch"))
                    {
                        await RunBatchAsync(commandLineArgs);
                    }
                    else
                    {
                        await RunInternalAsync(commandLineArgs);
                    }
                }
                catch (CliUsageException usageException)
                {
                    Logger.LogWarning(usageException.Message);
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }

                promptInput = GetPromptInput();

            } while (promptInput?.ToLower() != "exit");
        }

        private async Task RunBatchAsync(CommandLineArgs commandLineArgs)
        {
            var targetFile = commandLineArgs.Target;
            if (targetFile.IsNullOrWhiteSpace())
            {
                throw new CliUsageException(
                    "Must provide a file name/path that contains a list of commands" +
                    Environment.NewLine + Environment.NewLine +
                    "Example: " +
                    "  abp batch commands.txt"
                    );
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), targetFile);
            var fileLines = File.ReadAllLines(filePath);
            foreach (var line in fileLines)
            {
                var lineText = line;
                if (lineText.IsNullOrWhiteSpace() || lineText.StartsWith("#"))
                {
                    continue;
                }

                if (lineText.Contains('#'))
                {
                    lineText = lineText.Substring(0, lineText.IndexOf('#'));
                }

                var args = CommandLineArgumentParser.Parse(lineText);
                await RunInternalAsync(args);
            }
        }
        
        private async Task RunInternalAsync(CommandLineArgs commandLineArgs)
        {
            var commandType = CommandSelector.Select(commandLineArgs);

            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var command = (IConsoleCommand) scope.ServiceProvider.GetRequiredService(commandType);
                await command.ExecuteAsync(commandLineArgs);
            }
        }

        private async Task CheckCliVersionAsync()
        {
            var assembly = typeof(CliService).Assembly;
            var toolPath = GetToolPath(assembly);
            var currentCliVersion = await GetCurrentCliVersionInternalAsync(assembly);
            var updateChannel = GetUpdateChannel(currentCliVersion);

            Logger.LogInformation($"Version {currentCliVersion} ({updateChannel})");

            try
            {
                var latestVersion = await GetLatestVersion(updateChannel);

                if (latestVersion != null && latestVersion > currentCliVersion)
                {
                    LogNewVersionInfo(updateChannel, latestVersion, toolPath);
                }
            }
            catch (Exception e)
            {
                Logger.LogWarning("Unable to retrieve the latest version");
                Logger.LogWarning(e.Message);
            }
        }

        private string GetToolPath(Assembly assembly)
        {
            if (!assembly.Location.Contains(".store"))
            {
                return null;
            }

            return assembly.Location.Substring(0, assembly.Location.IndexOf(".store", StringComparison.Ordinal));
        }

        public async Task<SemanticVersion> GetCurrentCliVersionAsync(Assembly assembly)
        {
            return await GetCurrentCliVersionInternalAsync(assembly);
        }

        private async Task<SemanticVersion> GetCurrentCliVersionInternalAsync(Assembly assembly)
        {
            SemanticVersion currentCliVersion = default;

            var consoleOutput = new StringReader(CmdHelper.RunCmdAndGetOutput($"dotnet tool list -g"));
            string line;
            while ((line = await consoleOutput.ReadLineAsync()) != null)
            {
                if (line.StartsWith("volo.abp.cli", StringComparison.InvariantCultureIgnoreCase))
                {
                    var version = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries)[1];

                    SemanticVersion.TryParse(version, out currentCliVersion);

                    break;
                }
            }


            if (currentCliVersion == null)
            {
                // If not a tool executable, fallback to assembly version and treat as dev without updates
                // Assembly revisions are not supported by SemVer scheme required for NuGet, trim to {major}.{minor}.{patch}
                var assemblyVersion = string.Join(".", assembly.GetFileVersion().Split('.').Take(3));
                return SemanticVersion.Parse(assemblyVersion + "-dev");
            }

            return currentCliVersion;
        }

        private UpdateChannel GetUpdateChannel(SemanticVersion currentCliVersion)
        {
            if (!currentCliVersion.IsPrerelease)
            {
                return UpdateChannel.Stable;
            }

            if (currentCliVersion.Release.Contains("preview"))
            {
                return UpdateChannel.Nightly;
            }

            if (currentCliVersion.Release.Contains("dev"))
            {
                return UpdateChannel.Development;
            }

            return UpdateChannel.Prerelease;
        }

        private async Task<SemanticVersion> GetLatestVersion(UpdateChannel updateChannel)
        {
            switch (updateChannel)
            {
                case UpdateChannel.Stable:
                    return await NuGetService.GetLatestVersionOrNullAsync("Volo.Abp.Cli");

                case UpdateChannel.Prerelease:
                    return await NuGetService.GetLatestVersionOrNullAsync("Volo.Abp.Cli", includeReleaseCandidates: true);

                case UpdateChannel.Nightly:
                    return await NuGetService.GetLatestVersionOrNullAsync("Volo.Abp.Cli", includeNightly: true);

                default:
                    return default;
            }
        }

        private bool IsGlobalTool(string toolPath)
        {
            var globalPaths = new[] { @"%USERPROFILE%\.dotnet\tools\", "%HOME%/.dotnet/tools/", };
            return globalPaths.Select(Environment.ExpandEnvironmentVariables).Contains(toolPath);
        }

        private void LogNewVersionInfo(UpdateChannel updateChannel, SemanticVersion latestVersion, string toolPath)
        {
            var toolPathArg = IsGlobalTool(toolPath) ? "-g" : $"--tool-path {toolPath}";

            Logger.LogWarning($"ABP CLI has a newer {updateChannel.ToString().ToLowerInvariant()} version {latestVersion}, please update to get the latest features and fixes.");
            Logger.LogWarning(string.Empty);
            Logger.LogWarning("Update Command: ");

            // Update command doesn't support prerelease versions https://github.com/dotnet/sdk/issues/2551 workaround is to uninstall & install
            switch (updateChannel)
            {
                case UpdateChannel.Stable:
                    Logger.LogWarning($"dotnet tool update {toolPathArg} Volo.Abp.Cli");
                    break;

                case UpdateChannel.Prerelease:
                    Logger.LogWarning($"dotnet tool update {toolPathArg} Volo.Abp.Cli --version {latestVersion}");
                    break;

                case UpdateChannel.Nightly:
                case UpdateChannel.Development:
                    Logger.LogWarning($"dotnet tool uninstall {toolPathArg} Volo.Abp.Cli");
                    Logger.LogWarning($"dotnet tool install {toolPathArg} Volo.Abp.Cli --add-source https://www.myget.org/F/abp-nightly/api/v3/index.json --version {latestVersion}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(updateChannel), updateChannel, null);
            }

            Logger.LogWarning(string.Empty);
        }

        protected enum UpdateChannel
        {
            Development,
            Stable,
            Prerelease,
            Nightly
        }
    }
}
