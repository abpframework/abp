using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Volo.Abp.Cli
{
    [DependsOn(
        typeof(AbpDddDomainModule)
    )]
    public class AbpCliModule : AbpModule
    {

    }
}