using Volo.Abp;
using Volo.Abp.Modularity;

namespace AbpDesk
{
    [DependsOn(typeof(AbpCommonModule))]
    [DependsOn(typeof(AbpDddModule))]
    public class AbpDeskApplicationContractsModule : AbpModule
    {

    }
}
