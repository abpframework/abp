using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.Ldap
{
    [DependsOn(
        typeof(AbpAutofacModule)
    )]
    public class AbpLdapModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            Configure<AbpLdapOptions>(configuration.GetSection("LDAP"));
        }
    }
}