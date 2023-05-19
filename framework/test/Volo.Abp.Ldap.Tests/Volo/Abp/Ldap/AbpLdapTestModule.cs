using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;

namespace Volo.Abp.Ldap;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpLdapModule),
    typeof(AbpTestBaseModule)
)]
public class AbpLdapTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpSettingOptions>(options =>
        {
            options.ValueProviders.Add<TestLdapSettingValueProvider>();
        });
    }
}
