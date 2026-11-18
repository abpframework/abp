using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        var version = FindVersionOrNull(CmdHelper.RunCmdAndGetOutput("yarn -v"));
        if (version == null)
        {
            return false;
        }
        
        return version > SemanticVersion.Parse("1.20.0");
    }

    public string GetYarnCommand(string directory, string arguments = null)
    {
        // Prefer the yarn on PATH, so Corepack and the project's "packageManager" field decide the version.
        // npx doesn't search PATH and can't see the Corepack shims on Windows, it runs Yarn 1 even in a Yarn 2+ project.
        var yarn = "yarn";
        var version = FindVersionOrNull(CmdHelper.RunCmdAndGetOutput("yarn -v", directory));
        if (version == null)
        {
            yarn = "npx yarn";
            // "--no" keeps npx from waiting on an install prompt the user can't see.
            // If npx downloads it later, it gets the yarn package, which is Yarn 1.
            version = FindVersionOrNull(CmdHelper.RunCmdAndGetOutput("npx --no -- yarn -v", directory));
        }

        // Yarn 2+ rejects --ignore-scripts, --mode=skip-build skips the install scripts there.
        var ignoreScriptsOption = version != null && version.Major >= 2 ? "--mode=skip-build" : "--ignore-scripts";
        return string.IsNullOrWhiteSpace(arguments)
            ? $"{yarn} {ignoreScriptsOption}"
            : $"{yarn} {arguments} {ignoreScriptsOption}";
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
        CmdHelper.RunCmd(GetYarnCommand(directory), directory);
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
        CmdHelper.RunCmd(GetYarnCommand(directory, "add " + package + packageVersion), workingDirectory: directory);
    }

    /// <summary>
    /// Finds the package manager of the JavaScript project in the given directory.
    /// The "packageManager" field of the package.json wins, then the lock file. Falls back to npm.
    /// </summary>
    public NodePackageManager DetectPackageManager(string directory)
    {
        return FindPackageManagerInPackageJsonOrNull(directory)
               ?? FindPackageManagerByLockFileOrNull(directory)
               ?? NodePackageManager.Npm;
    }

    /// <summary>
    /// Builds the command that adds a package with the package manager of the project.
    /// Install scripts are skipped, like in the other commands of this class.
    /// </summary>
    public string GetAddPackageCommand(string directory, string package, string version = null)
    {
        EnsureSafePackageName(package);
        EnsureSafeVersion(version);

        var packageWithVersion = string.IsNullOrWhiteSpace(version) ? package : $"{package}@{version}";

        return DetectPackageManager(directory) switch
        {
            NodePackageManager.Yarn => GetYarnCommand(directory, "add " + packageWithVersion),
            NodePackageManager.Pnpm => $"pnpm add {packageWithVersion} --ignore-scripts",
            _ => $"npm install {packageWithVersion} --ignore-scripts"
        };
    }

    private NodePackageManager? FindPackageManagerInPackageJsonOrNull(string directory)
    {
        var packageJsonFilePath = Path.Combine(directory, "package.json");
        if (!File.Exists(packageJsonFilePath))
        {
            return null;
        }

        string packageManager;
        try
        {
            // Example value: "pnpm@9.12.0+sha512.4abf7..."
            packageManager = JObject.Parse(File.ReadAllText(packageJsonFilePath))["packageManager"]?.ToString();
        }
        catch (JsonException ex)
        {
            Logger.LogWarning($"Cannot read the \"packageManager\" field of '{packageJsonFilePath}'. {ex.Message}");
            return null;
        }

        return packageManager?.Split('@')[0].Trim() switch
        {
            "npm" => NodePackageManager.Npm,
            "yarn" => NodePackageManager.Yarn,
            "pnpm" => NodePackageManager.Pnpm,
            _ => null
        };
    }

    private static NodePackageManager? FindPackageManagerByLockFileOrNull(string directory)
    {
        if (File.Exists(Path.Combine(directory, "package-lock.json")) ||
            File.Exists(Path.Combine(directory, "npm-shrinkwrap.json")))
        {
            return NodePackageManager.Npm;
        }

        if (File.Exists(Path.Combine(directory, "yarn.lock")))
        {
            return NodePackageManager.Yarn;
        }

        if (File.Exists(Path.Combine(directory, "pnpm-lock.yaml")))
        {
            return NodePackageManager.Pnpm;
        }

        return null;
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

    public static bool IsSafeVersion(string version)
    {
        return string.IsNullOrWhiteSpace(version) || SafeVersionRegex.IsMatch(version);
    }

    public static void EnsureSafeVersion(string version)
    {
        if (!IsSafeVersion(version))
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

    private static SemanticVersion FindVersionOrNull(string output)
    {
        var outputLines = output.Trim().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var outputLine in outputLines)
        {
            if (SemanticVersion.TryParse(outputLine.Trim(), out var version))
            {
                return version;
            }
        }

        return null;
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
