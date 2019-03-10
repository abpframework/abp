using Microsoft.Extensions.DependencyInjection;
using Abp.FeatureManagement.Localization;
using Volo.Abp.Application;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Abp.FeatureManagement
{
    [DependsOn(
        typeof(FeatureManagementDomainSharedModule),
        typeof(AbpDddApplicationModule)
        )]
    public class FeatureManagementApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<PermissionOptions>(options =>
            {
                options.DefinitionProviders.Add<FeatureManagementPermissionDefinitionProvider>();
            });

            Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<FeatureManagementApplicationContractsModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<FeatureManagementResource>()
                    .AddVirtualJson("/Abp/FeatureManagement/Localization/ApplicationContracts");
            });
        }
    }
}
