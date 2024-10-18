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
        .FirstOrDefaultAsync(x =>
            x.CreatorId == userId &&
            x.EntityType == entityType &&
            x.EntityId == entityId,
            GetCancellationToken(cancellationToken));

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

    public async Task<List<string>> GetEntityIdsFilteredByUserAsync([NotNull] Guid userId, [NotNull] string entityType, [CanBeNull] Guid? tenantId = null, CancellationToken cancellationToken = default)
    {
        Check.NotNull(userId, nameof(userId));
        Check.NotNullOrWhiteSpace(entityType, nameof(entityType));

        var dbContext = await GetDbContextAsync();
        var result = from umi in dbContext.Set<UserMarkedItem>()
                     where umi.CreatorId == userId
                           && umi.EntityType == entityType
                           && umi.TenantId == tenantId
                     select umi.EntityId;

        return await result.ToListAsync(cancellationToken: GetCancellationToken(cancellationToken));
    }
}
