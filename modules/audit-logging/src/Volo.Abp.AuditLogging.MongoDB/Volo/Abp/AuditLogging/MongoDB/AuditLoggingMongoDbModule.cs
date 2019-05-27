using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Volo.Abp.AuditLogging.MongoDB
{
    [DependsOn(typeof(AuditLoggingDomainModule))]
    [DependsOn(typeof(MongoDbModule))]
    public class AuditLoggingMongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<AuditLoggingMongoDbContext>(options =>
            {
                options.AddRepository<AuditLog, MongoAuditLogRepository>();
            });
        }
    }
}
