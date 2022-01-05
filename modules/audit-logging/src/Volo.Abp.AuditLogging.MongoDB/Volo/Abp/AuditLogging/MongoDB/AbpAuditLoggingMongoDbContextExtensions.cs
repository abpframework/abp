using Volo.Abp.MongoDB;

namespace Volo.Abp.AuditLogging.MongoDB;

public static class AbpAuditLoggingMongoDbContextExtensions
{
    public static void ConfigureAuditLogging(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<AuditLog>(b =>
        {
            b.CollectionName = AbpAuditLoggingDbProperties.DbTablePrefix + "AuditLogs";
        });
    }
}
