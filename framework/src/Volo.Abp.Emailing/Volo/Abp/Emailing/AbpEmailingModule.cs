using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;

namespace Volo.Abp.Emailing
{
    public class AbpEmailingModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<SettingOptions>(options =>
            {
                options.DefinitionProviders.Add<EmailSettingProvider>();
            });

            context.Services.AddAssemblyOf<AbpEmailingModule>();
        }
    }
}
