using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Comments;

public class EfCoreCommentRepository : EfCoreRepository<ICmsKitDbContext, Comment, Guid>,
    ICommentRepository
{
    public EfCoreCommentRepository(IDbContextProvider<ICmsKitDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public virtual async Task<CommentWithAuthorQueryResultItem> GetWithAuthorAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = from comment in (await GetDbSetAsync())
                    join user in (await GetDbContextAsync()).Set<CmsUser>() on comment.CreatorId equals user.Id
                    where id == comment.Id
                    select new CommentWithAuthorQueryResultItem
                    {
                        Comment = comment,
                        Author = user
                    };

        var commentWithAuthor = await query.FirstOrDefaultAsync(GetCancellationToken(cancellationToken));

        if (commentWithAuthor == null)
        {
            throw new EntityNotFoundException(typeof(Comment), id);
        }

        return commentWithAuthor;
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
            commentApproveState);

        if (!sorting.IsNullOrEmpty())
        {
            sorting = "comment." + sorting;
        }

        query = query.OrderBy(sorting.IsNullOrEmpty() ? "comment.creationTime desc" : sorting)
            .PageBy(skipCount, maxResultCount);

        return await query.ToListAsync(token);
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
        var token = GetCancellationToken(cancellationToken);
        var query = await GetListQueryAsync(
            text,
            entityType,
            repliedCommentId,
            authorUsername,
            creationStartDate,
            creationEndDate,
            commentApproveState);

        return await query.LongCountAsync(token);
    }

    public virtual async Task<List<CommentWithAuthorQueryResultItem>> GetListWithAuthorsAsync(
        string entityType,
        string entityId,
        CommentApproveState commentApproveState = CommentApproveState.All,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
        Check.NotNullOrWhiteSpace(entityId, nameof(entityId));

        var query = from comment in (await GetDbSetAsync())
                    join user in (await GetDbContextAsync()).Set<CmsUser>() on comment.CreatorId equals user.Id
                    where entityType == comment.EntityType && entityId == comment.EntityId
                    orderby comment.CreationTime
                    select new CommentWithAuthorQueryResultItem
                    {
                        Comment = comment,
                        Author = user
                    };

        query = query.WhereIf(commentApproveState == CommentApproveState.Approved, c => c.Comment.IsApproved == true);
        query = query.WhereIf(commentApproveState == (CommentApproveState.Approved | CommentApproveState.Waiting), c => c.Comment.IsApproved == true || c.Comment.IsApproved == null);

        return await query.ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task DeleteWithRepliesAsync(
        Comment comment,
        CancellationToken cancellationToken = default)
    {
        var replies = await (await GetDbSetAsync())
            .Where(x => x.RepliedCommentId == comment.Id)
            .ToListAsync(GetCancellationToken(cancellationToken));

        foreach (var reply in replies)
        {
            await DeleteAsync(
                reply,
                cancellationToken: GetCancellationToken(cancellationToken)
            );
        }

        await DeleteAsync(comment, cancellationToken: GetCancellationToken(cancellationToken));
    }

    public virtual async Task<bool> ExistsAsync(
        string idempotencyToken,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).AnyAsync(x => x.IdempotencyToken == idempotencyToken, GetCancellationToken(cancellationToken));
    }

    protected virtual async Task<IQueryable<CommentWithAuthorQueryResultItem>> GetListQueryAsync(
        string filter = null,
        string entityType = null,
        Guid? repliedCommentId = null,
        string authorUsername = null,
        DateTime? creationStartDate = null,
        DateTime? creationEndDate = null,
        CommentApproveState commentApproveState = CommentApproveState.All)
    {
        var commentDbSet = await GetDbSetAsync();
        var cmsUserSet = (await GetDbContextAsync()).Set<CmsUser>();

        var query = from comment in commentDbSet
                    join user in cmsUserSet
                        on comment.CreatorId equals user.Id
                    select new CommentWithAuthorQueryResultItem
                    {
                        Comment = comment,
                        Author = user
                    };

        return query.WhereIf(!filter.IsNullOrWhiteSpace(), c => c.Comment.Text.Contains(filter))
            .WhereIf(!entityType.IsNullOrWhiteSpace(), c => c.Comment.EntityType.Contains(entityType))
            .WhereIf(repliedCommentId.HasValue, c => c.Comment.RepliedCommentId == repliedCommentId)
            .WhereIf(!authorUsername.IsNullOrWhiteSpace(), c => c.Author.UserName.Contains(authorUsername))
            .WhereIf(creationStartDate.HasValue, c => c.Comment.CreationTime >= creationStartDate)
            .WhereIf(creationEndDate.HasValue, c => c.Comment.CreationTime <= creationEndDate)
            .WhereIf(CommentApproveState.Approved == commentApproveState, c => c.Comment.IsApproved == true)
            .WhereIf(CommentApproveState.Disapproved == commentApproveState, c => c.Comment.IsApproved == false)
            .WhereIf(CommentApproveState.Waiting == commentApproveState, c => c.Comment.IsApproved == null);
    }
    
    public async Task DeleteByEntityTypeAndIdAsync(string entityType, string entityId, CancellationToken cancellationToken = default)
    {
         await (await GetDbSetAsync()).Where(x => x.EntityType == entityType && x.EntityId == entityId).ExecuteDeleteAsync(GetCancellationToken(cancellationToken));
    }
}
