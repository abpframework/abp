using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace ProductManagement
{
    [DependsOn(
        typeof(ProductManagementDomainSharedModule)
        )]
    public class ProductManagementDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<ProductManagementDomainModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Get<ProductManagementResource>().AddVirtualJson("/MyCompanyName/ProductManagement/Localization/Domain");
            });

            Configure<ExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("ProductManagement", typeof(ProductManagementResource));
            });
        }
    }
}
