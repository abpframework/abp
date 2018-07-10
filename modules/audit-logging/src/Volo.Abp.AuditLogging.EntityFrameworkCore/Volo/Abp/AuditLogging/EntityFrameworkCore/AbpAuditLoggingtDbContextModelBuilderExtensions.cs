using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Volo.Abp.AuditLogging.EntityFrameworkCore
{
    public static class AbpAuditLoggingtDbContextModelBuilderExtensions
    {
        public static void ConfigureAuditLogging(
            [NotNull] this ModelBuilder builder,
            [CanBeNull] string tablePrefix = AbpAuditLoggingConsts.DefaultDbTablePrefix,
            [CanBeNull] string schema = AbpAuditLoggingConsts.DefaultDbSchema)
        {
            Check.NotNull(builder, nameof(builder));

            if (tablePrefix == null)
            {
                tablePrefix = "";
            }

            builder.Entity<AuditLog>(b =>
            {
                b.ToTable(tablePrefix + "AuditLogs", schema);
                
                b.Property(x => x.ClientIpAddress).HasMaxLength(AuditLogConsts.MaxClientIpAddressLength).HasColumnName(nameof(AuditLog.ClientIpAddress));
                b.Property(x => x.ClientName).HasMaxLength(AuditLogConsts.MaxClientNameLength); //TODO: Add HasColumnNames
                b.Property(x => x.BrowserInfo).HasMaxLength(AuditLogConsts.MaxBrowserInfoLength);
                b.Property(x => x.Exceptions).HasMaxLength(AuditLogConsts.MaxExceptionLength);

                b.HasOne<EntityChange>().WithMany().HasForeignKey(x => x.EntityChanges);
                b.HasMany<AuditLogAction>().WithOne().HasForeignKey(x => x.);

                b.HasIndex(x => new { x.TenantId, x.UserId, x.ExecutionTime });
            });

            builder.Entity<EntityChange>(b =>
            {
                b.ToTable(tablePrefix + "EntityChanges", schema);

                b.Property(x => x.EntityTypeFullName).IsRequired();
                b.Property(x => x.EntityId).IsRequired();

                b.HasIndex(x => new { x.TenantId, x.EntityTypeFullName});
            });

            builder.Entity<AuditLogAction>(b =>
            {
                b.ToTable(tablePrefix + "AuditLogActions", schema);

                b.Property(x => x.ServiceName).HasMaxLength(AuditLogActionConsts.MaxServiceNameLength);
                b.Property(x => x.MethodName).HasMaxLength(AuditLogActionConsts.MaxMethodNameLength);
                b.Property(x => x.Parameters).HasMaxLength(AuditLogActionConsts.MaxParametersLength);

                b.HasIndex(x => new { x.ServiceName, x.ExecutionTime});
            });


        }
    }
}
