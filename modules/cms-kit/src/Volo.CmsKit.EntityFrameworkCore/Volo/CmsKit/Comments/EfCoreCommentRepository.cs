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

        public async Task<List<Comment>> GetListAsync(
            string entityType,
            string entityId)
        {
            Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
            Check.NotNullOrWhiteSpace(entityId, nameof(entityId));

            return await DbSet
                .Where(x =>
                    x.EntityType == entityType &&
                    x.EntityId == entityId)
                .ToListAsync();
        }

        public override async Task DeleteAsync(Guid id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var replies = await DbSet
                .Where(x => x.RepliedCommentId == id)
                .ToListAsync(GetCancellationToken(cancellationToken));

            foreach (var reply in replies)
            {
                await base.DeleteAsync(reply.Id, autoSave, GetCancellationToken(cancellationToken));
            }

            await base.DeleteAsync(id, autoSave, GetCancellationToken(cancellationToken));
        }
    }
}
