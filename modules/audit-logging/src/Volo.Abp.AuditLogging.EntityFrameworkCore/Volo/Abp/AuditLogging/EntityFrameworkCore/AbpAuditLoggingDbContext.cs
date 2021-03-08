using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.AuditLogging.EntityFrameworkCore
{
    [ConnectionStringName(AbpAuditLoggingDbProperties.ConnectionStringName)]
    public class AbpAuditLoggingDbContext : AbpDbContext<AbpAuditLoggingDbContext>, IAuditLoggingDbContext
    {
        public DbSet<AuditLog> AuditLogs { get; set; }

        public AbpAuditLoggingDbContext(DbContextOptions<AbpAuditLoggingDbContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            AbpDbContextEvent.OnConfiguring(nameof(AbpAuditLoggingDbContext), optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureAuditLogging();

            AbpDbContextEvent.OnModelCreating(nameof(AbpAuditLoggingDbContext), builder);

        }

    }
}
