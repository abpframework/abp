using Volo.Abp.Modularity;

namespace Volo.Abp.MultiTenancy
{
    [DependsOn(typeof(AbpMultiTenancyModule))]
    public class MultiTenancyTestModule : AbpModule
    {
        
    }
}