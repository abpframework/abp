using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.Reactions
{
    public class EfCoreUserReactionRepository : EfCoreRepository<ICmsKitDbContext, UserReaction, Guid>, IUserReactionRepository
    {
        public EfCoreUserReactionRepository(IDbContextProvider<ICmsKitDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public Task<UserReaction> FindAsync(Guid userId, string entityType, string entityId, string reactionName)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserReaction>> GetListForUserAsync(Guid userId, string entityType, string entityId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ReactionSummaryQueryResultItem>> GetSummariesAsync(string inputEntityType, string inputEntityId)
        {
            throw new NotImplementedException();
        }
    }
}
