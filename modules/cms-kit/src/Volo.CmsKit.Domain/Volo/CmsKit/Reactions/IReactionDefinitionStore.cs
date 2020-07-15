using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.CmsKit.Reactions
{
    public interface IReactionDefinitionStore
    {
        Task<List<ReactionDefinition>> GetAvailableReactionsAsync([CanBeNull] string entityType);

        Task<ReactionDefinition> GetReactionOrNullAsync([NotNull] string reactionName, [CanBeNull] string entityType);
    }
}
