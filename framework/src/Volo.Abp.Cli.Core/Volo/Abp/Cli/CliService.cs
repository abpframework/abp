using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NuGet.Versioning;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Commands;
using Volo.Abp.Cli.Memory;
using Volo.Abp.Cli.Version;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli;

public class CliService : ITransientDependency
{
    private readonly MemoryService _memoryService;
    public ILogger<CliService> Logger { get; set; }
    protected ICommandLineArgumentParser CommandLineArgumentParser { get; }
    protected ICommandSelector CommandSelector { get; }
    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected PackageVersionCheckerService PackageVersionCheckerService { get; }
    public ICmdHelper CmdHelper { get; }
    protected CliVersionService CliVersionService { get; }

    public CliService(
        ICommandLineArgumentParser commandLineArgumentParser,
        ICommandSelector commandSelector,
        IServiceScopeFactory serviceScopeFactory,
        PackageVersionCheckerService nugetService,
        ICmdHelper cmdHelper,
        MemoryService memoryService,
        CliVersionService cliVersionService)
    {
        _memoryService = memoryService;
        CommandLineArgumentParser = commandLineArgumentParser;
        CommandSelector = commandSelector;
        ServiceScopeFactory = serviceScopeFactory;
        PackageVersionCheckerService = nugetService;
        CmdHelper = cmdHelper;
        CliVersionService = cliVersionService;

        Logger = NullLogger<CliService>.Instance;
    }

    public async Task RunAsync(string[] args)
    {
        var currentCliVersion = await CliVersionService.GetCurrentCliVersionAsync();
        Logger.LogInformation($"ABP CLI {currentCliVersion}");

        var commandLineArgs = CommandLineArgumentParser.Parse(args);

#if !DEBUG
        if (!commandLineArgs.Options.ContainsKey("skip-cli-version-check"))
        {
            await CheckCliVersionAsync(currentCliVersion);
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
            Environment.ExitCode = 1;
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            throw;
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
            var command = (IConsoleCommand)scope.ServiceProvider.GetRequiredService(commandType);
            await command.ExecuteAsync(commandLineArgs);
        }
    }

    private async Task CheckCliVersionAsync(SemanticVersion currentCliVersion)
    {
        if (!await IsLatestVersionCheckExpiredAsync())
        {
            return;
        }

        try
        {
            var assembly = typeof(CliService).Assembly;
            var toolPath = GetToolPath(assembly);
            var updateChannel = GetUpdateChannel(currentCliVersion);

            var latestVersionInfo = await GetLatestVersion(updateChannel);
            if (ShouldLogNewVersionInfo(latestVersionInfo, currentCliVersion))
            {
                if(updateChannel == UpdateChannel.Prerelease && !latestVersionInfo.Version.IsPrerelease)
                {
                    latestVersionInfo = await PackageVersionCheckerService.GetLatestStableVersionFromGithubAsync();

                    if(ShouldLogNewVersionInfo(latestVersionInfo, currentCliVersion))
                    {
                        LogNewVersionInfo(updateChannel, latestVersionInfo.Version, toolPath, latestVersionInfo.Message);
                    }

                    return;
                }

                LogNewVersionInfo(updateChannel, latestVersionInfo.Version, toolPath, latestVersionInfo.Message);
            }
        }
        catch (Exception e)
        {
            Logger.LogWarning("Unable to retrieve the latest version: " + e.Message);
        }
    }

    private bool ShouldLogNewVersionInfo(LatestVersionInfo latestVersionInfo, SemanticVersion currentCliVersion)
    {
        return latestVersionInfo != null && latestVersionInfo.Version > currentCliVersion;
    }

    private async Task<bool> IsLatestVersionCheckExpiredAsync()
    {
        try
        {
            var latestTimeAsString = await _memoryService.GetAsync(CliConsts.MemoryKeys.LatestCliVersionCheckDate);
            if (DateTime.TryParse(latestTimeAsString,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out var latestTimeParsed))
            {
                if (DateTime.Now.Subtract(latestTimeParsed).TotalDays < 1)
                {
                    return false;
                }
            }

            await _memoryService.SetAsync(CliConsts.MemoryKeys.LatestCliVersionCheckDate, DateTime.Now.ToString(CultureInfo.InvariantCulture));

            return true;
        }
        catch (Exception)
        {
            return true;
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

    private async Task<LatestVersionInfo> GetLatestVersion(UpdateChannel updateChannel)
    {
        switch (updateChannel)
        {
            case UpdateChannel.Stable:
                return await PackageVersionCheckerService.GetLatestVersionOrNullAsync("Volo.Abp.Cli");

            case UpdateChannel.Prerelease:
                return await PackageVersionCheckerService.GetLatestVersionOrNullAsync("Volo.Abp.Cli", includeReleaseCandidates: true);

            case UpdateChannel.Nightly:
                return await PackageVersionCheckerService.GetLatestVersionOrNullAsync("Volo.Abp.Cli", includeNightly: true);

            default:
                return default;
        }
    }

    private bool IsGlobalTool(string toolPath)
    {
        var globalPaths = new[] { @"%USERPROFILE%\.dotnet\tools\", "%HOME%/.dotnet/tools/", };
        return globalPaths.Select(Environment.ExpandEnvironmentVariables).Contains(toolPath);
    }

    private void LogNewVersionInfo(UpdateChannel updateChannel, SemanticVersion latestVersion, string toolPath, string message = null)
    {
        var toolPathArg = IsGlobalTool(toolPath) ? "-g" : $"--tool-path {toolPath}";

        Logger.LogWarning($"A newer {updateChannel.ToString().ToLowerInvariant()} version of the ABP CLI is available: {latestVersion}.");

        if (!string.IsNullOrWhiteSpace(message))
        {
            Logger.LogWarning(message);
        }

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
