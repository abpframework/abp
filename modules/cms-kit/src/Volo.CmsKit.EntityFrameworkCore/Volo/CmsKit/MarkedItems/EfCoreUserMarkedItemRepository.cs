using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.MarkedItems;
public class EfCoreUserMarkedItemRepository : EfCoreRepository<ICmsKitDbContext, UserMarkedItem, Guid>, IUserMarkedItemRepository
{
    public EfCoreUserMarkedItemRepository(IDbContextProvider<ICmsKitDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<UserMarkedItem> FindAsync(Guid userId, [NotNull] string entityType, [NotNull] string entityId, CancellationToken cancellationToken = default)
    {
        Check.NotNull(userId, nameof(userId));
        Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
        Check.NotNull(entityId, nameof(entityId));

        var entity = await(await GetDbSetAsync())
        .Where(x =>
            x.CreatorId == userId &&
            x.EntityType == entityType &&
            x.EntityId == entityId)
        .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));

        return entity;
    }

    public async Task<List<UserMarkedItem>> GetListForUserAsync(Guid userId, [NotNull] string entityType, CancellationToken cancellationToken = default)
    {
        Check.NotNull(userId, nameof(userId));
        Check.NotNullOrWhiteSpace(entityType, nameof(entityType));

        return await(await GetDbSetAsync())
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
    public async Task<IQueryable<UserMarkedItem>> GetQueryForUserAsync([NotNull] string entityType, [NotNull] Guid userId, CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
        Check.NotNull(userId, nameof(userId));

        var dbSet = await GetDbSetAsync();

        var query = dbSet
            .Where(x => x.EntityType == entityType && x.CreatorId == userId);

        return query;
    }
}
