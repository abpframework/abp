using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Abp.Timing;

namespace Volo.Abp.BackgroundJobs.MongoDB
{
    public class MongoBackgroundJobRepository : MongoDbRepository<IBackgroundJobsMongoDbContext, BackgroundJobRecord, Guid>, IBackgroundJobRepository
    {
        protected IClock Clock { get; }

        public MongoBackgroundJobRepository(
            IMongoDbContextProvider<IBackgroundJobsMongoDbContext> dbContextProvider,
            IClock clock)
            : base(dbContextProvider)
        {
            Clock = clock;
        }

        public virtual async Task<List<BackgroundJobRecord>> GetWaitingListAsync(int maxResultCount)
        {
            return await (await GetWaitingListQuery(maxResultCount)).ToListAsync();
        }

        protected virtual async Task<IMongoQueryable<BackgroundJobRecord>> GetWaitingListQuery(int maxResultCount)
        {
            var now = Clock.Now;
            return (await GetMongoQueryableAsync())
                .Where(t => !t.IsAbandoned && t.NextTryTime <= now)
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.TryCount)
                .ThenBy(t => t.NextTryTime)
                .Take(maxResultCount);
        }
    }
}
