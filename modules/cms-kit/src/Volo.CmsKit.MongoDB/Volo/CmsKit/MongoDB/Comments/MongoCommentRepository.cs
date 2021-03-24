using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Comments;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.MongoDB.Comments
{
    public class MongoCommentRepository : MongoDbRepository<ICmsKitMongoDbContext, Comment, Guid>, ICommentRepository
    {
        public MongoCommentRepository(IMongoDbContextProvider<ICmsKitMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<CommentWithAuthorQueryResultItem> GetWithAuthorAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var query = from comment in (await GetMongoQueryableAsync(cancellationToken))
                join user in (await GetDbContextAsync(cancellationToken)).CmsUsers on comment.CreatorId equals user.Id
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

        public async Task<List<CommentWithAuthorQueryResultItem>> GetListAsync(
            string filter = null, 
            string entityType = null, 
            string entityId = null, 
            Guid? repliedCommentId = null,
            string authorUsername = null,
            DateTime? creationStartDate = null, 
            DateTime? creationEndDate = null, 
            string sorting = null,
            int maxResultCount = int.MaxValue, 
            int skipCount = 0, 
            CancellationToken cancellationToken = default
        )
        {
            var token = GetCancellationToken(cancellationToken);
            var query = await GetListQueryAsync(
                filter, 
                entityType, 
                entityId, 
                repliedCommentId, 
                authorUsername, 
                creationStartDate, 
                creationEndDate, 
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

            var authors = await authorsQuery.ToListAsync(token);
            
            return comments
                .Select(
                    comment =>
                        new CommentWithAuthorQueryResultItem
                        {
                            Comment = comment,
                            Author = authors.FirstOrDefault(a => a.Id == comment.CreatorId)
                        }).ToList();
        }

        public async Task<long> GetCountAsync(
            string text = null, 
            string entityType = null, 
            string entityId = null,
            Guid? repliedCommentId = null, 
            string authorUsername = null,
            DateTime? creationStartDate = null,
            DateTime? creationEndDate = null, 
            CancellationToken cancellationToken = default
        )
        {
            var query = await GetListQueryAsync(
                text, 
                entityType, 
                entityId, 
                repliedCommentId, 
                authorUsername, 
                creationStartDate, 
                creationEndDate, 
                cancellationToken);

            return await query.As<IMongoQueryable<Comment>>()
                .LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<CommentWithAuthorQueryResultItem>> GetListWithAuthorsAsync(
            string entityType,
            string entityId,
            CancellationToken cancellationToken = default)
        {
            Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
            Check.NotNullOrWhiteSpace(entityId, nameof(entityId));

            var authorsQuery = from comment in (await GetMongoQueryableAsync(cancellationToken))
                join user in (await GetDbContextAsync(cancellationToken)).CmsUsers on comment.CreatorId equals user.Id
                where entityType == comment.EntityType && entityId == comment.EntityId
                orderby comment.CreationTime
                select user;

            var authors = await authorsQuery.ToListAsync(GetCancellationToken(cancellationToken));

            var comments = await (await GetMongoQueryableAsync(cancellationToken))
                .Where(c => c.EntityId == entityId && c.EntityType == entityType)
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

        public async Task DeleteWithRepliesAsync(
            Comment comment,
            CancellationToken cancellationToken = default)
        {
            var replies = await (await GetMongoQueryableAsync(cancellationToken))
                .Where(x => x.RepliedCommentId == comment.Id)
                .ToListAsync(GetCancellationToken(cancellationToken));

            foreach (var reply in replies)
            {
                await base.DeleteAsync(
                    reply,
                    cancellationToken: GetCancellationToken(cancellationToken)
                );
            }

            await base.DeleteAsync(
                comment,
                cancellationToken: GetCancellationToken(cancellationToken)
            );
        }
        
        protected virtual async Task<IQueryable<Comment>> GetListQueryAsync(
            string filter = null, 
            string entityType = null, 
            string entityId = null,
            Guid? repliedCommentId = null, 
            string authorUsername = null,
            DateTime? creationStartDate = null,
            DateTime? creationEndDate = null,
            CancellationToken cancellationToken = default
        )
        {
            var queryable = await GetMongoQueryableAsync(cancellationToken);
            
            if (!string.IsNullOrEmpty(authorUsername))
            {
                var authorQueryable = (await GetDbContextAsync(cancellationToken)).Collection<CmsUser>().AsQueryable();
                
                var author = await authorQueryable.FirstOrDefaultAsync(x => x.UserName == authorUsername, cancellationToken: cancellationToken);

                var authorId = author?.Id ?? Guid.Empty;

                queryable = queryable.Where(x => x.CreatorId == authorId);
            }
            
            return queryable.WhereIf(!filter.IsNullOrWhiteSpace(), c => c.Text.Contains(filter))
                .WhereIf(!entityType.IsNullOrWhiteSpace(), c => c.EntityType == entityType)
                .WhereIf(!entityId.IsNullOrWhiteSpace(), c => c.EntityId == entityId)
                .WhereIf(repliedCommentId.HasValue, c => c.RepliedCommentId == repliedCommentId)
                .WhereIf(creationStartDate.HasValue, c => c.CreationTime >= creationStartDate)
                .WhereIf(creationEndDate.HasValue, c => c.CreationTime <= creationEndDate);
        }
    }
}
