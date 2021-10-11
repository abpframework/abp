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

namespace Volo.CmsKit.Reactions
{
    public class EfCoreUserReactionRepository : EfCoreRepository<ICmsKitDbContext, UserReaction, Guid>,
        IUserReactionRepository
    {
        public EfCoreUserReactionRepository(IDbContextProvider<ICmsKitDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<UserReaction> FindAsync(
            Guid userId,
            string entityType,
            string entityId,
            string reactionName,
            CancellationToken cancellationToken = default)
        {
            Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
            Check.NotNullOrWhiteSpace(entityId, nameof(entityId));
            Check.NotNullOrWhiteSpace(reactionName, nameof(reactionName));

            return await (await GetDbSetAsync())
                .Where(x =>
                    x.CreatorId == userId &&
                    x.EntityType == entityType &&
                    x.EntityId == entityId &&
                    x.ReactionName == reactionName)
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<UserReaction>> GetListForUserAsync(
            Guid userId,
            string entityType,
            string entityId,
            CancellationToken cancellationToken = default)
        {
            Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
            Check.NotNullOrWhiteSpace(entityId, nameof(entityId));

            return await (await GetDbSetAsync())
                .Where(x =>
                    x.CreatorId == userId &&
                    x.EntityType == entityType &&
                    x.EntityId == entityId)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<ReactionSummaryQueryResultItem>> GetSummariesAsync(
            string entityType,
            string entityId,
            CancellationToken cancellationToken = default)
        {
            Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
            Check.NotNullOrWhiteSpace(entityId, nameof(entityId));

            return await (await GetDbSetAsync())
                .Where(x =>
                    x.EntityType == entityType &&
                    x.EntityId == entityId)
                .GroupBy(x => x.ReactionName)
                .Select(g => new ReactionSummaryQueryResultItem
                {
                    ReactionName = g.Key,
                    Count = g.Count()
                })
                .ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}
