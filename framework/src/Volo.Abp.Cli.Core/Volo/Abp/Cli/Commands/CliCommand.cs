using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Commands.Services;
using Volo.Abp.Cli.Version;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands;

public class CliCommand : IConsoleCommand, ITransientDependency
{
    public const string Name = "cli";

    private const string CliPackageName = "Volo.Abp.Cli";

    private readonly ICmdHelper _cmdHelper;
    private readonly PackageVersionCheckerService _packageVersionCheckerService;
    private readonly AbpNuGetIndexUrlService _nuGetIndexUrlService;
    public ILogger<CliCommand> Logger { get; set; }

    public CliCommand(ICmdHelper cmdHelper, PackageVersionCheckerService packageVersionCheckerService, AbpNuGetIndexUrlService nuGetIndexUrlService)
    {
        _cmdHelper = cmdHelper;
        _packageVersionCheckerService = packageVersionCheckerService;
        _nuGetIndexUrlService = nuGetIndexUrlService;

        Logger = NullLogger<CliCommand>.Instance;
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
                _cmdHelper.RunCmd("abp");
                break;

            case "update":
                await UpdateCliAsync(version, preview);
                break;

            case "remove":
                RemoveCli();
                break;
        }
    }

    private async Task UpdateCliAsync(string version = null, bool preview = false)
    {
        var infoText = "Updating ABP CLI ";
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

            _cmdHelper.RunCmdAndExit($"dotnet tool update {CliPackageName}{versionOption} -g", delaySeconds: 2);
        }
        catch (Exception ex)
        {
            Logger.LogError("Couldn't update ABP CLI." + ex.Message);
            ShowCliManualUpdateCommand();
        }
    }

    private async Task<string> GetLatestPreviewVersion()
    {
        var latestPreviewVersionInfo = await _packageVersionCheckerService
            .GetLatestVersionOrNullAsync(
                packageId: CliPackageName,
                includeReleaseCandidates: true
            );

        return latestPreviewVersionInfo.Version.IsPrerelease ? latestPreviewVersionInfo.Version.ToString() : null;
    }

    private void ShowCliManualUpdateCommand()
    {
        Logger.LogError("You can also run the following command to update ABP CLI.");
        Logger.LogError("dotnet tool update -g Volo.Abp.Cli");
    }

    private void RemoveCli()
    {
        Logger.LogInformation("Removing CLI...");
        _cmdHelper.RunCmdAndExit("dotnet tool uninstall " + CliPackageName + " -g", delaySeconds: 2);
    }

    public string GetUsageInfo()
    {
        var sb = new StringBuilder();

        sb.AppendLine("");
        sb.AppendLine("Usage:");
        sb.AppendLine("");
        sb.AppendLine("  abp cli [options]");
        sb.AppendLine("");
        sb.AppendLine("Options:");
        sb.AppendLine("");
        sb.AppendLine("update                                 (update ABP CLI to the latest)");
        sb.AppendLine("remove                                 (uninstall ABP CLI)");
        sb.AppendLine("");
        sb.AppendLine("Examples:");
        sb.AppendLine("");
        sb.AppendLine("  abp cli update");
        sb.AppendLine("  abp cli update --preview");
        sb.AppendLine("  abp cli update --version 4.2.2");
        sb.AppendLine("  abp cli remove");
        sb.AppendLine("");

        return sb.ToString();
    }

    public static string GetShortDescription()
    {
        return "Update or remove ABP CLI. See https://abp.io/docs/latest/cli";
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
