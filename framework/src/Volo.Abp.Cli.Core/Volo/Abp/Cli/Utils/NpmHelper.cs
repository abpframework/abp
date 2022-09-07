using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NuGet.Versioning;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Utils;

public class NpmHelper : ITransientDependency
{
    protected ICmdHelper CmdHelper { get; }
    public ILogger<NpmHelper> Logger { get; set; }

    public NpmHelper(ICmdHelper cmdHelper)
    {
        CmdHelper = cmdHelper;
        Logger = NullLogger<NpmHelper>.Instance;
    }

    public bool IsNpmInstalled()
    {
        var output = CmdHelper.RunCmdAndGetOutput("npm -v").Trim();
        var outputLines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        return outputLines.Any(ol => SemanticVersion.TryParse(ol, out _));
    }

    public bool IsYarnAvailable()
    {
        var output = CmdHelper.RunCmdAndGetOutput("yarn -v").Trim();
        var outputLines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        SemanticVersion version = null;

        foreach (var outputLine in outputLines)
        {
            if (SemanticVersion.TryParse(outputLine, out version))
            {
                break;
            }
        }

        if (version == null)
        {
            return false;
        }
        
        return version > SemanticVersion.Parse("1.20.0");
    }

    public void RunNpmInstall(string directory)
    {
        Logger.LogInformation($"Running npm install on {directory}");
        CmdHelper.RunCmd($"npm install", directory);
    }

    public void RunYarn(string directory)
    {
        Logger.LogInformation($"Running Yarn on {directory}");
        CmdHelper.RunCmd($"yarn", directory);
    }

    public void NpmInstallPackage(string package, string version, string directory)
    {
        var packageVersion = !string.IsNullOrWhiteSpace(version) ? $"@{version}" : string.Empty;
        CmdHelper.RunCmd("npm install " + package + packageVersion, workingDirectory: directory);
    }

    public void YarnAddPackage(string package, string version, string directory)
    {
        var packageVersion = !string.IsNullOrWhiteSpace(version) ? $"@{version}" : string.Empty;
        CmdHelper.RunCmd("yarn add " + package + packageVersion, workingDirectory: directory);
    }

    public string GetInstalledNpmPackages()
    {
        Logger.LogInformation("Checking installed npm global packages...");
        return CmdHelper.RunCmdAndGetOutput("npm list -g --depth 0 --silent", out int exitCode);
    }

    public void InstallYarn()
    {
        Logger.LogInformation("Installing yarn...");
        CmdHelper.RunCmd("npm install yarn -g");
    }
}
