using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.CmsKit;

[DependsOn(
    typeof(AbpHttpClientModule),
    typeof(CmsKitCommonApplicationContractsModule)
    )]
public class CmsKitCommonHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(CmsKitCommonApplicationContractsModule).Assembly,
            CmsKitCommonRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CmsKitCommonHttpApiClientModule>();
        });
    }
}
