using Volo.Abp.Auditing;
using Volo.Abp.Modularity;

namespace Volo.Abp.Application
{
    [DependsOn(
        typeof(AbpAuditingModule)
        )]
    public class AbpDddApplicationContractsModule : AbpModule
    {
    }
}
