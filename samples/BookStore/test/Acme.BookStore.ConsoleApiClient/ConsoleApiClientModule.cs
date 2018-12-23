using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Acme.BookStore.ConsoleApiClient
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpHttpClientModule),
        typeof(BookStoreApplicationModule)
        )]
    public class ConsoleApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            context.Services.Configure<RemoteServiceOptions>(configuration);

            context.Services.AddHttpClientProxies(
                typeof(BookStoreApplicationModule).Assembly
            );
        }
    }
}
