using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Castle;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;
using Volo.Abp.Validation;
using Volo.Abp.ExceptionHandling;

namespace Volo.Abp.Http.Client
{
    [DependsOn(
        typeof(AbpHttpModule),
        typeof(AbpCastleCoreModule),
        typeof(AbpThreadingModule),
        typeof(AbpMultiTenancyModule),
        typeof(AbpValidationModule),
        typeof(AbpExceptionHandlingModule)
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
