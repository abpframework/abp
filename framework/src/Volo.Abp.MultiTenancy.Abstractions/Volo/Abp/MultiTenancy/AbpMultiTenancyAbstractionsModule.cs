using Volo.Abp.Authorization;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace Volo.Abp.MultiTenancy
{
    [DependsOn(
        typeof(AbpDataModule),
        typeof(AbpAuthorizationModule)
        )]
    public class AbpMultiTenancyAbstractionsModule : AbpModule //TODO: Rename to AbpMultiTenancyModule?
    {

    }
}
