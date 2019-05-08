using Volo.Abp.Application;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.TenantManagement
{
    [DependsOn(typeof(AbpDddApplicationModule))]
    [DependsOn(typeof(AbpTenantManagementDomainSharedModule))]
    public class AbpTenantManagementApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpTenantManagementApplicationContractsModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<AbpTenantManagementResource>()
                    .AddVirtualJson("/Volo/Abp/TenantManagement/Localization/ApplicationContracts");
            });
        }
    }
}