using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.MultiTenancy
{
    public class AbpMultiTenancyAbstractionsModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpMultiTenancyAbstractionsModule>();
        }
    }
}
