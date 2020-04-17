using Volo.Abp.Modularity;

namespace Volo.Abp.Security
{
    [DependsOn(
        typeof(AbpSecurityModule),
        typeof(AbpTestBaseModule)
        )]
    public class AbpSecurityTestModule : AbpModule
    {

    }
}
