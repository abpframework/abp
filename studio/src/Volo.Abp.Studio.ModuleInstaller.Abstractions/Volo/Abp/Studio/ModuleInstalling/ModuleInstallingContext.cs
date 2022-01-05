using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using Volo.Abp.Studio.Packages;

namespace Volo.Abp.Studio.ModuleInstalling;

public class ModuleInstallingContext
{
    public string ModuleName { get; set; }

    public string TargetModule { get; set; }

    public bool WithSourceCode { get; set; }

    public bool AddToSolutionFile { get; set; }

    public string Version { get; set; }

    public List<EfCoreConfigurationMethodDeclaration> EfCoreConfigurationMethodDeclarations { get; }

    public List<PackageInfo> TargetModulePackages { get; protected set; }

    public List<PackageInfoWithAnalyze> ReferenceModulePackages { get; protected set; }

    public Dictionary<string, string> Options { get; }

    public IServiceProvider ServiceProvider { get; }

    public ModuleInstallingContext(
        string moduleName,
        string targetModule,
        bool withSourceCode,
        bool addToSolutionFile,
        string version,
        Dictionary<string, string> options,
        IServiceProvider serviceProvider)
    {
        ModuleName = moduleName;
        TargetModule = targetModule;
        WithSourceCode = withSourceCode;
        AddToSolutionFile = addToSolutionFile;
        Version = version;
        Options = options;

        TargetModulePackages = new List<PackageInfo>();
        ReferenceModulePackages = new List<PackageInfoWithAnalyze>();

        EfCoreConfigurationMethodDeclarations = new List<EfCoreConfigurationMethodDeclaration>();

        ServiceProvider = Check.NotNull(serviceProvider, nameof(serviceProvider));
    }

    public void AddEfCoreConfigurationMethodDeclaration(params EfCoreConfigurationMethodDeclaration[] methodNames)
    {
        foreach (var methodName in methodNames)
        {
            Check.NotNull(methodName, nameof(methodName));

            EfCoreConfigurationMethodDeclarations.Add(methodName);
        }
    }

    public void SetReferenceModulePackages([NotNull] List<PackageInfoWithAnalyze> referenceModulePackages)
    {
        Check.NotNull(referenceModulePackages, nameof(referenceModulePackages));

        ReferenceModulePackages = referenceModulePackages;
    }

    public void SetTargetModulePackages([NotNull] List<PackageInfo> targetModulePackages)
    {
        Check.NotNull(targetModulePackages, nameof(targetModulePackages));

        TargetModulePackages = targetModulePackages;
    }

    public string GetTargetSourceCodeFolder()
    {
        return CalculateTargetSourceCodeFolder(TargetModule, ModuleName);
    }

    public static string CalculateTargetSourceCodeFolder(string targetModule, string moduleName)
    {
        return Path.Combine(Path.GetDirectoryName(targetModule), "modules", moduleName);
    }
}
