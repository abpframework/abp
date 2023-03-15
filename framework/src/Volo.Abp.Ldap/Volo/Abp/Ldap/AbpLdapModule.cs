using Volo.Abp.Modularity;
using Volo.Abp.Settings;

namespace Volo.Abp.Ldap;

[DependsOn(
    typeof(AbpLdapAbstractionsModule),
    typeof(AbpSettingsModule))]
public class AbpLdapModule : AbpModule
{
   
}
