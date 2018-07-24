using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Timing;

namespace Volo.Abp.BackgroundJobs.EntityFrameworkCore
{
    public class EfCoreBackgroundJobRepository : EfCoreRepository<IBackgroundJobsDbContext, BackgroundJobRecord, Guid>, IBackgroundJobRepository
    {
        protected IClock Clock { get; }

        public EfCoreBackgroundJobRepository(
            IDbContextProvider<IBackgroundJobsDbContext> dbContextProvider,
            IClock clock) 
            : base(dbContextProvider)
        {
            Clock = clock;
        }

        public async Task<List<BackgroundJobRecord>> GetWaitingListAsync(int maxResultCount)
        {
            return await DbSet
                .Where(t => !t.IsAbandoned && t.NextTryTime <= Clock.Now)
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.TryCount)
                .ThenBy(t => t.NextTryTime)
                .Take(maxResultCount)
                .ToListAsync();
        }
    }
}
