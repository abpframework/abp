using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Acme.BookStore.BookManagement
{
    [DependsOn(
        typeof(BookManagementApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class BookManagementHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "BookManagement";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(BookManagementApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
