using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Volo.Abp.TenantManagement
{
    [DependsOn(
        typeof(TenantManagementApplicationContractsModule), 
        typeof(HttpClientModule))]
    public class TenantManagementHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "AbpTenantManagement";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(TenantManagementApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}