using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Localization;
using Volo.Abp.Application;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
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
            Configure<PermissionOptions>(options =>
            {
                options.DefinitionProviders.Add<ProductManagementPermissionDefinitionProvider>();
            });

            Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<ProductManagementApplicationContractsModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<ProductManagementResource>()
                    .AddVirtualJson("/MyCompanyName/ProductManagement/Localization/ApplicationContracts");
            });
        }
    }
}
