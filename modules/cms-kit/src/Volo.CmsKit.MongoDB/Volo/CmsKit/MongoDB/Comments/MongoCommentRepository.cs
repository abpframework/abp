using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Comments;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.MongoDB.Comments;

public class MongoCommentRepository : MongoDbRepository<ICmsKitMongoDbContext, Comment, Guid>, ICommentRepository
{
    public MongoCommentRepository(IMongoDbContextProvider<ICmsKitMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<CommentWithAuthorQueryResultItem> GetWithAuthorAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var comment = await GetAsync(id);
        var author = await (await GetDbContextAsync())
            .CmsUsers
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == comment.CreatorId, GetCancellationToken(cancellationToken));

        return new CommentWithAuthorQueryResultItem()
        {
            Comment = comment,
            Author = author
        };
    }

    public virtual async Task<List<CommentWithAuthorQueryResultItem>> GetListAsync(
        string filter = null,
        string entityType = null,
        Guid? repliedCommentId = null,
        string authorUsername = null,
        DateTime? creationStartDate = null,
        DateTime? creationEndDate = null,
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CommentApproveState commentApproveState = CommentApproveState.All,
        CancellationToken cancellationToken = default
    )
    {
        var token = GetCancellationToken(cancellationToken);
        var query = await GetListQueryAsync(
            filter,
            entityType,
            repliedCommentId,
            authorUsername,
            creationStartDate,
            creationEndDate,
            commentApproveState,
            token);

        var comments = await query.OrderBy(sorting.IsNullOrEmpty() ? "creationTime desc" : sorting)
            .As<IMongoQueryable<Comment>>()
            .PageBy<Comment, IMongoQueryable<Comment>>(skipCount, maxResultCount)
            .ToListAsync(token);

        var commentIds = comments.Select(x => x.Id).ToList();

        var authorsQuery = from comment in (await GetMongoQueryableAsync(token))
            join user in (await GetDbContextAsync(token)).CmsUsers on comment.CreatorId equals user.Id
            where commentIds.Contains(comment.Id)
            orderby comment.CreationTime
            select user;

        var authors = await ApplyDataFilters<IMongoQueryable<CmsUser>, CmsUser>(authorsQuery).ToListAsync(token);

        return comments
            .Select(
                comment =>
                    new CommentWithAuthorQueryResultItem {
                        Comment = comment, Author = authors.FirstOrDefault(a => a.Id == comment.CreatorId)
                    }).ToList();
    }

    public virtual async Task<long> GetCountAsync(
        string text = null,
        string entityType = null,
        Guid? repliedCommentId = null,
        string authorUsername = null,
        DateTime? creationStartDate = null,
        DateTime? creationEndDate = null,
        CommentApproveState commentApproveState = CommentApproveState.All,
        CancellationToken cancellationToken = default
    )
    {
        var query = await GetListQueryAsync(
            text,
            entityType,
            repliedCommentId,
            authorUsername,
            creationStartDate,
            creationEndDate,
            commentApproveState,
            cancellationToken);

        return await query.As<IMongoQueryable<Comment>>()
            .LongCountAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<CommentWithAuthorQueryResultItem>> GetListWithAuthorsAsync(
        string entityType,
        string entityId,
        CommentApproveState commentApproveState = CommentApproveState.All,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
        Check.NotNullOrWhiteSpace(entityId, nameof(entityId));

        var authorsQuery = from comment in (await GetMongoQueryableAsync(cancellationToken))
                           join user in (await GetDbContextAsync(cancellationToken)).CmsUsers on comment.CreatorId equals user.Id
                           where entityType == comment.EntityType && entityId == comment.EntityId
                           orderby comment.CreationTime
                           select user;

        var authors = await ApplyDataFilters<IMongoQueryable<CmsUser>, CmsUser>(authorsQuery).ToListAsync(GetCancellationToken(cancellationToken));

        var commentsQuery = (await GetMongoQueryableAsync(cancellationToken))
        .Where(c => c.EntityId == entityId && c.EntityType == entityType);

        commentsQuery = commentApproveState switch {
            CommentApproveState.Approved => commentsQuery.Where(c => c.IsApproved == true),
            CommentApproveState.Approved | CommentApproveState.Waiting => commentsQuery.Where(c => c.IsApproved == true || c.IsApproved == null),
            _ => commentsQuery
        };

        var comments = await commentsQuery
            .OrderBy(c => c.CreationTime)
            .ToListAsync(GetCancellationToken(cancellationToken));

        return comments
            .Select(
                comment =>
                    new CommentWithAuthorQueryResultItem
                    {
                        Comment = comment,
                        Author = authors.FirstOrDefault(a => a.Id == comment.CreatorId)
                    }).ToList();
    }

    public virtual async Task DeleteWithRepliesAsync(
        Comment comment,
        CancellationToken cancellationToken = default)
    {
        var replies = await (await GetMongoQueryableAsync(cancellationToken))
            .Where(x => x.RepliedCommentId == comment.Id)
            .ToListAsync(GetCancellationToken(cancellationToken));

        await base.DeleteAsync(
            comment,
            cancellationToken: GetCancellationToken(cancellationToken)
        );

        foreach (var reply in replies)
        {
            await base.DeleteAsync(
                reply,
                cancellationToken: GetCancellationToken(cancellationToken)
            );
        }
    }

    public virtual async Task<bool> ExistsAsync(string idempotencyToken, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .AnyAsync(x => x.IdempotencyToken == idempotencyToken, GetCancellationToken(cancellationToken));
    }

    protected virtual async Task<IQueryable<Comment>> GetListQueryAsync(
        string filter = null,
        string entityType = null,
        Guid? repliedCommentId = null,
        string authorUsername = null,
        DateTime? creationStartDate = null,
        DateTime? creationEndDate = null,
        CommentApproveState commentApproveState = CommentApproveState.All,
        CancellationToken cancellationToken = default
    )
    {
        var queryable = await GetMongoQueryableAsync(cancellationToken);

        if (!string.IsNullOrEmpty(authorUsername))
        {
            var author = await (await GetMongoQueryableAsync<CmsUser>(cancellationToken)).FirstOrDefaultAsync(x => x.UserName == authorUsername, cancellationToken: cancellationToken);

            var authorId = author?.Id ?? Guid.Empty;

            queryable = queryable.Where(x => x.CreatorId == authorId);
        }

        return queryable.WhereIf(!filter.IsNullOrWhiteSpace(), c => c.Text.Contains(filter))
            .WhereIf(!entityType.IsNullOrWhiteSpace(), c => c.EntityType == entityType)
            .WhereIf(repliedCommentId.HasValue, c => c.RepliedCommentId == repliedCommentId)
            .WhereIf(creationStartDate.HasValue, c => c.CreationTime >= creationStartDate)
            .WhereIf(creationEndDate.HasValue, c => c.CreationTime <= creationEndDate)
            .WhereIf(CommentApproveState.Approved == commentApproveState, c => c.IsApproved == true)
            .WhereIf(CommentApproveState.Disapproved == commentApproveState, c => c.IsApproved == false)
            .WhereIf(CommentApproveState.Waiting == commentApproveState, c => c.IsApproved == null);
    }
}
