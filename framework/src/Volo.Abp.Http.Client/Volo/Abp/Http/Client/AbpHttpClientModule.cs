using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Castle;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Volo.Abp.Http.Client
{
    [DependsOn(
        typeof(AbpHttpModule),
        typeof(AbpCastleCoreModule),
        typeof(AbpThreadingModule)
        )]
    public class AbpHttpClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            Configure<RemoteServiceOptions>(configuration);
        }
    }
}
