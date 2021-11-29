using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Castle.DynamicProxy;
using Volo.Abp.Modularity;

namespace Volo.Abp.Castle
{
    public class AbpCastleCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddTransient(typeof(AbpAsyncDeterminationInterceptor<>));
        }
    }
}
