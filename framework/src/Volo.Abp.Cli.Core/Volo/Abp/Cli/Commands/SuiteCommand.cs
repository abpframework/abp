using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Licensing;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.Cli.ProjectBuilding.Building;
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
                case "i":
                    Logger.LogInformation("Installing Suite...");
                    await InstallSuiteAsync();
                    break;

                case "update":
                case "u":
                    Logger.LogInformation("Updating Suite...");
                    await UpdateSuiteAsync();
                    break;

                case "remove":
                case "r":
                    Logger.LogInformation("Removing Suite...");
                    RemoveSuite();
                    break;
            }
        }

        private async Task InstallSuiteAsync()
        {
            var nugetIndexUrl = await GetNuGetIndexUrlAsync();
            var result = CmdHelper.RunCmd("dotnet tool install " + SuitePackageName + " --add-source " + nugetIndexUrl + " -g");

            if (result == 0)
            {
                Logger.LogInformation("Suite has been successfully installed.");
                Logger.LogInformation("You can run it with the CLI command \"abp suite\"");
            }
        }

        private async Task UpdateSuiteAsync()
        {
            var nugetIndexUrl = await GetNuGetIndexUrlAsync();
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
                    Logger.LogWarning("Suite is not installed! To install it you can run the command: \"abp suite install\"");
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning("Couldn't check Suite installed status: " + ex.Message);
            }

            CmdHelper.RunCmd("abp-suite");
        }

        private async Task<string> GetNuGetIndexUrlAsync()
        {
            var apiKeyResult = await _apiKeyService.GetApiKeyOrNullAsync();
            if (apiKeyResult == null ||
                string.IsNullOrEmpty(apiKeyResult.ApiKey))
            {
                Logger.LogError("Couldn't retrieve your NuGet API key!");
                Logger.LogWarning(File.Exists(CliPaths.AccessToken)
                    ? "Make sure you have an active session and license on commercial.abp.io. To re-sign in you can use the CLI command \"abp login <username>\"."
                    : "You are not signed in to commercial.abp.io. Use the CLI command \"abp login <username>\" to sign in.");

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
            sb.AppendLine("<no argument>                               (runs Suite)");
            sb.AppendLine("-i|--install                                (installs Suite as a dotnet global tool)");
            sb.AppendLine("-u|--update                                 (updates Suite to the latest)");
            sb.AppendLine("-r|--remove                                 (uninstalls Suite)");
            sb.AppendLine("");
            sb.AppendLine("Examples:");
            sb.AppendLine("");
            sb.AppendLine("  abp suite");
            sb.AppendLine("  abp suite install");
            sb.AppendLine("  abp suite update");
            sb.AppendLine("");

            return sb.ToString();
        }

        public string GetShortDescription()
        {
            return "Utility commands to use Abp Suite tool. Installs, updates, removes or starts Suite.";
        }
    }
}