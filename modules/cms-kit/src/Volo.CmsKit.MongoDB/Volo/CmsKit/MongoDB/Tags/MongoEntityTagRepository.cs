using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MongoDB.Driver;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.MongoDB.Tags;

public class MongoEntityTagRepository : MongoDbRepository<ICmsKitMongoDbContext, EntityTag>, IEntityTagRepository
{
    public MongoEntityTagRepository(IMongoDbContextProvider<ICmsKitMongoDbContext> dbContextProvider) : base(
        dbContextProvider)
    {
    }

    public virtual async Task DeleteManyAsync(Guid[] tagIds, string entityId, CancellationToken cancellationToken = default)
    {
        var token = GetCancellationToken(cancellationToken);

        var collection = await GetCollectionAsync(token);

        var builder = Builders<EntityTag>.Filter;
        var filter = builder.And(
                builder.In(x => x.TagId, tagIds),
                builder.Eq(x => x.EntityId, entityId)
           );

        await collection.DeleteManyAsync(filter, token);
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
        var blogPostQueryable = (await GetQueryableAsync())
            .Where(q => q.TagId == tagId && q.TenantId == tenantId)
            .Select(q => q.EntityId);

        return await AsyncExecuter.ToListAsync(blogPostQueryable, GetCancellationToken(cancellationToken));
    }

    public async Task<List<string>> GetEntityIdsFilteredByTagNameAsync(
        [NotNull] string tagName,
        [NotNull] string entityType,
        [CanBeNull] Guid? tenantId = null,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var entityTagQueryable = await GetMongoQueryableAsync(GetCancellationToken(cancellationToken));
        var tagQueryable = dbContext.Tags.AsQueryable();

        var resultQueryable = entityTagQueryable
                                .Join(
                                    tagQueryable,
                                    o => o.TagId,
                                    i => i.Id,
                                    (entityTag, tag) => new { entityTag, tag })
                                .Where(x => x.tag.EntityType == entityType
                                    && x.entityTag.TenantId == tenantId
                                    && x.tag.TenantId == tenantId
                                    && x.tag.IsDeleted == false
                                )
                                .Select(s => s.entityTag.EntityId);

        return await AsyncExecuter.ToListAsync(resultQueryable, GetCancellationToken(cancellationToken));
    }
}
