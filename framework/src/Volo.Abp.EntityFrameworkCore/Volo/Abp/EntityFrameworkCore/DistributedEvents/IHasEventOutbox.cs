using Microsoft.EntityFrameworkCore;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents
{
    public interface IHasEventOutbox : IEfCoreDbContext
    {
        DbSet<OutgoingEventRecord> OutgoingEvents { get; set; }
    }
}