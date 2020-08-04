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

namespace Volo.CmsKit.Comments
{
    public class EfCoreCommentRepository : EfCoreRepository<ICmsKitDbContext, Comment, Guid>,
        ICommentRepository
    {
        public EfCoreCommentRepository(IDbContextProvider<ICmsKitDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<CommentWithAuthor>> GetListWithAuthorsAsync(
            string entityType,
            string entityId)
        {
            Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
            Check.NotNullOrWhiteSpace(entityId, nameof(entityId));

            var query = from comment in DbSet
                join user in DbContext.CmsUsers on comment.CreatorId equals user.Id
                where entityType == comment.EntityType && entityId == comment.EntityId
                orderby comment.CreationTime
                select new CommentWithAuthor
                {
                    Comment = comment,
                    Author = user
                };

            return await query.ToListAsync();
        }

        public override async Task DeleteAsync(Guid id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var replies = await DbSet
                .Where(x => x.RepliedCommentId == id)
                .ToListAsync(GetCancellationToken(cancellationToken));

            foreach (var reply in replies)
            {
                //TODO: Discuss if it is better to mark it as deleted and show in the ui as "This is deleted" instead of deleting it and replies completely
                await base.DeleteAsync(reply.Id, autoSave, GetCancellationToken(cancellationToken));
            }

            await base.DeleteAsync(id, autoSave, GetCancellationToken(cancellationToken));
        }
    }
}
