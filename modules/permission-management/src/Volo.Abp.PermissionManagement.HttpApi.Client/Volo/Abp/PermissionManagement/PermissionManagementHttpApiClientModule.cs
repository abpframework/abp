using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Volo.Abp.PermissionManagement
{
    [DependsOn(
        typeof(PermissionManagementApplicationContractsModule),
        typeof(HttpClientModule))]
    public class PermissionManagementHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "AbpPermissionManagement";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(PermissionManagementApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
