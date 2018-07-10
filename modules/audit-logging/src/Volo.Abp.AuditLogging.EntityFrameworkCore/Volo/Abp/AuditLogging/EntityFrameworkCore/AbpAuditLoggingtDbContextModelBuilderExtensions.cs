using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

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

                b.ConfigureExtraProperties();

                b.Property(x => x.ClientIpAddress).HasMaxLength(AuditLogConsts.MaxClientIpAddressLength).HasColumnName(nameof(AuditLog.ClientIpAddress));
                b.Property(x => x.ClientName).HasMaxLength(AuditLogConsts.MaxClientNameLength).HasColumnName(nameof(AuditLog.ClientName));
                b.Property(x => x.BrowserInfo).HasMaxLength(AuditLogConsts.MaxBrowserInfoLength).HasColumnName(nameof(AuditLog.BrowserInfo));
                b.Property(x => x.HttpMethod).HasMaxLength(AuditLogConsts.MaxHttpMethodLength).HasColumnName(nameof(AuditLog.HttpMethod));
                b.Property(x => x.Url).HasMaxLength(AuditLogConsts.MaxUrlLength).HasColumnName(nameof(AuditLog.Url));
                b.Property(x => x.Exceptions).HasMaxLength(AuditLogConsts.MaxExceptionsLength).HasColumnName(nameof(AuditLog.Exceptions));
                b.Property(x => x.Comments).HasMaxLength(AuditLogConsts.MaxCommentsLength).HasColumnName(nameof(AuditLog.Comments));
                b.Property(x => x.ExecutionDuration).HasColumnName(nameof(AuditLog.ExecutionDuration));
                b.Property(x => x.ImpersonatorTenantId).HasColumnName(nameof(AuditLog.ImpersonatorTenantId));
                b.Property(x => x.ImpersonatorUserId).HasColumnName(nameof(AuditLog.ImpersonatorUserId));
                b.Property(x => x.UserId).HasColumnName(nameof(AuditLog.UserId));
                b.Property(x => x.TenantId).HasColumnName(nameof(AuditLog.TenantId));

                b.HasMany<AuditLogAction>().WithOne().HasForeignKey(x => x.AuditLogId);
                b.HasMany<EntityChange>().WithOne().HasForeignKey(x => x.AuditLogId);

                b.HasIndex(x => new { x.TenantId, x.ExecutionTime });
                b.HasIndex(x => new { x.TenantId, x.UserId, x.ExecutionTime });
            });

            builder.Entity<AuditLogAction>(b =>
            {
                b.ToTable(tablePrefix + "AuditLogActions", schema);

                b.ConfigureExtraProperties();

                b.Property(x => x.AuditLogId).HasColumnName(nameof(AuditLogAction.AuditLogId));
                b.Property(x => x.ServiceName).HasMaxLength(AuditLogActionConsts.MaxServiceNameLength).HasColumnName(nameof(AuditLogAction.ServiceName));
                b.Property(x => x.MethodName).HasMaxLength(AuditLogActionConsts.MaxMethodNameLength).HasColumnName(nameof(AuditLogAction.MethodName));
                b.Property(x => x.Parameters).HasMaxLength(AuditLogActionConsts.MaxParametersLength).HasColumnName(nameof(AuditLogAction.Parameters));
                b.Property(x => x.ExecutionTime).HasColumnName(nameof(AuditLogAction.ExecutionTime));
                b.Property(x => x.ExecutionDuration).HasColumnName(nameof(AuditLogAction.ExecutionDuration));
                
                b.HasIndex(x => new { x.AuditLogId });
                b.HasIndex(x => new { x.TenantId, x.ServiceName, x.MethodName, x.ExecutionTime });
            });

            builder.Entity<EntityChange>(b =>
            {
                b.ToTable(tablePrefix + "EntityChanges", schema);

                b.ConfigureExtraProperties();

                b.Property(x => x.EntityTypeFullName).HasMaxLength(EntityChangeConsts.MaxEntityTypeFullNameLength).IsRequired().HasColumnName(nameof(EntityChange.EntityTypeFullName));
                b.Property(x => x.EntityId).HasMaxLength(EntityChangeConsts.MaxEntityIdLength).IsRequired().HasColumnName(nameof(EntityChange.EntityId));
                b.Property(x => x.AuditLogId).IsRequired().HasColumnName(nameof(EntityChange.AuditLogId));
                b.Property(x => x.ChangeTime).IsRequired().HasColumnName(nameof(EntityChange.ChangeTime));
                b.Property(x => x.ChangeType).IsRequired().HasColumnName(nameof(EntityChange.ChangeType));
                b.Property(x => x.TenantId).IsRequired().HasColumnName(nameof(EntityChange.TenantId));

                b.HasMany<EntityPropertyChange>().WithOne().HasForeignKey(x => x.EntityChangeId);

                b.HasIndex(x => new { x.AuditLogId });
                b.HasIndex(x => new { x.TenantId, x.EntityTypeFullName, x.EntityId });
            });

            builder.Entity<EntityPropertyChange>(b =>
            {
                b.ToTable(tablePrefix + "EntityPropertyChanges", schema);

                b.Property(x => x.NewValue).HasMaxLength(EntityPropertyChangeConsts.MaxNewValueLength).IsRequired().HasColumnName(nameof(EntityPropertyChange.NewValue));
                b.Property(x => x.PropertyName).HasMaxLength(EntityPropertyChangeConsts.MaxPropertyNameLength).IsRequired().HasColumnName(nameof(EntityPropertyChange.PropertyName));
                b.Property(x => x.PropertyTypeFullName).HasMaxLength(EntityPropertyChangeConsts.MaxPropertyTypeFullNameLength).IsRequired().HasColumnName(nameof(EntityPropertyChange.PropertyTypeFullName));
                b.Property(x => x.OriginalValue).HasMaxLength(EntityPropertyChangeConsts.MaxOriginalValueLength).HasColumnName(nameof(EntityPropertyChange.OriginalValue));

                b.HasIndex(x => new { x.EntityChangeId });
            });
        }
    }
}
