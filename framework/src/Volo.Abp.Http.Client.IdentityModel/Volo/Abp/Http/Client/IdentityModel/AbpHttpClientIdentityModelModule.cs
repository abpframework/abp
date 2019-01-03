using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Http.Client.IdentityModel
{
    [DependsOn(
        typeof(AbpHttpClientModule)
        )]
    public class AbpHttpClientIdentityModelModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            Configure<IdentityClientOptions>(configuration);
        }
    }
}
