using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Volo.Abp.Ui
{
    public class AbpUiModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.AddVirtualJson<AbpUiResource>("en", "/Localization/Resources/AbpUi");
            });

            services.AddAssemblyOf<AbpUiModule>();
        }
    }
}
