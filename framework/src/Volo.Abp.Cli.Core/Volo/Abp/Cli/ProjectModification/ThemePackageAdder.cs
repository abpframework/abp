using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification;

public class ThemePackageAdder : ITransientDependency
{
    protected NpmHelper NpmHelper { get; }
    protected PackageJsonFileFinder PackageJsonFileFinder { get; }

    public ThemePackageAdder(NpmHelper npmHelper, PackageJsonFileFinder packageJsonFileFinder)
    {
        NpmHelper = npmHelper;
        PackageJsonFileFinder = packageJsonFileFinder;
    }

    public void AddNpmPackage(string rootDirectory, string package, string version)
    {
        var packageJsonFilePaths = PackageJsonFileFinder.Find(rootDirectory)
            .Where(x => !File.Exists(x.RemovePostFix("package.json") + "angular.json"))
            .ToList();

        AddPackage(packageJsonFilePaths, package, version);
    }

    public void AddAngularPackage(string rootDirectory, string package, string version)
    {
        var angularPackageJsonFilePaths = PackageJsonFileFinder.Find(rootDirectory)
            .Where(x => File.Exists(x.RemovePostFix("package.json") + "angular.json"))
            .ToList();

        AddPackage(angularPackageJsonFilePaths, package, version);
    }

    private void AddPackage(List<string> packageJsonFilePaths, string package, string version)
    {
        if (!packageJsonFilePaths.Any() || string.IsNullOrWhiteSpace(package))
        {
            return;
        }

        foreach (var packageJsonFilePath in packageJsonFilePaths)
        {
            var directory = Path.GetDirectoryName(packageJsonFilePath).EnsureEndsWith(Path.DirectorySeparatorChar);
            
            NpmHelper.YarnAddPackage(package, version, directory);
        }
    }
}
