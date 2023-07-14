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

namespace Volo.CmsKit.Tags;

public class EfCoreEntityTagRepository : EfCoreRepository<ICmsKitDbContext, EntityTag>, IEntityTagRepository
{
    public EfCoreEntityTagRepository(IDbContextProvider<ICmsKitDbContext> dbContextProvider) : base(
        dbContextProvider)
    {
    }

    public virtual async Task DeleteManyAsync(Guid[] tagIds, string entityId, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var dbSet = dbContext.Set<EntityTag>();
        dbSet.RemoveRange(dbSet.Where(x => tagIds.Contains(x.TagId) && x.EntityId == entityId));
        await dbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
    }

    public virtual Task<EntityTag> FindAsync(
        [NotNull] Guid tagId,
        [NotNull] string entityId,
        [CanBeNull] Guid? tenantId,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrEmpty(entityId, nameof(entityId));

        return base.FindAsync(x =>
                x.TagId == tagId &&
                x.EntityId == entityId &&
                x.TenantId == tenantId,
            cancellationToken: GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<string>> GetEntityIdsFilteredByTagAsync(
        [NotNull] Guid tagId,
        [CanBeNull] Guid? tenantId,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbContextAsync()).Set<EntityTag>()
            .Where(q => q.TagId == tagId && q.TenantId == tenantId)
            .Select(q => q.EntityId)
            .ToListAsync(cancellationToken: GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<string>> GetEntityIdsFilteredByTagNameAsync(
        [NotNull] string tagName,
        [NotNull] string entityType,
        [CanBeNull] Guid? tenantId = null,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var result = from et in dbContext.Set<EntityTag>()
                     join t in dbContext.Set<Tag>() on et.TagId equals t.Id
                     where t.Name == tagName
                           && t.EntityType == entityType
                           && et.TenantId == tenantId
                           && t.TenantId == tenantId
                           && !t.IsDeleted
                     select et.EntityId;
        return await result.ToListAsync(cancellationToken: GetCancellationToken(cancellationToken));
    }
}