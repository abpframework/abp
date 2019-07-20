using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Volo.Abp.Account
{
    [DependsOn(
        typeof(AbpAccountApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class AbpAccountHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Account";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(typeof(AbpAccountApplicationContractsModule).Assembly, RemoteServiceName);
        }
    }
}