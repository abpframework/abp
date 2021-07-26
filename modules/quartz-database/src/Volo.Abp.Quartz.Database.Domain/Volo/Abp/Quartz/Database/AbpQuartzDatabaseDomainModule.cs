using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Volo.Abp.Quartz.Database
{
    [DependsOn(
        typeof(AbpDddDomainModule)
    )]
    public class AbpQuartzDatabaseDomainModule : AbpModule
    {

    }
}
