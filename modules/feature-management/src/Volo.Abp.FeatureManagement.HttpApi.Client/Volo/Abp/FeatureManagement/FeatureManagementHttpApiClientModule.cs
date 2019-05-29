using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Volo.Abp.FeatureManagement
{
    [DependsOn(
        typeof(FeatureManagementApplicationContractsModule),
        typeof(HttpClientModule))]
    public class FeatureManagementHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "AbpFeatureManagement";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(FeatureManagementApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
