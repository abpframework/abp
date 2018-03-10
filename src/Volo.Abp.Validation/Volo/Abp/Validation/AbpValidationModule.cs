using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Validation
{
    public class AbpValidationModule : AbpModule
    {
        public override void PreConfigureServices(IServiceCollection services)
        {
            services.OnRegistred(ValidationInterceptorRegistrar.RegisterIfNeeded);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpValidationModule>();
        }
    }
}
