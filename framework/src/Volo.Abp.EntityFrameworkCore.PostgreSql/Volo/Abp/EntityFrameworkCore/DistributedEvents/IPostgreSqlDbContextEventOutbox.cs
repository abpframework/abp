namespace Volo.Abp.EntityFrameworkCore.DistributedEvents
{
    public interface IPostgreSqlDbContextEventOutbox<TDbContext> : IDbContextEventOutbox<TDbContext>
        where TDbContext : IHasEventOutbox
    {
    }
}
