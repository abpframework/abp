using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.AuditLogging.EntityFrameworkCore
{
    [ConnectionStringName(AbpAuditLoggingConsts.ConnectionStringName)]
    public interface IAuditLoggingDbContext : IEfCoreDbContext
    {
        DbSet<AuditLog> AuditLogs { get; set; }
        DbSet<AuditLogAction> AuditLogActions { get; set; }
    }
}