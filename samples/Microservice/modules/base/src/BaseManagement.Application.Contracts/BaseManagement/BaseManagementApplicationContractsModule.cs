using BaseManagement.Localization;
using Volo.Abp.Application;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Volo.Abp.VirtualFileSystem;

namespace BaseManagement
{
    [DependsOn(
        typeof(BaseManagementDomainSharedModule),
        typeof(AbpDddApplicationModule)
        )]
    public class BaseManagementApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<BaseManagementApplicationContractsModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<BaseManagementResource>()
                    .AddVirtualJson("/BaseManagement/Localization/ApplicationContracts");
            });
        }
    }
}
