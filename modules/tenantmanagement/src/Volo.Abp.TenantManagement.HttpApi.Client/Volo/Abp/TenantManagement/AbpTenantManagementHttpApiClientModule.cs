using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Volo.Abp.TenantManagement
{
    [DependsOn(
        typeof(AbpTenantManagementApplicationContractsModule), 
        typeof(AbpHttpClientModule))]
    public class AbpTenantManagementHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "AbpTenantManagement";

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClientProxies(
                typeof(AbpTenantManagementApplicationContractsModule).Assembly,
                RemoteServiceName
            );

            services.AddAssemblyOf<AbpTenantManagementHttpApiClientModule>();
        }
    }
}