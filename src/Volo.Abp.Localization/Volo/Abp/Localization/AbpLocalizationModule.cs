using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Localization.Resources.Validation;
using Volo.Abp.Modularity;

namespace Volo.Abp.Localization
{
    public class AbpLocalizationModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            AbpStringLocalizerFactory.Replace(services);

            services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.AddJson<AbpValidationResource>("en");
            });

            services.AddAssemblyOf<AbpLocalizationModule>();
        }
    }
}
