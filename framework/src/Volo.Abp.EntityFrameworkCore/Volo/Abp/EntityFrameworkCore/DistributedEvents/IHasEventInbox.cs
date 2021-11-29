using Microsoft.EntityFrameworkCore;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents
{
    public interface IHasEventInbox : IEfCoreDbContext
    {
        DbSet<IncomingEventRecord> IncomingEvents { get; set; }
    }
}