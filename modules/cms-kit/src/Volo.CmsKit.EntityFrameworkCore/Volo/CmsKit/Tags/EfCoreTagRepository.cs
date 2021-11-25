using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.Tags;

public class EfCoreTagRepository : EfCoreRepository<ICmsKitDbContext, Tag, Guid>, ITagRepository
{
    public EfCoreTagRepository(IDbContextProvider<ICmsKitDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<bool> AnyAsync(
        [NotNull] string entityType,
        [NotNull] string name,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrEmpty(entityType, nameof(entityType));
        Check.NotNullOrEmpty(name, nameof(name));

        return await (await GetDbSetAsync()).AnyAsync(x =>
                x.EntityType == entityType &&
                x.Name == name,
            GetCancellationToken(cancellationToken));
    }

    public virtual Task<Tag> GetAsync(
        [NotNull] string entityType,
        [NotNull] string name,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrEmpty(entityType, nameof(entityType));
        Check.NotNullOrEmpty(name, nameof(name));

        return GetAsync(x =>
                x.EntityType == entityType &&
                x.Name == name,
            cancellationToken: GetCancellationToken(cancellationToken));
    }

    public virtual Task<Tag> FindAsync(
        [NotNull] string entityType,
        [NotNull] string name,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrEmpty(entityType, nameof(entityType));
        Check.NotNullOrEmpty(name, nameof(name));

        return FindAsync(x =>
                x.EntityType == entityType &&
                x.Name == name,
            cancellationToken: GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<Tag>> GetAllRelatedTagsAsync(
        [NotNull] string entityType,
        [NotNull] string entityId,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrEmpty(entityType, nameof(entityType));
        Check.NotNullOrEmpty(entityId, nameof(entityId));

        var entityTagIds = await (await GetDbContextAsync()).Set<EntityTag>()
            .Where(q => q.EntityId == entityId)
            .Select(q => q.TagId)
            .ToListAsync(cancellationToken: GetCancellationToken(cancellationToken));

        var query = (await GetDbSetAsync())
            .Where(x => x.EntityType == entityType &&
                        entityTagIds.Contains(x.Id));

        return await query.ToListAsync(cancellationToken: GetCancellationToken(cancellationToken));
    }

    public async Task<List<Tag>> GetListAsync(string filter)
    {
        return await (await GetQueryableByFilterAsync(filter)).ToListAsync();
    }

    public async Task<int> GetCountAsync(string filter)
    {
        return await (await GetQueryableByFilterAsync(filter)).CountAsync();
    }

    private async Task<IQueryable<Tag>> GetQueryableByFilterAsync(string filter)
    {
        return (await GetQueryableAsync())
            .WhereIf(
                !filter.IsNullOrEmpty(),
                x =>
                    x.Name.ToLower().Contains(filter.ToLower()) ||
                    x.EntityType.ToLower().Contains(filter.ToLower()));
    }
}
