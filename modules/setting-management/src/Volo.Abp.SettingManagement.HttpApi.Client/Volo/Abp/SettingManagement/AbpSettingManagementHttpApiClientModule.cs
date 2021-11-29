using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.SettingManagement
{
    [DependsOn(
        typeof(AbpSettingManagementApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class AbpSettingManagementHttpApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddStaticHttpClientProxies(
                typeof(AbpSettingManagementApplicationContractsModule).Assembly,
                SettingManagementRemoteServiceConsts.RemoteServiceName
            );

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpSettingManagementHttpApiClientModule>();
            });
        }
    }
}
