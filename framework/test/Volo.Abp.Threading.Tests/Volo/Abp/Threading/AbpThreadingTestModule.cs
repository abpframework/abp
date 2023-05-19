using Volo.Abp.Modularity;

namespace Volo.Abp.Threading;

[DependsOn(
    typeof(AbpThreadingModule),
    typeof(AbpTestBaseModule)
)]
public class AbpThreadingTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }
}
