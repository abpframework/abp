using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.Settings
{
    [DependsOn(
        typeof(AutofacModule),
        typeof(SettingsModule),
        typeof(TestBaseModule)
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
