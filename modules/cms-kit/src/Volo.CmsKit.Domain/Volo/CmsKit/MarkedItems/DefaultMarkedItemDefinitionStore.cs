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

        var definition = Options.EntityTypes.SingleOrDefault(x => x.EntityType.Equals(entityType, StringComparison.InvariantCultureIgnoreCase));

        return Task.FromResult(definition);
    }
}