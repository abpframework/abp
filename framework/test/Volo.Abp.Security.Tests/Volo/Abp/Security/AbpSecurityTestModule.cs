using Volo.Abp.Modularity;

namespace Volo.Abp.Security
{
    [DependsOn(
        typeof(SecurityModule),
        typeof(TestBaseModule)
        )]
    public class AbpSecurityTestModule : AbpModule
    {

    }
}
