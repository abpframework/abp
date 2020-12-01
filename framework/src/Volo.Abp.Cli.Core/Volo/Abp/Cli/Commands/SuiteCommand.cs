using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NuGet.Versioning;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Commands.Services;
using Volo.Abp.Cli.NuGet;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands
{
    public class SuiteCommand : IConsoleCommand, ITransientDependency
    {
        private readonly AbpNuGetIndexUrlService _nuGetIndexUrlService;
        private readonly NuGetService _nuGetService;
        private const string SuitePackageName = "Volo.Abp.Suite";
        public ILogger<SuiteCommand> Logger { get; set; }

        public SuiteCommand(AbpNuGetIndexUrlService nuGetIndexUrlService, NuGetService nuGetService)
        {
            _nuGetIndexUrlService = nuGetIndexUrlService;
            _nuGetService = nuGetService;
            Logger = NullLogger<SuiteCommand>.Instance;
        }

        public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            var operationType = NamespaceHelper.NormalizeNamespace(commandLineArgs.Target);

            var preview = commandLineArgs.Options.ContainsKey(Options.Preview.Short) ||
                          commandLineArgs.Options.ContainsKey(Options.Preview.Long);

            var version = commandLineArgs.Options.GetOrNull(Options.Version.Short, Options.Version.Long);

            switch (operationType)
            {
                case "":
                case null:
                    await InstallSuiteIfNotInstalledAsync();
                    RunSuite();
                    break;

                case "install":
                    await InstallSuiteAsync(version, preview);
                    break;

                case "update":
                    await UpdateSuiteAsync(version, preview);
                    break;

                case "remove":
                    Logger.LogInformation("Removing ABP Suite...");
                    RemoveSuite();
                    break;
            }
        }

        private async Task InstallSuiteIfNotInstalledAsync()
        {
            var currentSuiteVersionAsString = GetCurrentSuiteVersion();

            if (string.IsNullOrEmpty(currentSuiteVersionAsString))
            {
                await InstallSuiteAsync();
            }
        }

        private string GetCurrentSuiteVersion()
        {
            var dotnetToolList = CmdHelper.RunCmdAndGetOutput("dotnet tool list -g");

            var suiteLine = dotnetToolList.Split(Environment.NewLine)
                .FirstOrDefault(l => l.ToLower().StartsWith("volo.abp.suite "));

            if (string.IsNullOrEmpty(suiteLine))
            {
                return null;
            }

            return suiteLine.Split(" ", StringSplitOptions.RemoveEmptyEntries)[1];
        }

        private async Task InstallSuiteAsync(string version = null, bool preview = false)
        {
            var infoText = "Installing ABP Suite ";
            if (version != null)
            {
                infoText += "v" + version + "... ";
            }
            else if (preview)
            {
                infoText += "latest preview version...";
            }
            else
            {
                infoText += "latest version...";
            }

            Logger.LogInformation(infoText);

            var nugetIndexUrl = await _nuGetIndexUrlService.GetAsync();

            if (nugetIndexUrl == null)
            {
                return;
            }

            try
            {
                var versionOption = string.Empty;

                if (preview)
                {
                    var latestPreviewVersion = await GetLatestPreviewVersion();
                    if (latestPreviewVersion != null)
                    {
                        versionOption = $" --version {latestPreviewVersion}";
                        Logger.LogInformation("Latest preview version is " + latestPreviewVersion);
                    }
                }
                else if (version != null)
                {
                    versionOption = $" --version {version}";
                }

                var result = CmdHelper.RunCmd(
                    $"dotnet tool install {SuitePackageName}{versionOption} --add-source {nugetIndexUrl} -g"
                );

                if (result == 0)
                {
                    Logger.LogInformation("ABP Suite has been successfully installed.");
                    Logger.LogInformation("You can run it with the CLI command \"abp suite\"");
                }
                else
                {
                    ShowSuiteManualInstallCommand();
                }
            }
            catch (Exception e)
            {
                Logger.LogError("Couldn't install ABP Suite." + e.Message);
                ShowSuiteManualInstallCommand();
            }
        }

        private void ShowSuiteManualInstallCommand()
        {
            Logger.LogInformation("You can also run the following command to install ABP Suite.");
            Logger.LogInformation("dotnet tool install -g Volo.Abp.Suite --add-source https://nuget.abp.io/<your-private-key>/v3/index.json");
        }

        private async Task UpdateSuiteAsync(string version = null, bool preview = false)
        {
            var infoText = "Updating ABP Suite ";
            if (version != null)
            {
                infoText += "to the " + version + "... ";
            }
            else if (preview)
            {
                infoText += "to the latest preview version...";
            }
            else
            {
                infoText += "...";
            }

            Logger.LogInformation(infoText);

            var nugetIndexUrl = await _nuGetIndexUrlService.GetAsync();
            if (nugetIndexUrl == null)
            {
                Logger.LogError("Cannot find your NuGet service URL!");
                return;
            }

            try
            {
                var versionOption = string.Empty;

                if (preview)
                {
                    var latestPreviewVersion = await GetLatestPreviewVersion();
                    if (latestPreviewVersion != null)
                    {
                        versionOption = $" --version {latestPreviewVersion}";
                        Logger.LogInformation("Latest preview version is " + latestPreviewVersion);
                    }
                }
                else if (version != null)
                {
                    versionOption = $" --version {version}";
                }

                var result = CmdHelper.RunCmd(
                    $"dotnet tool update {SuitePackageName}{versionOption} --add-source {nugetIndexUrl} -g"
                );

                if (result != 0)
                {
                    ShowSuiteManualUpdateCommand();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Couldn't update ABP Suite." + ex.Message);
                ShowSuiteManualUpdateCommand();
            }
        }

        private async Task<string> GetLatestPreviewVersion()
        {
            var latestPreviewVersion = await _nuGetService
                .GetLatestVersionOrNullAsync(
                    packageId: SuitePackageName,
                    includeReleaseCandidates: true
                );

            return latestPreviewVersion.IsPrerelease ? latestPreviewVersion.ToString() : null;
        }

        private void ShowSuiteManualUpdateCommand()
        {
            Logger.LogError("You can also run the following command to update ABP Suite.");
            Logger.LogError("dotnet tool update -g Volo.Abp.Suite --add-source https://nuget.abp.io/<your-private-key>/v3/index.json");
        }

        private static void RemoveSuite()
        {
            CmdHelper.RunCmd("dotnet tool uninstall " + SuitePackageName + " -g");
        }

        private void RunSuite()
        {
            try
            {
                if (!GlobalToolHelper.IsGlobalToolInstalled("abp-suite"))
                {
                    Logger.LogWarning(
                        "ABP Suite is not installed! To install it you can run the command: \"abp suite install\"");
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning("Couldn't check ABP Suite installed status: " + ex.Message);
            }

            CmdHelper.RunCmd("abp-suite");
        }

        public string GetUsageInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine("");
            sb.AppendLine("Usage:");
            sb.AppendLine("");
            sb.AppendLine("  abp suite [options]");
            sb.AppendLine("");
            sb.AppendLine("Options:");
            sb.AppendLine("");
            sb.AppendLine("<no argument>                          (run ABP Suite)");
            sb.AppendLine("install                                (install ABP Suite as a dotnet global tool)");
            sb.AppendLine("update                                 (update ABP Suite to the latest)");
            sb.AppendLine("remove                                 (uninstall ABP Suite)");
            sb.AppendLine("");
            sb.AppendLine("Examples:");
            sb.AppendLine("");
            sb.AppendLine("  abp suite");
            sb.AppendLine("  abp suite install");
            sb.AppendLine("  abp suite install --preview");
            sb.AppendLine("  abp suite update");
            sb.AppendLine("  abp suite update --preview");
            sb.AppendLine("  abp suite remove");
            sb.AppendLine("");

            return sb.ToString();
        }

        public string GetShortDescription()
        {
            return "Install, update, remove or start ABP Suite. See https://commercial.abp.io/tools/suite.";
        }

        public static class Options
        {
            public static class Preview
            {
                public const string Long = "preview";
                public const string Short = "p";
            }

            public static class Version
            {
                public const string Long = "version";
                public const string Short = "v";
            }
        }
    }
}