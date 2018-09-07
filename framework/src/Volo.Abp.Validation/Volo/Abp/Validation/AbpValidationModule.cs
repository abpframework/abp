using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Validation
{
    public class AbpValidationModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.OnRegistred(ValidationInterceptorRegistrar.RegisterIfNeeded);
        }
    }
}
