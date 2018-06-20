using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Castle;
using Volo.Abp.Modularity;

namespace Volo.Abp.Http.Client
{
    [DependsOn(typeof(AbpHttpModule))]
    [DependsOn(typeof(AbpCastleCoreModule))]
    public class AbpHttpClientModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpHttpClientModule>();
        }
    }
}
