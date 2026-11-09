using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification;

public class AngularPwaSupportAdder : ITransientDependency
{
    protected ICmdHelper CmdHelper { get; }
    protected PackageJsonFileFinder PackageJsonFileFinder { get; }

    public AngularPwaSupportAdder(
        ICmdHelper cmdHelper,
        PackageJsonFileFinder packageJsonFileFinder)
    {
        CmdHelper = cmdHelper;
        PackageJsonFileFinder = packageJsonFileFinder;
    }

    public virtual void AddPwaSupport(string rootDirectory)
    {
        var fileList = PackageJsonFileFinder.Find(rootDirectory).Where(x => File.Exists(x.RemovePostFix("package.json") + "angular.json")).ToList();

        fileList.ForEach(AddPwaSupportToProject);
    }

    protected virtual void AddPwaSupportToProject(string filePath)
    {
        var directory = Path.GetDirectoryName(filePath).EnsureEndsWith(Path.DirectorySeparatorChar);
        var pwaMajorVersion = GetAngularCliMajorVersion(filePath);

        CmdHelper.RunCmd(
            $"ng add @angular/pwa@{pwaMajorVersion} --skip-confirmation",
            out var exitCode,
            workingDirectory: directory
        );

        if (exitCode != 0)
        {
            throw new AbpException(
                $"Failed to add Angular PWA support to '{filePath}'. 'ng add @angular/pwa@{pwaMajorVersion}' exited with code {exitCode}."
            );
        }
    }

    protected virtual string GetAngularCliMajorVersion(string packageJsonFilePath)
    {
        var packageJson = JObject.Parse(File.ReadAllText(packageJsonFilePath));

        var cliVersion =
            (packageJson["devDependencies"]?["@angular/cli"] ?? packageJson["dependencies"]?["@angular/cli"])?.ToString();

        var match = cliVersion == null ? null : Regex.Match(cliVersion, @"\d+");
        if (match == null || !match.Success)
        {
            throw new AbpException(
                $"Could not determine the @angular/cli version from '{packageJsonFilePath}', so the matching @angular/pwa version cannot be selected."
            );
        }

        return match.Value;
    }
}
