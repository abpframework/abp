using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using JetBrains.Annotations;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Abp.Timing;

namespace Volo.Abp.BackgroundJobs.MongoDB;

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

    public virtual async Task<List<BackgroundJobRecord>> GetWaitingListAsync([CanBeNull] string applicationName, int maxResultCount, CancellationToken cancellationToken = default)
    {
        return await (await GetWaitingListQuery(applicationName, maxResultCount, cancellationToken)).ToListAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual async Task<IQueryable<BackgroundJobRecord>> GetWaitingListQuery([CanBeNull] string applicationName, int maxResultCount, CancellationToken cancellationToken = default)
    {
        var now = Clock.Now;
        return (await GetQueryableAsync(cancellationToken))
            .Where(t => t.ApplicationName == applicationName)
            .Where(t => !t.IsAbandoned && t.NextTryTime <= now)
            .OrderByDescending(t => t.Priority)
            .ThenBy(t => t.TryCount)
            .ThenBy(t => t.NextTryTime)
            .Take(maxResultCount);
    }
}
