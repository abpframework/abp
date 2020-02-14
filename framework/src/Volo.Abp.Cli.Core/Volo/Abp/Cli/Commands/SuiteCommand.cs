using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Licensing;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands
{
    public class SuiteCommand : IConsoleCommand, ITransientDependency
    {
        private const string SuitePackageName = "Volo.Abp.Suite";
        public ILogger<SuiteCommand> Logger { get; set; }
        private readonly IApiKeyService _apiKeyService;

        public SuiteCommand(IApiKeyService apiKeyService)
        {
            _apiKeyService = apiKeyService;
            Logger = NullLogger<SuiteCommand>.Instance;
        }

        public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            var operationType = NamespaceHelper.NormalizeNamespace(commandLineArgs.Target);

            switch (operationType)
            {
                case "":
                case null:
                    RunSuite();
                    break;

                case "install":
                    Logger.LogInformation("Installing ABP Suite...");
                    await InstallSuiteAsync();
                    break;

                case "update":
                    Logger.LogInformation("Updating ABP Suite...");
                    await UpdateSuiteAsync();
                    break;

                case "remove":
                    Logger.LogInformation("Removing ABP Suite...");
                    RemoveSuite();
                    break;
            }
        }

        private async Task InstallSuiteAsync()
        {
            var nugetIndexUrl = await GetNuGetIndexUrlAsync();

            if (nugetIndexUrl == null)
            {
                return;
            }

            var result = CmdHelper.RunCmd("dotnet tool install " + SuitePackageName + " --add-source " + nugetIndexUrl + " -g");

            if (result == 0)
            {
                Logger.LogInformation("ABP Suite has been successfully installed.");
                Logger.LogInformation("You can run it with the CLI command \"abp suite\"");
            }
        }

        private async Task UpdateSuiteAsync()
        {
            var nugetIndexUrl = await GetNuGetIndexUrlAsync();

            if (nugetIndexUrl == null)
            {
                return;
            }

            CmdHelper.RunCmd("dotnet tool update " + SuitePackageName + " --add-source " + nugetIndexUrl + " -g");
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
                    Logger.LogWarning("ABP Suite is not installed! To install it you can run the command: \"abp suite install\"");
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning("Couldn't check ABP Suite installed status: " + ex.Message);
            }

            CmdHelper.RunCmd("abp-suite");
        }

        private async Task<string> GetNuGetIndexUrlAsync()
        {
            var apiKeyResult = await _apiKeyService.GetApiKeyOrNullAsync();

            if (apiKeyResult == null)
            {
                Logger.LogWarning("You are not signed in! Use the CLI command \"abp login <username>\" to sign in, then try again.");
                return null;
            }

            if (!string.IsNullOrWhiteSpace(apiKeyResult.ErrorMessage))
            {
                Logger.LogWarning(apiKeyResult.ErrorMessage);
                return null;
            }

            if (string.IsNullOrEmpty(apiKeyResult.ApiKey))
            {
                Logger.LogError("Couldn't retrieve your NuGet API key! You can re-sign in with the CLI command \"abp login <username>\".");
                return null;
            }

            return CliUrls.GetNuGetServiceIndexUrl(apiKeyResult.ApiKey);
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
            sb.AppendLine("  abp suite update");
            sb.AppendLine("  abp suite remove");
            sb.AppendLine("");

            return sb.ToString();
        }

        public string GetShortDescription()
        {
            return "Install, update, remove or start ABP Suite. See https://commercial.abp.io/tools/suite.";
        }
    }
}