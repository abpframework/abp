using Microsoft.EntityFrameworkCore;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents
{
    public interface IHasEventOutbox
    {
        DbSet<OutgoingEventRecord> OutgoingEventRecords { get; set; }
    }
}