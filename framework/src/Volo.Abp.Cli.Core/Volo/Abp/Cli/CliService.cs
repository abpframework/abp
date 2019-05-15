using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Commands;
using Volo.Abp.Cli.NuGet;
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
            Logger.LogInformation($"ABP CLI, version {GetCliVersion()}.");
            Logger.LogInformation("https://abp.io");

            await CheckHasNewVersion();
            
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

        private async Task CheckHasNewVersion()
        {
            var currentVersion = GetCliVersion();
            var latestVersion = await NuGetService.GetLatestVersionAsync("Volo.Abp.Cli");
            
            if (!string.IsNullOrEmpty(latestVersion) && currentVersion != latestVersion)
            {
                Logger.LogInformation("");
                Logger.LogWarning("Cli has a new version. Please update.");
                Logger.LogWarning("Update Command: dotnet tool update -g Volo.Abp.Cli");
            }
        }
        
        private static string GetCliVersion()
        {
            return typeof(CliService)
                .Assembly
                .GetFileVersion()
                .RemovePostFix(".0");
        }
    }
}