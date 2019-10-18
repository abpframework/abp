using ProductManagement.Localization;
using Volo.Abp.Application;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Volo.Abp.VirtualFileSystem;

namespace ProductManagement
{
    [DependsOn(
        typeof(ProductManagementDomainSharedModule),
        typeof(AbpDddApplicationModule)
        )]
    public class ProductManagementApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<ProductManagementApplicationContractsModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<ProductManagementResource>()
                    .AddVirtualJson("/ProductManagement/Localization/ApplicationContracts");
            });
        }
    }
}
