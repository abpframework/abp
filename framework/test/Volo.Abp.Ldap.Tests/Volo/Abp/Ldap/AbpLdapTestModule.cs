using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.Ldap
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpLdapModule),
        typeof(AbpTestBaseModule)
    )]
    public class AbpLdapTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLdapOptions>(settings =>
            {

            });
        }
    }
}
