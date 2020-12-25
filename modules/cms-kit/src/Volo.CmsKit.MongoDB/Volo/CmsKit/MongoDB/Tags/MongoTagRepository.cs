using JetBrains.Annotations;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.MongoDB.Tags
{
    public class MongoTagRepository : MongoDbRepository<ICmsKitMongoDbContext, Volo.CmsKit.Tags.Tag, Guid>, ITagRepository
    {
        public MongoTagRepository(IMongoDbContextProvider<ICmsKitMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public Task<bool> AnyAsync(
            [NotNull] string entityType,
            [NotNull] string name,
            Guid? tenantId,
            CancellationToken cancellationToken = default)
        {
            return GetMongoQueryable()
                    .AnyAsync(x =>
                            x.EntityType == entityType &&
                            x.Name == name &&
                            x.TenantId == tenantId,
                        cancellationToken);
        }

        public Task<Volo.CmsKit.Tags.Tag> GetAsync(
            [NotNull] string entityType,
            [NotNull] string name,
            Guid? tenantId,
            CancellationToken cancellationToken = default)
        {
            return GetAsync(x =>
                x.EntityType == entityType &&
                x.Name == name &&
                x.TenantId == tenantId,
               cancellationToken: cancellationToken);
        }

        public Task<Volo.CmsKit.Tags.Tag> FindAsync(
            [NotNull] string entityType,
            [NotNull] string name,
            Guid? tenantId,
            CancellationToken cancellationToken = default)
        {
            return FindAsync(x =>
                x.EntityType == entityType &&
                x.Name == name &&
                x.TenantId == tenantId,
               cancellationToken: cancellationToken);
        }

        public virtual async Task<List<Volo.CmsKit.Tags.Tag>> GetAllRelatedTagsAsync(
            [NotNull] string entityType,
            [NotNull] string entityId,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            var entityTagIds = await DbContext.EntityTags.AsQueryable()
                .Where(q => q.EntityId == entityId && q.TenantId == tenantId)
                .Select(q => q.EntityId)
                .ToListAsync(cancellationToken: GetCancellationToken(cancellationToken));

            var query = GetMongoQueryable()
                .Where(x => x.EntityType == entityType &&
                            x.TenantId == tenantId &&
                            entityTagIds.Contains(x.Id.ToString()));

            var result = await query.ToListAsync(cancellationToken: GetCancellationToken(cancellationToken));
            return result;
        }
    }
}
