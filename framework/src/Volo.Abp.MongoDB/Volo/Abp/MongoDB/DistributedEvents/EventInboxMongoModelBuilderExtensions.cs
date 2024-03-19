using Volo.Abp.Data;

namespace Volo.Abp.MongoDB.DistributedEvents;

public static class EventInboxMongoModelBuilderExtensions
{
    public static void ConfigureEventInbox(this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<IncomingEventRecord>(b =>
        {
            b.CollectionName = AbpCommonDbProperties.DbTablePrefix + "EventInbox";
        });
    }
}
