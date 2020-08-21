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
            Configure<AbpLdapOptions>(options =>
            {
                options.ServerHost = "192.168.0.3";
                options.ServerPort = 389;
                options.BaseDc = "dc=abp,dc=io";
                options.UserName = "admin";
                options.Password = "123qwe";
            });
        }
    }
}
