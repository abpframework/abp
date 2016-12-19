using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Data;
using Volo.Abp.Data.MultiTenancy;
using Volo.Abp.Modularity;

namespace Volo.Abp.MultiTenancy
{
    public class AbpMultiTenancyModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Replace(ServiceDescriptor.Transient<IConnectionStringResolver, MultiTenantConnectionStringResolver>());
            services.AddAssemblyOf<AbpMultiTenancyModule>();
        }
    }
}
