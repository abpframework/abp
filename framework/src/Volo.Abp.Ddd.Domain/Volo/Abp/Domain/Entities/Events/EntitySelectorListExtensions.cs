using System;

namespace Volo.Abp.Domain.Entities.Events;

public static class EntitySelectorListExtensions
{
    public static IEntitySelectorList Add(this IEntitySelectorList selectors, string name, Func<Type, bool> predicate)
    {
        selectors.Add(new NamedTypeSelector(name, predicate));
        return selectors;
    }
}
