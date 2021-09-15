using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(
        typeof(MyProjectNameApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class MyProjectNameHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "MyProjectName";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(MyProjectNameApplicationContractsModule).Assembly,
                RemoteServiceName
            );

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<MyProjectNameHttpApiClientModule>();
            });

        }
    }
}
