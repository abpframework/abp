using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.SecurityLog;

namespace Volo.Abp.Security;

[DependsOn(
    typeof(AbpSecurityModule),
    typeof(AbpTestBaseModule),
    typeof(AbpAutofacModule)
    )]
public class AbpSecurityTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpSecurityLogOptions>(x =>
        {
            x.ApplicationName = "AbpSecurityTest";
        });
    }
}
