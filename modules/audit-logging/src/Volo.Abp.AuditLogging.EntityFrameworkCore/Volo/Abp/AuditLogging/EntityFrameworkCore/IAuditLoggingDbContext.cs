using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.AuditLogging.EntityFrameworkCore
{
    [ConnectionStringName("AbpAuditLogging")]
    public interface IAuditLoggingDbContext : IEfCoreDbContext
    {
        DbSet<AuditLog> AuditLogs { get; set; }
    }
}