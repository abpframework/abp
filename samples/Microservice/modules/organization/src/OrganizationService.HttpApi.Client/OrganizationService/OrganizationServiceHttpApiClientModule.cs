using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace OrganizationService
{
    [DependsOn(
        typeof(OrganizationServiceApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class OrganizationServiceHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "OrganizationService";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(OrganizationServiceApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
