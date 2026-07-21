using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

    [Obsolete("This method is deprecated. Use 'RunYarn' instead (it uses 'npx', so there is no need for 'yarn' to be globally installed.")]
    public void RunNpmInstall(string directory, params string[] args)
    {
        Logger.LogInformation($"Running npm install on {directory}");
        CmdHelper.RunCmd($"npm install --ignore-scripts {args.JoinAsString(" ")}", directory);
    }

    public void RunYarn(string directory)
    {
        Logger.LogInformation($"Running Yarn on {directory}");
        CmdHelper.RunCmd($"npx yarn --ignore-scripts", directory);
    }

    [Obsolete("This method is deprecated. Use 'YarnAddPackage' instead (it uses 'npx', so there is no need for 'yarn' to be globally installed.")]
    public void NpmInstallPackage(string package, string version, string directory)
    {
        EnsureSafePackageName(package);
        EnsureSafeVersion(version);
        var packageVersion = !string.IsNullOrWhiteSpace(version) ? $"@{version}" : string.Empty;
        CmdHelper.RunCmd("npm install --ignore-scripts " + package + packageVersion, workingDirectory: directory);
    }

    public void YarnAddPackage(string package, string version, string directory)
    {
        EnsureSafePackageName(package);
        EnsureSafeVersion(version);
        var packageVersion = !string.IsNullOrWhiteSpace(version) ? $"@{version}" : string.Empty;
        CmdHelper.RunCmd("npx yarn add " + package + packageVersion + " --ignore-scripts", workingDirectory: directory);
    }

    private static readonly Regex SafePackageNameRegex = new(
        @"^(@[a-zA-Z0-9][a-zA-Z0-9._-]*/)?[a-zA-Z0-9][a-zA-Z0-9._-]*$",
        RegexOptions.Compiled);

    private static readonly Regex SafeVersionRegex = new(
        @"^[a-zA-Z0-9._~^+\-]+$",
        RegexOptions.Compiled);

    public static void EnsureSafePackageName(string packageName)
    {
        if (string.IsNullOrWhiteSpace(packageName) || !SafePackageNameRegex.IsMatch(packageName))
        {
            throw new CliUsageException($"Invalid npm package name detected: {SanitizeForLog(packageName)}");
        }
    }

    public static void EnsureSafeVersion(string version)
    {
        if (!string.IsNullOrWhiteSpace(version) && !SafeVersionRegex.IsMatch(version))
        {
            throw new CliUsageException($"Invalid npm package version detected: {SanitizeForLog(version)}");
        }
    }

    public static string SanitizeForLog(string value)
    {
        if (value == null)
        {
            return "(null)";
        }

        return Regex.Replace(value, @"[\x00-\x1F\x7F]", "?");
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
