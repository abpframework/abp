using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace ProductManagement
{
    [DependsOn(
        typeof(ProductManagementApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class ProductManagementHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "ProductManagement";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(ProductManagementApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
