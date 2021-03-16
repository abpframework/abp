using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.CmsKit.Reactions
{
    public interface IReactionDefinitionStore : IEntityTypeDefinitionStore<ReactionEntityTypeDefinition>
    {
        Task<List<ReactionDefinition>> GetReactionsAsync([NotNull] string entityType);

        Task<ReactionDefinition> GetReactionOrNullAsync([NotNull] string reactionName, [NotNull] string entityType);
    }
}
