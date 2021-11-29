using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.AuditLogging.EntityFrameworkCore
{
    public static class AbpAuditLoggingDbContextModelBuilderExtensions
    {
        public static void ConfigureAuditLogging(
            [NotNull] this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<AuditLog>(b =>
            {
                b.ToTable(AbpAuditLoggingDbProperties.DbTablePrefix + "AuditLogs", AbpAuditLoggingDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.ApplicationName).HasMaxLength(AuditLogConsts.MaxApplicationNameLength).HasColumnName(nameof(AuditLog.ApplicationName));
                b.Property(x => x.ClientIpAddress).HasMaxLength(AuditLogConsts.MaxClientIpAddressLength).HasColumnName(nameof(AuditLog.ClientIpAddress));
                b.Property(x => x.ClientName).HasMaxLength(AuditLogConsts.MaxClientNameLength).HasColumnName(nameof(AuditLog.ClientName));
                b.Property(x => x.ClientId).HasMaxLength(AuditLogConsts.MaxClientIdLength).HasColumnName(nameof(AuditLog.ClientId));
                b.Property(x => x.CorrelationId).HasMaxLength(AuditLogConsts.MaxCorrelationIdLength).HasColumnName(nameof(AuditLog.CorrelationId));
                b.Property(x => x.BrowserInfo).HasMaxLength(AuditLogConsts.MaxBrowserInfoLength).HasColumnName(nameof(AuditLog.BrowserInfo));
                b.Property(x => x.HttpMethod).HasMaxLength(AuditLogConsts.MaxHttpMethodLength).HasColumnName(nameof(AuditLog.HttpMethod));
                b.Property(x => x.Url).HasMaxLength(AuditLogConsts.MaxUrlLength).HasColumnName(nameof(AuditLog.Url));
                b.Property(x => x.HttpStatusCode).HasColumnName(nameof(AuditLog.HttpStatusCode));

                b.Property(x => x.Comments).HasMaxLength(AuditLogConsts.MaxCommentsLength).HasColumnName(nameof(AuditLog.Comments));
                b.Property(x => x.ExecutionDuration).HasColumnName(nameof(AuditLog.ExecutionDuration));
                b.Property(x => x.ImpersonatorTenantId).HasColumnName(nameof(AuditLog.ImpersonatorTenantId));
                b.Property(x => x.ImpersonatorUserId).HasColumnName(nameof(AuditLog.ImpersonatorUserId));
                b.Property(x => x.UserId).HasColumnName(nameof(AuditLog.UserId));
                b.Property(x => x.UserName).HasMaxLength(AuditLogConsts.MaxUserNameLength).HasColumnName(nameof(AuditLog.UserName));
                b.Property(x => x.TenantId).HasColumnName(nameof(AuditLog.TenantId));

                b.HasMany(a => a.Actions).WithOne().HasForeignKey(x => x.AuditLogId).IsRequired();
                b.HasMany(a => a.EntityChanges).WithOne().HasForeignKey(x => x.AuditLogId).IsRequired();

                b.HasIndex(x => new { x.TenantId, x.ExecutionTime });
                b.HasIndex(x => new { x.TenantId, x.UserId, x.ExecutionTime });

                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<AuditLogAction>(b =>
            {
                b.ToTable(AbpAuditLoggingDbProperties.DbTablePrefix + "AuditLogActions", AbpAuditLoggingDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.AuditLogId).HasColumnName(nameof(AuditLogAction.AuditLogId));
                b.Property(x => x.ServiceName).HasMaxLength(AuditLogActionConsts.MaxServiceNameLength).HasColumnName(nameof(AuditLogAction.ServiceName));
                b.Property(x => x.MethodName).HasMaxLength(AuditLogActionConsts.MaxMethodNameLength).HasColumnName(nameof(AuditLogAction.MethodName));
                b.Property(x => x.Parameters).HasMaxLength(AuditLogActionConsts.MaxParametersLength).HasColumnName(nameof(AuditLogAction.Parameters));
                b.Property(x => x.ExecutionTime).HasColumnName(nameof(AuditLogAction.ExecutionTime));
                b.Property(x => x.ExecutionDuration).HasColumnName(nameof(AuditLogAction.ExecutionDuration));

                b.HasIndex(x => new { x.AuditLogId });
                b.HasIndex(x => new { x.TenantId, x.ServiceName, x.MethodName, x.ExecutionTime });

                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<EntityChange>(b =>
            {
                b.ToTable(AbpAuditLoggingDbProperties.DbTablePrefix + "EntityChanges", AbpAuditLoggingDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.EntityTypeFullName).HasMaxLength(EntityChangeConsts.MaxEntityTypeFullNameLength).IsRequired().HasColumnName(nameof(EntityChange.EntityTypeFullName));
                b.Property(x => x.EntityId).HasMaxLength(EntityChangeConsts.MaxEntityIdLength).IsRequired().HasColumnName(nameof(EntityChange.EntityId));
                b.Property(x => x.AuditLogId).IsRequired().HasColumnName(nameof(EntityChange.AuditLogId));
                b.Property(x => x.ChangeTime).IsRequired().HasColumnName(nameof(EntityChange.ChangeTime));
                b.Property(x => x.ChangeType).IsRequired().HasColumnName(nameof(EntityChange.ChangeType));
                b.Property(x => x.TenantId).HasColumnName(nameof(EntityChange.TenantId));

                b.HasMany(a => a.PropertyChanges).WithOne().HasForeignKey(x => x.EntityChangeId);

                b.HasIndex(x => new { x.AuditLogId });
                b.HasIndex(x => new { x.TenantId, x.EntityTypeFullName, x.EntityId });

                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<EntityPropertyChange>(b =>
            {
                b.ToTable(AbpAuditLoggingDbProperties.DbTablePrefix + "EntityPropertyChanges", AbpAuditLoggingDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.NewValue).HasMaxLength(EntityPropertyChangeConsts.MaxNewValueLength).HasColumnName(nameof(EntityPropertyChange.NewValue));
                b.Property(x => x.PropertyName).HasMaxLength(EntityPropertyChangeConsts.MaxPropertyNameLength).IsRequired().HasColumnName(nameof(EntityPropertyChange.PropertyName));
                b.Property(x => x.PropertyTypeFullName).HasMaxLength(EntityPropertyChangeConsts.MaxPropertyTypeFullNameLength).IsRequired().HasColumnName(nameof(EntityPropertyChange.PropertyTypeFullName));
                b.Property(x => x.OriginalValue).HasMaxLength(EntityPropertyChangeConsts.MaxOriginalValueLength).HasColumnName(nameof(EntityPropertyChange.OriginalValue));

                b.HasIndex(x => new { x.EntityChangeId });

                b.ApplyObjectExtensionMappings();
            });

            builder.TryConfigureObjectExtensions<AbpAuditLoggingDbContext>();
        }
    }
}
