using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyModuleName
{
    [DependsOn(
        typeof(MyModuleNameApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class MyModuleNameHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "MyModuleName";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(MyModuleNameApplicationContractsModule).Assembly,
                RemoteServiceName
            );

            context.Services.AddAssemblyOf<MyModuleNameHttpApiClientModule>();
        }
    }
}
