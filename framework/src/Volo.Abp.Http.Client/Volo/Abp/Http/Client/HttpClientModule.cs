using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Castle;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace Volo.Abp.Http.Client
{
    [DependsOn(
        typeof(HttpModule),
        typeof(CastleCoreModule),
        typeof(ThreadingModule),
        typeof(MultiTenancyModule)
        )]
    public class HttpClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            Configure<RemoteServiceOptions>(configuration);
        }
    }
}
