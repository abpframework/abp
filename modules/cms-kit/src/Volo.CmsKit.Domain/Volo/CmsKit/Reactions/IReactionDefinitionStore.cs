using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.CmsKit.Reactions
{
    public interface IReactionDefinitionStore
    {
        Task<bool> IsDefinedAsync([NotNull]string entityType);

        Task<ReactionEntityTypeDefinition> GetAsync([NotNull] string entityType);

        Task<List<ReactionDefinition>> GetReactionsAsync([NotNull] string entityType);

        Task<ReactionDefinition> GetReactionOrNullAsync([NotNull] string reactionName, [NotNull] string entityType);
    }
}
