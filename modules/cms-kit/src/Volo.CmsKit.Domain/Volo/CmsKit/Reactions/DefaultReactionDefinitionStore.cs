using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Volo.CmsKit.Reactions;

public class DefaultReactionDefinitionStore : IReactionDefinitionStore
{
    protected CmsKitReactionOptions Options { get; }

    public DefaultReactionDefinitionStore(IOptions<CmsKitReactionOptions> options)
    {
        Options = options.Value;
    }

    public virtual async Task<List<ReactionDefinition>> GetReactionsAsync([NotNull] string entityType)
    {
        Check.NotNullOrEmpty(entityType, nameof(entityType));

        var definition = await GetAsync(entityType);

        return definition.Reactions;
    }

    public virtual async Task<ReactionDefinition> GetReactionOrNullAsync([NotNull] string reactionName, [NotNull] string entityType)
    {
        Check.NotNullOrEmpty(entityType, nameof(entityType));
        Check.NotNullOrEmpty(reactionName, nameof(reactionName));

        var definition = await GetAsync(entityType);

        return definition.Reactions.SingleOrDefault(x => x.Name == reactionName);
    }

    public virtual Task<bool> IsDefinedAsync([NotNull] string entityType)
    {
        Check.NotNullOrWhiteSpace(entityType, nameof(entityType));

        var isDefined = Options.EntityTypes.Any(x => x.EntityType.Equals(entityType, StringComparison.InvariantCultureIgnoreCase));

        return Task.FromResult(isDefined);
    }

    /// <summary>
    /// Gets single <see cref="ReactionEntityTypeDefinition"/> by entityType.
    /// </summary>
    /// <param name="entityType">EntityType to get definition.</param>
    /// <exception cref="EntityCantHaveReactionException">Thrown when EntityType is not configured as taggable.</exception>
    /// <exception cref="InvalidOperationException">More than one element satisfies the condition in predicate.</exception>
    public virtual Task<ReactionEntityTypeDefinition> GetAsync([NotNull] string entityType)
    {
        Check.NotNullOrWhiteSpace(entityType, nameof(entityType));

        var definition = Options.EntityTypes.SingleOrDefault(x => x.EntityType.Equals(entityType, StringComparison.InvariantCultureIgnoreCase)) ??
                     throw new EntityCantHaveReactionException(entityType);

        return Task.FromResult(definition);
    }
}
