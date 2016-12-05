using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.DependencyInjection;

namespace Volo.Abp
{
    public class AbpKernelModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpKernelModule>();
        }
    }
}
