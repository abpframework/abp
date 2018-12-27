using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Castle;
using Volo.Abp.Modularity;

namespace Volo.Abp.Http.Client
{
    [DependsOn(typeof(AbpHttpModule))]
    [DependsOn(typeof(AbpCastleCoreModule))]
    public class AbpHttpClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            Configure<RemoteServiceOptions>(configuration);
        }
    }
}
