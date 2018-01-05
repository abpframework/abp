using Volo.Abp.Modularity;

namespace Volo.Abp.MultiTenancy
{
    [DependsOn(typeof(AbpMultiTenancyDomainModule))]
    public class MultiTenancyTestModule : AbpModule
    {
        
    }
}