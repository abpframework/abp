using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Blogging
{
    [DependsOn(
        typeof(BloggingApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class BloggingHttpApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddStaticHttpClientProxies(typeof(BloggingApplicationContractsModule).Assembly,
                BloggingRemoteServiceConsts.RemoteServiceName);

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<BloggingHttpApiClientModule>();
            });
        }

    }
}
