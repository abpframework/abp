using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;

namespace Volo.Abp.Emailing
{
    public class AbpEmailingModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<SettingOptions>(options =>
            {
                options.DefinitionProviders.Add<EmailSettingProvider>();
            });

            services.AddAssemblyOf<AbpEmailingModule>();
        }
    }
}
