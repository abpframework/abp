using Volo.Abp.Modularity;

namespace Volo.Abp.Uow
{
    [DependsOn(typeof(AbpUnitOfWorkModule))]
    public class AbpUnitOfWorkTestModule : AbpModule
    {

    }
}
