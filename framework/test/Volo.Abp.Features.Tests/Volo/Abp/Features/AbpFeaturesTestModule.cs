using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.Features
{
    [DependsOn(
        typeof(AbpFeaturesModule),
        typeof(AbpTestBaseModule),
        typeof(AbpAutofacModule)
        )]
    public class AbpFeaturesTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            
        }
    }
}
