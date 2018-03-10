using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace AbpDesk
{
    [DependsOn(typeof(AbpDddApplicationModule))]
    public class AbpDeskApplicationContractsModule : AbpModule
    {

    }
}
