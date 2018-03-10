using Volo.Abp;
using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace AbpDesk
{
    [DependsOn(typeof(AbpCommonModule))]
    [DependsOn(typeof(AbpDddApplicationModule))]
    public class AbpDeskApplicationContractsModule : AbpModule
    {

    }
}
