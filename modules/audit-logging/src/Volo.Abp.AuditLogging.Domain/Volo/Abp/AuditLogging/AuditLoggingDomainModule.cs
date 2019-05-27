using Volo.Abp.Auditing;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Volo.Abp.AuditLogging
{
    [DependsOn(typeof(AuditingModule))]
    [DependsOn(typeof(DddDomainModule))]
    [DependsOn(typeof(AuditLoggingDomainSharedModule))]
    public class AuditLoggingDomainModule : AbpModule
    {

    }
}
