using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using Abp.FeatureManagement.Localization;

namespace Abp.FeatureManagement
{
    [DependsOn(
        typeof(AbpLocalizationModule)
        )]
    public class FeatureManagementDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Add<FeatureManagementResource>("en");
            });
        }
    }
}
