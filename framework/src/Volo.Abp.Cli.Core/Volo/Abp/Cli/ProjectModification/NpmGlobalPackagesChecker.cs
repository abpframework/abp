using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification;

public class NpmGlobalPackagesChecker : ITransientDependency
{
    public ICmdHelper CmdHelper { get; }
    public ILogger<NpmGlobalPackagesChecker> Logger { get; set; }

    public NpmGlobalPackagesChecker(ICmdHelper cmdHelper)
    {
        CmdHelper = cmdHelper;
        Logger = NullLogger<NpmGlobalPackagesChecker>.Instance;
    }

    public void Check()
    {
        var installedNpmPackages = GetInstalledNpmPackages();

        if (!installedNpmPackages.Contains(" yarn@"))
        {
            InstallYarn();
        }
    }

    protected virtual string GetInstalledNpmPackages()
    {
        Logger.LogInformation("Checking installed npm global packages...");
        return CmdHelper.RunCmdAndGetOutput("npm list -g --depth 0 --silent", out int exitCode);
    }

    protected virtual void InstallYarn()
    {
        Logger.LogInformation("Installing yarn...");
        CmdHelper.RunCmd("npm install yarn -g");
    }
}
