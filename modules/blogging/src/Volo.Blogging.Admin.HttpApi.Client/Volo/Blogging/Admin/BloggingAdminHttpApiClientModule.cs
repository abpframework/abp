using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Blogging.Admin
{
    [DependsOn(
        typeof(BloggingAdminApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class BloggingAdminHttpApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddStaticHttpClientProxies(typeof(BloggingAdminApplicationContractsModule).Assembly,
                BloggingAdminRemoteServiceConsts.RemoteServiceName);

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<BloggingAdminHttpApiClientModule>();
            });
        }

    }
}
