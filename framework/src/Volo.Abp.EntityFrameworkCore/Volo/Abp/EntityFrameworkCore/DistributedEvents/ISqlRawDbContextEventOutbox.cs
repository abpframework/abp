namespace Volo.Abp.EntityFrameworkCore.DistributedEvents;

public interface ISqlRawDbContextEventOutbox<TDbContext> : IDbContextEventOutbox<TDbContext>
    where TDbContext : IHasEventOutbox
{
}
