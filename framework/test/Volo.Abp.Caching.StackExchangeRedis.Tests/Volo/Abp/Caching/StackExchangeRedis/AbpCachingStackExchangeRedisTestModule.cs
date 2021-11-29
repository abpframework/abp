using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.Caching.StackExchangeRedis
{
    [DependsOn(
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(AbpTestBaseModule),
        typeof(AbpAutofacModule)
        )]
    public class AbpCachingStackExchangeRedisTestModule : AbpModule
    {
        
    }
}