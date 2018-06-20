using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Serialization;
using Volo.Abp.Threading;

namespace Volo.Abp.Caching
{
    [DependsOn(typeof(AbpThreadingModule))]
    [DependsOn(typeof(AbpSerializationModule))]
    [DependsOn(typeof(AbpMultiTenancyAbstractionsModule))]
    public class AbpCachingModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();

            services.AddAssemblyOf<AbpCachingModule>();

            services.AddSingleton(typeof(IDistributedCache<>), typeof(DistributedCache<>));
        }
    }
}
