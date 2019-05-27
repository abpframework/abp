using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.Features
{
    [DependsOn(
        typeof(FeaturesModule),
        typeof(TestBaseModule),
        typeof(AutofacModule)
        )]
    public class AbpFeaturesTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            
        }
    }
}
