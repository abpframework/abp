using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents;

public static class EventInboxDbContextModelBuilderExtensions
{
    public static void ConfigureEventInbox([NotNull] this ModelBuilder builder)
    {
        builder.Entity<IncomingEventRecord>(b =>
        {
            b.ToTable(AbpCommonDbProperties.DbTablePrefix + "EventInbox", AbpCommonDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.EventName).IsRequired().HasMaxLength(IncomingEventRecord.MaxEventNameLength);
            b.Property(x => x.EventData).IsRequired();

            b.HasIndex(x => new { x.Processed, x.CreationTime });
            b.HasIndex(x => x.MessageId);
        });
    }
}
