using JetBrains.Annotations;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Tags;
using Tag = Volo.CmsKit.Tags.Tag;

namespace Volo.CmsKit.MongoDB.Tags
{
    public class MongoTagRepository : MongoDbRepository<ICmsKitMongoDbContext, Volo.CmsKit.Tags.Tag, Guid>, ITagRepository
    {
        public MongoTagRepository(IMongoDbContextProvider<ICmsKitMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<bool> AnyAsync(
            [NotNull] string entityType,
            [NotNull] string name,
            CancellationToken cancellationToken = default)
        {
            Check.NotNullOrEmpty(entityType, nameof(entityType));
            Check.NotNullOrEmpty(name, nameof(name));

            return await (await GetMongoQueryableAsync(cancellationToken))
                    .AnyAsync(x =>
                            x.EntityType == entityType &&
                            x.Name == name,
                        GetCancellationToken(cancellationToken));
        }

        public Task<Tag> GetAsync(
            [NotNull] string entityType,
            [NotNull] string name,
            CancellationToken cancellationToken = default)
        {
            return GetAsync(x =>
                x.EntityType == entityType &&
                x.Name == name,
               cancellationToken: GetCancellationToken(cancellationToken));
        }

        public Task<Tag> FindAsync(
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

            var entityTagIds = await (await GetMongoQueryableAsync<EntityTag>(cancellationToken))
                .Where(q => q.EntityId == entityId)
                .Select(q => q.TagId)
                .ToListAsync(cancellationToken: GetCancellationToken(cancellationToken));

            var query = (await GetMongoQueryableAsync(cancellationToken))
                            .Where(x =>
                                x.EntityType == entityType &&
                                entityTagIds.Contains(x.Id));

            var result = await query.ToListAsync(cancellationToken: GetCancellationToken(cancellationToken));
            return result;
        }


        public async Task<List<Tag>> GetListAsync(string filter)
        {
            return await (await GetQueryableByFilterAsync(filter)).ToListAsync();
        }

        public async Task<int> GetCountAsync(string filter)
        {
            return await (await GetQueryableByFilterAsync(filter)).CountAsync();
        }

        private async Task<IMongoQueryable<Tag>> GetQueryableByFilterAsync(string filter)
        {
            var mongoQueryable = await GetMongoQueryableAsync();

            if (!filter.IsNullOrWhiteSpace())
            {
                mongoQueryable = mongoQueryable.Where(x =>
                        x.Name.ToLower().Contains(filter) ||
                        x.EntityType.ToLower().Contains(filter));
            }

            return mongoQueryable;
        }
    }
}
