using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Volo.Blogging
{
    [DependsOn(
        typeof(BloggingApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class BloggingHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Blogging";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(typeof(BloggingApplicationContractsModule).Assembly, RemoteServiceName);
        }

    }
}
