using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Caching
{
    public class AbpCachingModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();

            services.AddAssemblyOf<AbpCachingModule>();
        }
    }
}
