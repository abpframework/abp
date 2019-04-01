using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using ProductManagement.Localization;

namespace ProductManagement
{
    [DependsOn(
        typeof(AbpLocalizationModule)
        )]
    public class ProductManagementDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Add<ProductManagementResource>("en");
            });
        }
    }
}
