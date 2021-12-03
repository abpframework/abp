using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.AuditLogging;

[DependsOn(
    typeof(AbpAuditLoggingEntityFrameworkCoreTestModule)
    )]
public class AbpAuditLoggingTestModule : AbpModule
{

}
