using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Ratings;

namespace Volo.CmsKit.MongoDB.Ratings;

public class MongoRatingRepository : MongoDbRepository<ICmsKitMongoDbContext, Rating, Guid>, IRatingRepository
{
    public MongoRatingRepository(IMongoDbContextProvider<ICmsKitMongoDbContext> dbContextProvider) : base(
        dbContextProvider)
    {
    }

    public virtual async Task<Rating> GetCurrentUserRatingAsync(string entityType, string entityId, Guid userId,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
        Check.NotNullOrWhiteSpace(entityId, nameof(entityId));

        var rating = await (await GetMongoQueryableAsync(cancellationToken))
            .FirstOrDefaultAsync(r => r.EntityType == entityType && r.EntityId == entityId && r.CreatorId == userId,
                GetCancellationToken(cancellationToken));

        return rating;
    }

    public virtual async Task<List<RatingWithStarCountQueryResultItem>> GetGroupedStarCountsAsync(string entityType,
        string entityId, CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
        Check.NotNullOrWhiteSpace(entityId, nameof(entityId));

        var query = (
            from rating in (await GetMongoQueryableAsync(cancellationToken))
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
