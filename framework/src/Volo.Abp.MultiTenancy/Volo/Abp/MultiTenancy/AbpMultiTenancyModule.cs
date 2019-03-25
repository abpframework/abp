using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Security;

namespace Volo.Abp.MultiTenancy
{
    [DependsOn(
        typeof(AbpDataModule),
        typeof(AbpSecurityModule)
        )]
    public class AbpMultiTenancyModule : AbpModule
    {

    }
}
