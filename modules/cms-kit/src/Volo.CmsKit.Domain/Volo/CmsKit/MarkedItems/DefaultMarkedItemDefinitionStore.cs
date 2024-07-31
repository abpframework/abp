using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Volo.Abp;

namespace Volo.CmsKit.MarkedItems;

public class DefaultMarkedItemDefinitionStore : IMarkedItemDefinitionStore
{
    protected CmsKitMarkedItemOptions Options { get; }

    public DefaultMarkedItemDefinitionStore(IOptions<CmsKitMarkedItemOptions> options)
    {
        Options = options.Value;
    }

    public virtual Task<bool> IsDefinedAsync([NotNull] string entityType)
    {
        Check.NotNullOrWhiteSpace(entityType, nameof(entityType));

        var isDefined = Options.EntityTypes.Any(x => x.EntityType.Equals(entityType, StringComparison.InvariantCultureIgnoreCase));

        return Task.FromResult(isDefined);
    }

    public virtual Task<MarkedItemEntityTypeDefinition> GetAsync([NotNull] string entityType)
    {
        Check.NotNullOrWhiteSpace(entityType, nameof(entityType));

        var definitions = Options.EntityTypes
                .Where(x => x.EntityType.Equals(entityType, StringComparison.InvariantCultureIgnoreCase))
                .ToList();

        if (definitions.Count == 0)
        {
            throw new MarkedItemDefinitionNotFoundException(entityType);
        }

        if (definitions.Count > 1)
        {
            throw new DuplicateMarkedItemDefinitionException(entityType);
        }

        return Task.FromResult(definitions.Single());
    }
}