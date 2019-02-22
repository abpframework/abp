using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace Volo.Abp.MultiTenancy
{
    [DependsOn(
        typeof(AbpDataModule)
        )]
    public class AbpMultiTenancyAbstractionsModule : AbpModule //TODO: Rename to AbpMultiTenancyModule?
    {

    }
}
