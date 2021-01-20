using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Comments;

namespace Volo.CmsKit.MongoDB.Comments
{
    public class MongoCommentRepository : MongoDbRepository<ICmsKitMongoDbContext, Comment, Guid>, ICommentRepository
    {
        public MongoCommentRepository(IMongoDbContextProvider<ICmsKitMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
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
    }
}
