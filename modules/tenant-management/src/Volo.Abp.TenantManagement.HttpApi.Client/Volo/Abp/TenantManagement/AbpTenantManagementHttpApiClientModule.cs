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

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(AbpTenantManagementApplicationContractsModule).Assembly,
                RemoteServiceName
            );

            context.Services.AddAssemblyOf<AbpTenantManagementHttpApiClientModule>();
        }
    }
}