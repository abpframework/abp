using JetBrains.Annotations;
using Volo.Abp.Data;

namespace Volo.Abp.MongoDB.DistributedEvents;

public static class EventOutboxMongoModelBuilderExtensions
{
    public static void ConfigureEventOutbox([NotNull] this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<OutgoingEventRecord>(b =>
        {
            b.CollectionName = AbpCommonDbProperties.DbTablePrefix + "EventOutbox";
        });
    }
}
