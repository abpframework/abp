using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Volo.Abp.Identity
{
    [DependsOn(
        typeof(IdentityApplicationContractsModule),
        typeof(HttpClientModule))]
    public class IdentityHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "AbpIdentity";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(IdentityApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}