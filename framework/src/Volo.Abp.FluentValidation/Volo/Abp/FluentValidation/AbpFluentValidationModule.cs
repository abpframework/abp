using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.FluentValidation
{
    public class AbpFluentValidationModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.OnRegistred(FluentValidationInterceptorRegistrar.RegisterIfNeeded);
        }
    }
}
