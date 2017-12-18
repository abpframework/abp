using Volo.Abp.Modularity;

namespace Volo.Abp.TestBase
{
    [DependsOn(typeof(AbpCommonModule))]
    public class AbpTestBaseModule : AbpModule
    {
        
    }
}
