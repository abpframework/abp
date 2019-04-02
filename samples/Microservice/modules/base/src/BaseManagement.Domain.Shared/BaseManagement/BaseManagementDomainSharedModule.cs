using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using BaseManagement.Localization;

namespace BaseManagement
{
    [DependsOn(
        typeof(AbpLocalizationModule)
        )]
    public class BaseManagementDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Add<BaseManagementResource>("en");
            });
        }
    }
}
