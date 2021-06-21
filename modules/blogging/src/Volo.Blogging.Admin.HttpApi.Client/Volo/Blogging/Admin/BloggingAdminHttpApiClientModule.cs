using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Volo.Blogging.Admin
{
    [DependsOn(
        typeof(BloggingAdminApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class BloggingAdminHttpApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(typeof(BloggingAdminApplicationContractsModule).Assembly,
                BloggingAdminRemoteServiceConsts.RemoteServiceName);
        }

    }
}
