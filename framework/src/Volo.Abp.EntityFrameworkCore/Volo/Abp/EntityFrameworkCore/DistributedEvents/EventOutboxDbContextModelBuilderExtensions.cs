using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents
{
    public static class EventOutboxDbContextModelBuilderExtensions
    {
        public static void ConfigureEventOutbox([NotNull] this ModelBuilder builder)
        {
            builder.Entity<OutgoingEventRecord>(b =>
            {
                b.ToTable(AbpCommonDbProperties.DbTablePrefix + "EventOutbox", AbpCommonDbProperties.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.EventName).IsRequired().HasMaxLength(OutgoingEventRecord.MaxEventNameLength);
                b.Property(x => x.EventData).IsRequired();

                b.HasIndex(x => x.CreationTime);
            });
        }
    }
}