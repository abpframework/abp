using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Volo.CmsKit.Public
{
    [DependsOn(
        typeof(PublicApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class PublicHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Public";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(PublicApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
