using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace BaseManagement
{
    [DependsOn(
        typeof(BaseManagementApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class BaseManagementHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "BaseManagement";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(BaseManagementApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
