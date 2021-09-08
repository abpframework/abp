using System.Threading.Tasks;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Guids;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents
{
    public class DbContextEventOutbox<TDbContext> : IEventOutbox 
        where TDbContext : IHasEventOutbox
    {
        protected IDbContextProvider<TDbContext> DbContextProvider { get; }
        protected IGuidGenerator GuidGenerator { get; }

        public DbContextEventOutbox(
            IDbContextProvider<TDbContext> dbContextProvider,
            IGuidGenerator guidGenerator)
        {
            DbContextProvider = dbContextProvider;
            GuidGenerator = guidGenerator;
        }
        
        public async Task EnqueueAsync(string eventName, byte[] eventData)
        {
            var dbContext = (IHasEventOutbox) await DbContextProvider.GetDbContextAsync();
            dbContext.OutgoingEventRecords.Add(
                new OutgoingEventRecord(GuidGenerator.Create(), eventName, eventData)
            );
        }
    }
}