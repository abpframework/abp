using JetBrains.Annotations;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Tags;
using Tag = Volo.CmsKit.Tags.Tag;

namespace Volo.CmsKit.MongoDB.Tags;

public class MongoTagRepository : MongoDbRepository<ICmsKitMongoDbContext, Volo.CmsKit.Tags.Tag, Guid>, ITagRepository
{
    public MongoTagRepository(IMongoDbContextProvider<ICmsKitMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<bool> AnyAsync(
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

    
    public virtual async Task<List<PopularTag>> GetPopularTagsAsync(string entityType, int maxCount, CancellationToken cancellationToken = default)
    {
        var tags = await (await GetMongoQueryableAsync(cancellationToken))
            .Where(x => x.EntityType == entityType)
            .Select(x => new { x.Id, x.Name })
            .ToListAsync(cancellationToken: GetCancellationToken(cancellationToken));

        var tagIds = tags.Select(x => x.Id);

        var entityTagCounts = await (await GetMongoQueryableAsync<EntityTag>(cancellationToken))
            .Where(q => tagIds.Contains(q.TagId))
            .GroupBy(q => q.TagId)
            .Select(q => new { TagId = q.Key, Count = q.Count() })
            .OrderByDescending(q => q.Count)
            .Take(maxCount)
            .ToListAsync(cancellationToken: GetCancellationToken(cancellationToken));

        return (from entityTagId in entityTagCounts
            let tag = tags.FirstOrDefault(x => x.Id == entityTagId.TagId)
            where tag != null
            select new PopularTag(tag.Id, tag.Name, entityTagId.Count)).ToList();
    }


    public virtual async Task<List<Tag>> GetListAsync(string filter, 
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string sorting = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableByFilterAsync(filter, cancellationToken))
            .OrderBy(sorting.IsNullOrEmpty() ? $"{nameof(Tag.CreationTime)}" : sorting)
            .As<IMongoQueryable<Tag>>()
            .PageBy<Tag, IMongoQueryable<Tag>>(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<int> GetCountAsync(string filter, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableByFilterAsync(filter, cancellationToken)).CountAsync(GetCancellationToken(cancellationToken));
    }

    private async Task<IMongoQueryable<Tag>> GetQueryableByFilterAsync(string filter, CancellationToken cancellationToken = default)
    {
        var mongoQueryable = await GetMongoQueryableAsync(cancellationToken: cancellationToken);

        if (!filter.IsNullOrWhiteSpace())
        {
            mongoQueryable = mongoQueryable.Where(x =>
                    x.Name.ToLower().Contains(filter.ToLower()) ||
                    x.EntityType.ToLower().Contains(filter));
        }

        return mongoQueryable;
    }
}
