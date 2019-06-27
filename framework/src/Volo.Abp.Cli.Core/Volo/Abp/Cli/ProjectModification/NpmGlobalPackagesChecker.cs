using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification
{
    public class NpmGlobalPackagesChecker : ITransientDependency
    {
        public ILogger<NpmGlobalPackagesChecker> Logger { get; set; }

        public NpmGlobalPackagesChecker(PackageJsonFileFinder packageJsonFileFinder)
        {
            Logger = NullLogger<NpmGlobalPackagesChecker>.Instance;
        }

        public void Check()
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

        protected virtual string GetInstalledNpmPackages()
        {
            Logger.LogInformation("Checking installed npm global packages...");
            return CmdHelper.RunCmdAndGetOutput("npm list -g --depth 0");
        }

        protected virtual void InstallYarn()
        {
            Logger.LogInformation("Installing yarn...");
            CmdHelper.RunCmd("npm install yarn -g");
        }

        protected virtual void InstallGulp()
        {
            Logger.LogInformation("Installing gulp...");
            CmdHelper.RunCmd("npm install gulp -g");
        }
    }
}
