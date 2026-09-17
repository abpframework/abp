using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Volo.Docs.Common;

namespace Volo.Docs
{
    [DependsOn(
        typeof(DocsCommonApplicationContractsModule),
        typeof(AbpHttpClientModule)
    )]
    public class DocsCommonHttpApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddStaticHttpClientProxies(typeof(DocsCommonApplicationContractsModule).Assembly, DocsCommonRemoteServiceConsts.RemoteServiceName);

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<DocsCommonHttpApiClientModule>();
            });
        }
    }
}
