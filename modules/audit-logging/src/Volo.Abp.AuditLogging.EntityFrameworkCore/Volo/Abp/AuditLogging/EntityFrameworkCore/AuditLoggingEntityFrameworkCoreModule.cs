using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.AuditLogging.EntityFrameworkCore
{
    [DependsOn(typeof(AuditLoggingDomainModule))]
    [DependsOn(typeof(EntityFrameworkCoreModule))]
    public class AuditLoggingEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<AbpAuditLoggingDbContext>(options =>
            {
                options.AddRepository<AuditLog, EfCoreAuditLogRepository>();
            });
        }
    }
}
