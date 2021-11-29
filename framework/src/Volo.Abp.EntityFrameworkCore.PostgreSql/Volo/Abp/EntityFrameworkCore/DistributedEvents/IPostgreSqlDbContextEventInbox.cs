namespace Volo.Abp.EntityFrameworkCore.DistributedEvents
{
    public interface IPostgreSqlDbContextEventInbox<TDbContext> : IDbContextEventInbox<TDbContext>
        where TDbContext : IHasEventInbox
    {

    }
}
