using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Features
{
    [DependsOn(
        typeof(AbpLocalizationAbstractionsModule),
        typeof(AbpMultiTenancyAbstractionsModule)
        )]
    public class AbpFeaturesModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            
        }
    }
}
