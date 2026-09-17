using Volo.Abp.Modularity;

namespace Volo.Abp.Timing;

[DependsOn(
    typeof(AbpTimingModule),
    typeof(AbpTestBaseModule)
)]
public class AbpTimingTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }
}
