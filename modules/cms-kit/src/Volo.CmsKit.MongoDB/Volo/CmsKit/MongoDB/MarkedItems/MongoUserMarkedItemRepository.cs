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

    public virtual async Task<List<string>> GetEntityIdsFilteredByUserAsync([NotNull] Guid userId, [NotNull] string entityType, [CanBeNull] Guid? tenantId = null, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var userMarkedItemQueryable = await GetMongoQueryableAsync(GetCancellationToken(cancellationToken));

        var resultQueryable = userMarkedItemQueryable
                                .Where(x => x.CreatorId == userId
                                    && x.EntityType == entityType
                                    && x.TenantId == tenantId
                                )
                                .Select(s => s.EntityId);

        return await AsyncExecuter.ToListAsync(resultQueryable, GetCancellationToken(cancellationToken));
    }
}
