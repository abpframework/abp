using Volo.Abp.Testing;

namespace Volo.Abp.Caching.StackExchangeRedis;

public abstract class AbpCachingStackExchangeRedisTestBase : AbpIntegratedTest<AbpCachingStackExchangeRedisTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
