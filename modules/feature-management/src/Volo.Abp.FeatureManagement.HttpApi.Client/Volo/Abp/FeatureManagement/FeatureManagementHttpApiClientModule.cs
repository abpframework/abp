using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Abp.FeatureManagement
{
    [DependsOn(
        typeof(FeatureManagementApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class FeatureManagementHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "FeatureManagement";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(FeatureManagementApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
