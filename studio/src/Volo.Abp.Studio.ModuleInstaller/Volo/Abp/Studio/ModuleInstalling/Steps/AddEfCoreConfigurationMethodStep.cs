using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Cli.ProjectModification;
using Volo.Abp.Studio.Packages;
using Volo.Abp.Studio.Packages.Modifying;
using Volo.Abp.Studio.Analyzing.Models.Module;

namespace Volo.Abp.Studio.ModuleInstalling.Steps;

public class AddEfCoreConfigurationMethodStep : ModuleInstallingPipelineStep
{
    public override async Task ExecuteAsync(ModuleInstallingContext context)
    {
        var efCoreProject = context.TargetModulePackages.FirstOrDefault(p => p.Role == PackageTypes.EntityFrameworkCore);

        if (efCoreProject == null)
        {
            return;
        }

        var efCoreProjectCsprojPath = efCoreProject.Path.RemovePostFix(PackageConsts.FileExtension) + ".csproj";

        var _derivedClassFinder = context.ServiceProvider.GetRequiredService<DerivedClassFinder>();
        var _dbContextFileBuilderConfigureAdder = context.ServiceProvider.GetRequiredService<DbContextFileBuilderConfigureAdder>();

        var dbContextFile = _derivedClassFinder.Find(efCoreProjectCsprojPath, "AbpDbContext").FirstOrDefault();

        if (dbContextFile == null)
        {
            return;
        }

        foreach (var declaration in context.EfCoreConfigurationMethodDeclarations)
        {
            _dbContextFileBuilderConfigureAdder.Add(dbContextFile, declaration.Namespace + ":" + declaration.MethodName);
        }
    }
}
