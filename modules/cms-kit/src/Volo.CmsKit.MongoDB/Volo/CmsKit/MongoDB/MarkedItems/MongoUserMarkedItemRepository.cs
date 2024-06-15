using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.MarkedItems;

namespace Volo.CmsKit.MongoDB.MarkedItems;
public class MongoUserMarkedItemRepository : MongoDbRepository<ICmsKitMongoDbContext, UserMarkedItem, Guid>, IUserMarkedItemRepository
{
    public MongoUserMarkedItemRepository(IMongoDbContextProvider<ICmsKitMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<UserMarkedItem> FindAsync(Guid userId, [NotNull] string entityType, [NotNull] string entityId, CancellationToken cancellationToken = default)
    {
        Check.NotNull(userId, nameof(userId));
        Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
        Check.NotNull(entityId, nameof(entityId));
        
        var entity = await (await GetMongoQueryableAsync(cancellationToken))
            .Where(x =>
            x.CreatorId == userId &&
            x.EntityType == entityType &&
            x.EntityId == entityId)
        .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));

        return entity;
    }

    public virtual async Task<List<UserMarkedItem>> GetListForUserAsync(Guid userId, [NotNull] string entityType, CancellationToken cancellationToken = default)
    {
        Check.NotNull(userId, nameof(userId));
        Check.NotNullOrWhiteSpace(entityType, nameof(entityType));

        return await(await GetMongoQueryableAsync(cancellationToken))
            .Where(x =>
                x.CreatorId == userId &&
                x.EntityType == entityType)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// Retrieves an IQueryable representing the user's marked items based on the specified entity type and user ID.
    /// </summary>
    /// <param name="entityType">The type of entity to filter by.</param>
    /// <param name="userId">The ID of the user whose marked items are being retrieved.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An IQueryable representing the user's marked items filtered by the specified entity type and user ID.</returns>
    public virtual async Task<IQueryable<UserMarkedItem>> GetQueryForUserAsync([NotNull] string entityType, [NotNull] Guid userId, CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
        Check.NotNull(userId, nameof(userId));

        var queryable = await GetMongoQueryableAsync(cancellationToken);
        var query = queryable
            .Where(x => x.EntityType == entityType && x.CreatorId == userId);

        return query;
    }
}
