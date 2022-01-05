using JetBrains.Annotations;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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

    public virtual async Task DeleteManyAsync(Guid[] tagIds, CancellationToken cancellationToken = default)
    {
        var token = GetCancellationToken(cancellationToken);

        var collection = await GetCollectionAsync(token);
        await collection.DeleteManyAsync(Builders<EntityTag>.Filter.In(x => x.TagId, tagIds), token);
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
}
