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
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.MarkedItems;
using Volo.CmsKit.Tags;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.MongoDB.Blogs;

public class MongoBlogPostRepository : MongoDbRepository<CmsKitMongoDbContext, BlogPost, Guid>, IBlogPostRepository
{
    private readonly MarkedItemManager _markedItemManager;
    private EntityTagManager _entityTagManager;
    public MongoBlogPostRepository(
        IMongoDbContextProvider<CmsKitMongoDbContext> dbContextProvider,
        MarkedItemManager markedItemManager,
        EntityTagManager entityTagManager) : base(
        dbContextProvider)
    {
        _markedItemManager = markedItemManager;
        _entityTagManager = entityTagManager;
    }

    public virtual async Task<BlogPost> GetBySlugAsync(Guid blogId, [NotNull] string slug,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrEmpty(slug, nameof(slug));

        var token = GetCancellationToken(cancellationToken);

        var blogPost = await GetAsync(x =>
                x.BlogId == blogId &&
                x.Slug.ToLower() == slug,
            cancellationToken: token);

        blogPost.Author = await (await GetQueryableAsync<CmsUser>(token)).FirstOrDefaultAsync(x => x.Id == blogPost.AuthorId, token);

        return blogPost;
    }

    public virtual async Task<int> GetCountAsync(
        string filter = null,
        Guid? blogId = null,
        Guid? authorId = null,
        Guid? tagId = null,
        Guid? favoriteUserId = null,
        BlogPostStatus? statusFilter = null,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        var tagFilteredEntityIds = await GetEntityIdsByTagId(tagId, cancellationToken);

        var favoriteUserFilteredEntityIds = await GetFavoriteEntityIdsByUserId(favoriteUserId, cancellationToken);
        return await (await GetQueryableAsync(cancellationToken))
            .WhereIf(tagFilteredEntityIds.Any(), x => tagFilteredEntityIds.Contains(x.Id))
            .WhereIf(favoriteUserFilteredEntityIds.Any(), x => favoriteUserFilteredEntityIds.Contains(x.Id))
            .WhereIf(!string.IsNullOrWhiteSpace(filter), x => x.Title.Contains(filter) || x.Slug.Contains(filter))
            .WhereIf(blogId.HasValue, x => x.BlogId == blogId)
            .WhereIf(authorId.HasValue, x => x.AuthorId == authorId)
            .WhereIf(statusFilter.HasValue, x => x.Status == statusFilter)
            .CountAsync(cancellationToken);
    }

    public virtual async Task<List<BlogPost>> GetListAsync(
        string filter = null,
        Guid? blogId = null,
        Guid? authorId = null,
        Guid? tagId = null,
        Guid? favoriteUserId = null,
        BlogPostStatus? statusFilter = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string sorting = null,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        
        var dbContext = await GetDbContextAsync(cancellationToken);
        var blogPostQueryable = await GetQueryableAsync(cancellationToken);

        var tagFilteredEntityIds = await GetEntityIdsByTagId(tagId, cancellationToken);

        var favoriteUserFilteredEntityIds = await GetFavoriteEntityIdsByUserId(favoriteUserId, cancellationToken);

        var usersQueryable = dbContext.Collection<CmsUser>().AsQueryable();

        var queryable = blogPostQueryable
            .WhereIf(tagFilteredEntityIds.Any(), x => tagFilteredEntityIds.Contains(x.Id))
            .WhereIf(favoriteUserFilteredEntityIds.Any(), x => favoriteUserFilteredEntityIds.Contains(x.Id))
            .WhereIf(blogId.HasValue, x => x.BlogId == blogId)
            .WhereIf(!string.IsNullOrWhiteSpace(filter), x => x.Title.Contains(filter) || x.Slug.Contains(filter))
            .WhereIf(authorId.HasValue, x => x.AuthorId == authorId)
            .WhereIf(statusFilter.HasValue, x => x.Status == statusFilter);

        queryable = queryable.OrderBy(sorting.IsNullOrEmpty() ? $"{nameof(BlogPost.CreationTime)} desc" : sorting);

        var combinedQueryable = queryable
                                .Join(
                                    usersQueryable,
                                    o => o.AuthorId,
                                    i => i.Id,
                                    (blogPost, user) => new { blogPost, user })
                                .Skip(skipCount)
                                .Take(maxResultCount);

        var combinedResult = await AsyncExecuter.ToListAsync(combinedQueryable, cancellationToken);

        return combinedResult.Select(s =>
                                    {
                                        s.blogPost.Author = s.user;
                                        return s.blogPost;
                                    }).ToList();
    }

    protected virtual async Task<List<Guid>> GetEntityIdsByTagId(Guid? tagId, CancellationToken cancellationToken)
    {
        var entityIdFilters = new List<Guid>();
        if (!tagId.HasValue)
        {
            return entityIdFilters;
        }

        var entityIds =
            await _entityTagManager.GetEntityIdsFilteredByTagAsync(tagId.Value, CurrentTenant.Id, cancellationToken);

        if (entityIds.Count == 0)
        {
            entityIdFilters.Add(Guid.Empty);
            return entityIdFilters;
        }
        
        foreach (var entityId in entityIds)
        {
            if (Guid.TryParse(entityId, out var parsedEntityId))
            {
                entityIdFilters.Add(parsedEntityId);
            }
        }

        return entityIdFilters;
    }

    protected virtual async Task<List<Guid>> GetFavoriteEntityIdsByUserId(Guid? userId, CancellationToken cancellationToken)
    {
        var entityIdFilters = new List<Guid>();
        if (!userId.HasValue)
        {
            return entityIdFilters;
        }

        var entityIds = await _markedItemManager.GetEntityIdsFilteredByUserAsync(
            userId.Value, 
            BlogPostConsts.EntityType, 
            CurrentTenant.Id, 
            cancellationToken
        );

        if (entityIds.Count == 0)
        {
            entityIdFilters.Add(Guid.Empty);
            return entityIdFilters;
        }
        
        foreach (var entityId in entityIds)
        {
            if (Guid.TryParse(entityId, out var parsedEntityId))
            {
                entityIdFilters.Add(parsedEntityId);
            }
        }

        return entityIdFilters;
    }

    public virtual async Task<bool> SlugExistsAsync(Guid blogId, [NotNull] string slug,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrEmpty(slug, nameof(slug));

        cancellationToken = GetCancellationToken(cancellationToken);
        var queryable = await GetQueryableAsync(cancellationToken);
        return await queryable.AnyAsync(x => x.BlogId == blogId && x.Slug.ToLower() == slug, cancellationToken);
    }

    public virtual async Task<List<CmsUser>> GetAuthorsHasBlogPostsAsync(int skipCount, int maxResultCount, string sorting, string filter, CancellationToken cancellationToken = default)
    {
        var queryable = (await CreateAuthorsQueryableAsync(cancellationToken))
                        .Skip(skipCount)
                        .Take(maxResultCount)
                        .OrderBy(sorting.IsNullOrEmpty() ? nameof(CmsUser.UserName) : sorting)
                        .WhereIf(!filter.IsNullOrEmpty(), x => x.UserName.Contains(filter.ToLower()));

        return await AsyncExecuter.ToListAsync(queryable, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<int> GetAuthorsHasBlogPostsCountAsync(string filter, CancellationToken cancellationToken = default)
    {
        return await AsyncExecuter.CountAsync(
            (await CreateAuthorsQueryableAsync(cancellationToken))
                .WhereIf(!filter.IsNullOrEmpty(), x => x.UserName.Contains(filter.ToLower())));
    }

    public virtual async Task<CmsUser> GetAuthorHasBlogPostAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await AsyncExecuter.FirstOrDefaultAsync(await CreateAuthorsQueryableAsync(cancellationToken), x => x.Id == id)
            ?? throw new EntityNotFoundException(typeof(CmsUser), id);
    }

    protected virtual async Task<IQueryable<CmsUser>> CreateAuthorsQueryableAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        var blogPostQueryable = (await GetQueryableAsync())
            .Where(x => x.Status == BlogPostStatus.Published);

        var usersQueryable = (await GetDbContextAsync(cancellationToken)).Collection<CmsUser>().AsQueryable();

        return blogPostQueryable
                        .Join(
                            usersQueryable,
                            o => o.AuthorId,
                            i => i.Id,
                            (blogPost, user) => new { blogPost, user })
                        .Select(s => s.user)
                        .Distinct();
    }

    public virtual async Task<bool> HasBlogPostWaitingForReviewAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        return await (await GetQueryableAsync(cancellationToken))
            .AnyAsync(x => x.Status == BlogPostStatus.WaitingForReview, cancellationToken);
    }

    public virtual async Task UpdateBlogAsync(Guid sourceBlogId, Guid? targetBlogId, CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        var blogPosts = await (await GetQueryableAsync(cancellationToken)).Where(x => x.BlogId == sourceBlogId).ToListAsync(cancellationToken);
        if (targetBlogId.HasValue)
        {
            foreach (var blogPost in blogPosts)
            {
                blogPost.SetBlogId(targetBlogId.Value);
            }

            await UpdateManyAsync(blogPosts, false, cancellationToken);
        }
        else
        {

            await DeleteManyAsync(blogPosts, false, cancellationToken);
        }
    }

    public virtual async Task DeleteByBlogIdAsync(Guid blogId, CancellationToken cancellationToken = default)
    {
        await DeleteAsync(x => x.BlogId == blogId, cancellationToken: cancellationToken);
    }
}
