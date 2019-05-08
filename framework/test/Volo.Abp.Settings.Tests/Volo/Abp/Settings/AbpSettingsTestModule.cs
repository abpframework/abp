using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.Settings
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpSettingsModule),
        typeof(AbpTestBaseModule)
        )]
    public class AbpSettingsTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<SettingOptions>(options =>
            {
                options.ValueProviders.Add<TestSettingValueProvider>();
            });
        }
    }
}
