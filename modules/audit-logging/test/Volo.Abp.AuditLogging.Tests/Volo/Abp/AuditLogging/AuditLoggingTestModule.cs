using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.AuditLogging
{
    [DependsOn(
        typeof(AuditLoggingEntityFrameworkCoreTestModule)
        )]
    public class AuditLoggingTestModule : AbpModule
    {

    }
}
