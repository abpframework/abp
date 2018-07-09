using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.AuditLogging.MongoDB
{
    [ConnectionStringName("AbpAuditLogging")]
    public interface IAuditLoggingMongoDbContext : IAbpMongoDbContext
    {
        IMongoCollection<AuditLog> AuditLogs { get; }
    }
}
