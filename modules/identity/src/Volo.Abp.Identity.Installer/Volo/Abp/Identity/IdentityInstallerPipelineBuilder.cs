﻿using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Studio.ModuleInstalling;

namespace Volo.Abp.Identity
{
    [Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
    [ExposeServices(typeof(IModuleInstallingPipelineBuilder))]
    public class IdentityInstallerPipelineBuilder : ModuleInstallingPipelineBuilderBase, IModuleInstallingPipelineBuilder, ITransientDependency
    {
        public async Task<ModuleInstallingPipeline> BuildAsync(ModuleInstallingContext context)
        {
            context.AddEfCoreConfigurationMethodDeclaration(
                new EfCoreConfigurationMethodDeclaration(
                    "Volo.Abp.Identity.EntityFrameworkCore",
                    "ConfigureIdentity"
                    )
                );

            return GetBasePipeline(context);
        }
    }
}
