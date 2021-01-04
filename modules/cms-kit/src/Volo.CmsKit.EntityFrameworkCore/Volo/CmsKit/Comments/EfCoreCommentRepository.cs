using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Comments
{
    public class EfCoreCommentRepository : EfCoreRepository<ICmsKitDbContext, Comment, Guid>,
        ICommentRepository
    {
        public EfCoreCommentRepository(IDbContextProvider<ICmsKitDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<CommentWithAuthorQueryResultItem>> GetListWithAuthorsAsync(
            string entityType,
            string entityId,
            CancellationToken cancellationToken = default)
        {
            Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
            Check.NotNullOrWhiteSpace(entityId, nameof(entityId));

            var dbSet = await GetDbSetAsync();

            var dbContext = await GetDbContextAsync();

            var query = from comment in dbSet
                        join user in dbContext.Set<CmsUser>() on comment.CreatorId equals user.Id
                        where entityType == comment.EntityType && entityId == comment.EntityId
                        orderby comment.CreationTime
                        select new CommentWithAuthorQueryResultItem
                        {
                            Comment = comment,
                            Author = user
                        };

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task DeleteWithRepliesAsync(
            Comment comment,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();

            var replies = await dbSet
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
    }
}
