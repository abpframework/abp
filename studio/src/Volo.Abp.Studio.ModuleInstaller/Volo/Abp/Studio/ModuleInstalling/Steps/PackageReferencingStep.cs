using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Studio.Packages;
using Volo.Abp.Studio.Packages.Modifying;
using Volo.Abp.Studio.Analyzing.Models.Module;

namespace Volo.Abp.Studio.ModuleInstalling.Steps;

public class PackageReferencingStep : ModuleInstallingPipelineStep
{
    public override async Task ExecuteAsync(ModuleInstallingContext context)
    {
        var _abpModuleFileManager = context.ServiceProvider.GetRequiredService<IAbpModuleFileManager>();

        foreach (var referencePackage in context.ReferenceModulePackages)
        {
            var targetPackages = GetTargetPackages(context.TargetModulePackages, referencePackage);

            foreach (var targetPackage in targetPackages)
            {
                await AddReferenceAsync(context, targetPackage, referencePackage);

                var targetAbpModulePath = FindAbpModuleFile(targetPackage.Path);

                await _abpModuleFileManager.AddDependency(targetAbpModulePath, FindAbpModuleName(referencePackage));
            }
        }
    }

    private async Task AddReferenceAsync(
        ModuleInstallingContext context,
        PackageInfo targetPackage,
        PackageInfoWithAnalyze referencePackage)
    {
        var _csprojFileManager = context.ServiceProvider.GetRequiredService<ICsprojFileManager>();
        var csprojFilePath = targetPackage.Path.RemovePostFix(PackageConsts.FileExtension) + ".csproj";

        if (context.WithSourceCode)
        {
            var referenceProjectPath = Directory.GetFiles(context.GetTargetSourceCodeFolder(),
                $"*{referencePackage.Name}.csproj",
                SearchOption.AllDirectories).FirstOrDefault();

            if (referenceProjectPath == null)
            {
                return;
            }

            await _csprojFileManager.AddProjectReferenceAsync(
                csprojFilePath,
                referenceProjectPath);
        }
        else
        {
            await _csprojFileManager.AddPackageReferenceAsync(
                csprojFilePath,
                referencePackage.Name,
                context.Version);
        }
    }

    private string FindAbpModuleFile(string targetPackagePath)
    {
        return Directory.GetFiles(Path.GetDirectoryName(targetPackagePath), "*Module.cs",
                SearchOption.AllDirectories)
            .FirstOrDefault();
    }

    private string FindAbpModuleName(PackageInfoWithAnalyze package)
    {
        var abpModuleModel = package.Analyze.Contents.Where(y =>
            y.ContentType == AbpModuleModel.ContentTypeName
        ).Cast<AbpModuleModel>().First();

        return abpModuleModel.Namespace + "." + abpModuleModel.Name;
    }

    private List<PackageInfo> GetTargetPackages(List<PackageInfo> targetModulePackages,
        PackageInfoWithAnalyze referencePackage)
    {
        if (PackageTypes.IsHostProject(referencePackage.Role))
        {
            return new List<PackageInfo>();
        }

        if (PackageTypes.IsUiProject(referencePackage.Role))
        {
            var useHostBlazorServerForMvcPackages = targetModulePackages.All(p => p.Role != PackageTypes.HostMvc);
            var targetHostType =
                PackageTypes.GetHostTypeOfUi(referencePackage.Role, useHostBlazorServerForMvcPackages);

            return targetModulePackages.Where(p => p.Role == targetHostType).ToList();
        }

        return targetModulePackages.Where(p => p.Role == referencePackage.Role).ToList();
    }
}
