using System;
using Volo.Abp.MongoDB;

namespace Volo.Abp.AuditLogging.MongoDB
{
    public static class AbpAuditLoggingMongoDbContextExtensions
    {
        public static void ConfigureAuditLogging(
            this IMongoModelBuilder builder,
            Action<MongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new AuditLoggingMongoModelBuilderConfigurationOptions();

            optionsAction?.Invoke(options);

            builder.Entity<AuditLog>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "AuditLogging";
            });
        }
    }
}
