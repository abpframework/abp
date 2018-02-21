using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Caching
{
    [DependsOn(typeof(AbpCachingModule))]
    public class AbpCachingTestModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpCachingTestModule>();
        }
    }
}