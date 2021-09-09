using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents
{
    public class DbContextEventInbox<TDbContext> : IDbContextEventInbox<TDbContext> 
        where TDbContext : IHasEventInbox
    {
        protected IDbContextProvider<TDbContext> DbContextProvider { get; }
        protected IClock Clock { get; }

        public DbContextEventInbox(
            IDbContextProvider<TDbContext> dbContextProvider,
            IClock clock)
        {
            DbContextProvider = dbContextProvider;
            Clock = clock;
        }

        [UnitOfWork]
        public virtual async Task EnqueueAsync(IncomingEventInfo incomingEvent)
        {
            var dbContext = (IHasEventInbox) await DbContextProvider.GetDbContextAsync();
            dbContext.IncomingEvents.Add(
                new IncomingEventRecord(incomingEvent)
            );
        }

        [UnitOfWork]
        public virtual async Task<List<IncomingEventInfo>> GetWaitingEventsAsync(int maxCount)
        {
            var dbContext = (IHasEventInbox) await DbContextProvider.GetDbContextAsync();
            
            var outgoingEventRecords = await dbContext
                .IncomingEvents
                .AsNoTracking()
                .Where(x => !x.Processed)
                .OrderBy(x => x.CreationTime)
                .Take(maxCount)
                .ToListAsync();
            
            return outgoingEventRecords
                .Select(x => x.ToIncomingEventInfo())
                .ToList();
        }

        public async Task MarkAsProcessedAsync(Guid id)
        {
            //TODO: Optimize?
            var dbContext = (IHasEventInbox) await DbContextProvider.GetDbContextAsync();
            var incomingEvent = await dbContext.IncomingEvents.FindAsync(id);
            if (incomingEvent != null)
            {
                incomingEvent.MarkAsProcessed(Clock.Now);
            }
        }
    }
}