using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.Reactions
{
    public interface IUserReactionRepository : IBasicRepository<UserReaction, Guid>
    {
        Task<UserReaction> FindAsync(
            Guid userId,
            [NotNull] string entityType,
            [NotNull] string entityId,
            [NotNull] string reactionName);
    }
}
