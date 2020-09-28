using Volo.Abp.Modularity;
using Volo.Abp.SecurityLog;

namespace Volo.Abp.Security
{
    [DependsOn(
        typeof(AbpSecurityModule),
        typeof(AbpTestBaseModule)
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
}
