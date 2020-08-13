using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Reactions;

namespace Volo.CmsKit.MongoDB.Reactions
{
    public class MongoUserReactionRepository : MongoDbRepository<ICmsKitMongoDbContext, UserReaction, Guid>, IUserReactionRepository
    {
        public MongoUserReactionRepository(IMongoDbContextProvider<ICmsKitMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<UserReaction> FindAsync(
            Guid userId,
            string entityType,
            string entityId,
            string reactionName)
        {
            Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
            Check.NotNullOrWhiteSpace(entityId, nameof(entityId));
            Check.NotNullOrWhiteSpace(reactionName, nameof(reactionName));

            return await GetMongoQueryable()
                .Where(x =>
                    x.CreatorId == userId &&
                    x.EntityType == entityType &&
                    x.EntityId == entityId &&
                    x.ReactionName == reactionName)
                .FirstOrDefaultAsync();
        }

        public async Task<List<UserReaction>> GetListForUserAsync(
            Guid userId,
            string entityType,
            string entityId)
        {
            Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
            Check.NotNullOrWhiteSpace(entityId, nameof(entityId));

            return await GetMongoQueryable()
                .Where(x =>
                    x.CreatorId == userId &&
                    x.EntityType == entityType &&
                    x.EntityId == entityId)
                .ToListAsync();
        }

        public async Task<List<ReactionSummaryQueryResultItem>> GetSummariesAsync(
            string entityType,
            string entityId)
        {
            Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
            Check.NotNullOrWhiteSpace(entityId, nameof(entityId));

            return await GetMongoQueryable()
                .Where(x =>
                    x.EntityType == entityType &&
                    x.EntityId == entityId)
                .GroupBy(x => x.ReactionName)
                .Select(g => new ReactionSummaryQueryResultItem
                {
                    ReactionName = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();
        }
    }
}
