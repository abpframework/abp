using System;
using System.IO;
using System.Linq;
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

        CmdHelper.RunCmd("ng add @angular/pwa --skip-confirmation", workingDirectory: directory);
    }
}
