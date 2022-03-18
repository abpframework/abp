using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.Ratings;

public class EfCoreRatingRepository : EfCoreRepository<ICmsKitDbContext, Rating, Guid>, IRatingRepository
{
    public EfCoreRatingRepository(IDbContextProvider<ICmsKitDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<Rating> GetCurrentUserRatingAsync(string entityType, string entityId, Guid userId,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
        Check.NotNullOrWhiteSpace(entityId, nameof(entityId));

        var rating = await (await GetDbSetAsync()).FirstOrDefaultAsync(
            r => r.EntityType == entityType && r.EntityId == entityId && r.CreatorId == userId,
            GetCancellationToken(cancellationToken));

        return rating;
    }

    public async Task<List<RatingWithStarCountQueryResultItem>> GetGroupedStarCountsAsync(string entityType,
        string entityId, CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
        Check.NotNullOrWhiteSpace(entityId, nameof(entityId));

        var query = (
            from rating in (await GetDbSetAsync())
            where rating.EntityType == entityType && rating.EntityId == entityId
            group rating by rating.StarCount
            into g
            select new RatingWithStarCountQueryResultItem
            {
                StarCount = g.Key,
                Count = g.Count()
            }
        ).OrderByDescending(r => r.StarCount);

        var ratings = await query.ToListAsync(GetCancellationToken(cancellationToken));

        return ratings;
    }
}
