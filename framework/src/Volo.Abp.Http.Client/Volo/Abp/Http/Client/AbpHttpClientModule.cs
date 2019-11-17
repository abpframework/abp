using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Castle;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace Volo.Abp.Http.Client
{
    [DependsOn(
        typeof(AbpHttpModule),
        typeof(AbpCastleCoreModule),
        typeof(AbpThreadingModule),
        typeof(AbpMultiTenancyModule)
        )]
    public class AbpHttpClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            Configure<AbpRemoteServiceOptions>(configuration);
        }
    }
}
