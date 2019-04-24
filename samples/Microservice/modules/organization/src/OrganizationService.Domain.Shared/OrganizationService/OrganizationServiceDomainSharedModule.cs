using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using OrganizationService.Localization;

namespace OrganizationService
{
    [DependsOn(
        typeof(AbpLocalizationModule)
        )]
    public class OrganizationServiceDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Add<OrganizationServiceResource>("en");
            });
        }
    }
}
