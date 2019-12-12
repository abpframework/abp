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
                    Logger.LogInformation("Running Suite...");
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
                    UpdateSuite();
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
            var apiKeyResult = await _apiKeyService.GetApiKeyOrNullAsync();
            if (apiKeyResult == null || string.IsNullOrEmpty(apiKeyResult.ApiKey))
            {
                Logger.LogInformation("Couldn't retrieve the API Key for Nuget!");
                await Task.CompletedTask;
                return;
            }

            var nugetIndexUrl = CliUrls.GetNuGetServiceIndexUrl(apiKeyResult.ApiKey);
            CmdHelper.RunCmd("dotnet tool install " + SuitePackageName + " --add-source " + nugetIndexUrl + " -g");
        }

        private static void UpdateSuite()
        {
            CmdHelper.RunCmd("dotnet tool update " + SuitePackageName + " -g");
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
            return "Shortcut commands to use Abp Suite tool.";
        }
    }
}