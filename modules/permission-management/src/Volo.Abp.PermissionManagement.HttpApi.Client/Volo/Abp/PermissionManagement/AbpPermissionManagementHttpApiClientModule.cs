using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Volo.Abp.PermissionManagement
{
    [DependsOn(
        typeof(AbpPermissionManagementApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class AbpPermissionManagementHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "AbpPermissionManagement";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(AbpPermissionManagementApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
