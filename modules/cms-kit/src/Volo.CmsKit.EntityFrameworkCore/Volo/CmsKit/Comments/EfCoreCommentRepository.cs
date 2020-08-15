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

            var query = from comment in DbSet
                join user in DbContext.Set<CmsUser>() on comment.CreatorId equals user.Id
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
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var replies = await DbSet
                .Where(x => x.RepliedCommentId == id)
                .ToListAsync(GetCancellationToken(cancellationToken));

            foreach (var reply in replies)
            {
                //TODO: Discuss if it is better to mark it as deleted and show in the ui as "This is deleted" instead of deleting it and replies completely
                await base.DeleteAsync(
                    reply.Id,
                    cancellationToken: GetCancellationToken(cancellationToken)
                );
            }

            await base.DeleteAsync(id, cancellationToken: GetCancellationToken(cancellationToken));
        }
    }
}
