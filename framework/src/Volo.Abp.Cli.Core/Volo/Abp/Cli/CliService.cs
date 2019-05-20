using System;
using System.Diagnostics;
using System.IO;
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

            await CheckForNewVersion();

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
            var installedNpmPackages = GetInstalledNpmPackages();

            if (!installedNpmPackages.Contains(" yarn@"))
            {
                InstallYarn();
            }
            if (!installedNpmPackages.Contains(" gulp@"))
            {
                InstallGulp();
            }
        }

        private string GetInstalledNpmPackages()
        {
            var output = "";

            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo("CMD.exe")
                {
                    Arguments = "/C npm list -g --depth 0",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
                process.Start();

                using (var stdOut = process.StandardOutput)
                {
                    using (var stdErr = process.StandardError)
                    {
                        output = stdOut.ReadToEnd();
                        output += stdErr.ReadToEnd();
                    }
                }
            }

            return output;
        }

        private void InstallYarn()
        {
            Logger.LogInformation("Installing yarn...");
            var process = Process.Start("CMD.exe", "/C npm install yarn -g");
            process.WaitForExit();
        }

        private void InstallGulp()
        {
            Logger.LogInformation("Installing gulp...");
            var process = Process.Start("CMD.exe", "/C npm install gulp -g");
            process.WaitForExit();
        }

        private async Task CheckForNewVersion()
        {
            try
            {
                var currentVersion = GetCliVersion();
                var latestVersion = await NuGetService.GetLatestVersionOrNullAsync("Volo.Abp.Cli");

                if (!latestVersion.IsNullOrEmpty() && currentVersion != latestVersion)
                {
                    Logger.LogInformation("");
                    Logger.LogWarning("ABP CLI has a newer version (" + latestVersion + "). Please update to get the latest features and fixes.");
                    Logger.LogWarning("");
                    Logger.LogWarning("Update Command: ");
                    Logger.LogWarning("    dotnet tool update -g Volo.Abp.Cli");
                    Logger.LogWarning("");
                }
            }
            catch (Exception e)
            {
                Logger.LogWarning("Could not get the latest version infom from NuGet.org:");
                Logger.LogWarning(e.Message);
            }
        }

        private static string GetCliVersion()
        {
            var version = typeof(CliService)
                .Assembly
                .GetFileVersion();

            /* Assembly versions are like "2.4.0.0", but NuGet removes the last "0" here,
             * like "2.4.0".
             * So, we need to remove it from the assembly version to match to the NuGet version.
             */

            if (version.Split('.').Length == 4)
            {
                version = version.RemovePostFix(".0");
            }

            return version;
        }
    }
}