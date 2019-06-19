using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NuGet.Versioning;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
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
        protected IHybridServiceScopeFactory ServiceScopeFactory { get; }
        protected NuGetService NuGetService { get; }

        public CliService(
            ICommandLineArgumentParser commandLineArgumentParser,
            ICommandSelector commandSelector,
            IHybridServiceScopeFactory serviceScopeFactory,
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

            await CheckCliVersionAsync();
            CheckDependencies();

            var commandLineArgs = CommandLineArgumentParser.Parse(args);
            var commandType = CommandSelector.Select(commandLineArgs);

            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var command = (IConsoleCommand)scope.ServiceProvider.GetRequiredService(commandType);

                try
                {
                    await command.ExecuteAsync(commandLineArgs);
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
        }

        private void CheckDependencies()
        {
            var installedNpmPackages = CmdHelper.RunCmdAndGetOutput("npm list -g --depth 0");

            if (!installedNpmPackages.Contains(" yarn@"))
            {
                InstallYarn();
            }
            if (!installedNpmPackages.Contains(" gulp@"))
            {
                InstallGulp();
            }
        }

        private void InstallYarn()
        {
            Logger.LogInformation("Installing yarn...");
            CmdHelper.RunCmd("npm install yarn -g");
        }

        private void InstallGulp()
        {
            Logger.LogInformation("Installing gulp...");
            CmdHelper.RunCmd("npm install gulp -g");
        }

        private async Task CheckCliVersionAsync()
        {
            var assembly = typeof(CliService).Assembly;
            var toolPath = GetToolPath(assembly);
            var currentCliVersion = await GetCurrentCliVersion(toolPath, assembly);
            var updateChannel = GetUpdateChannel(currentCliVersion);

            Logger.LogInformation($"Version {currentCliVersion} ({updateChannel} channel)");

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

        private static string GetToolPath(Assembly assembly)
        {
            if (!assembly.Location.Contains(".store"))
            {
                return null;
            }

            return assembly.Location.Substring(0, assembly.Location.IndexOf(".store", StringComparison.Ordinal));
        }

        private async Task<SemanticVersion> GetCurrentCliVersion(string toolPath, Assembly assembly)
        {
            SemanticVersion currentCliVersion = default;
            if (!string.IsNullOrEmpty(toolPath))
            {
                var consoleOutput = new StringReader(CmdHelper.RunCmdAndGetOutput($"dotnet tool list --tool-path {toolPath}"));
                string line;
                while ((line = await consoleOutput.ReadLineAsync()) != null)
                {
                    if (line.StartsWith("Volo.Abp.Cli", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var version = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries)[1];

                        SemanticVersion.TryParse(version, out currentCliVersion);

                        break;
                    }
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

        private static UpdateChannel GetUpdateChannel(SemanticVersion currentCliVersion)
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
                    return await NuGetService.GetLatestVersionOrNullAsync("Volo.Abp.Cli", includePreviews: true);

                case UpdateChannel.Nightly:
                    return await NuGetService.GetLatestVersionOrNullAsync("Volo.Abp.Cli", includeNightly: true);
                default:
                    return default;
            }
        }

        private void LogNewVersionInfo(UpdateChannel updateChannel, SemanticVersion latestVersion, string toolPath)
        {
            Logger.LogWarning(
                $"ABP CLI has a newer {updateChannel.ToString().ToLowerInvariant()} version {latestVersion}, please update to get the latest features and fixes.");
            Logger.LogWarning("");
            Logger.LogWarning("Update Command: ");

            // Update command doesn't support prerelease versions https://github.com/dotnet/sdk/issues/2551 workaround is to uninstall & install
            switch (updateChannel)
            {
                case UpdateChannel.Stable:
                    Logger.LogWarning($"    dotnet tool update --tool-path {toolPath} Volo.Abp.Cli");
                    break;

                case UpdateChannel.Prerelease:
                    Logger.LogWarning($"    dotnet tool uninstall --tool-path {toolPath} Volo.Abp.Cli");
                    Logger.LogWarning($"    dotnet tool install --tool-path {toolPath} --version {latestVersion} Volo.Abp.Cli");
                    break;

                case UpdateChannel.Nightly:
                    Logger.LogWarning($"    dotnet tool uninstall --tool-path {toolPath} Volo.Abp.Cli");
                    Logger.LogWarning(
                        $"    dotnet tool install --tool-path {toolPath} --add-source https://www.myget.org/F/abp-nightly/api/v3/index.json --version {latestVersion} Volo.Abp.Cli");
                    break;
            }

            Logger.LogWarning("");
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
