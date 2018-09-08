using Volo.Abp.Modularity;

namespace Volo.Abp.Caching
{
    [DependsOn(typeof(AbpCachingModule))]
    public class AbpCachingTestModule : AbpModule
    {

    }
}