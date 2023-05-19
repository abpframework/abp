using System.Linq;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Auditing;

public static class EntityHistorySelectorListExtensions
{
    public const string AllEntitiesSelectorName = "Abp.Entities.All";

    public static void AddAllEntities(this IEntityHistorySelectorList selectors)
    {
        if (selectors.Any(s => s.Name == AllEntitiesSelectorName))
        {
            return;
        }

        selectors.Add(new NamedTypeSelector(AllEntitiesSelectorName, t => typeof(IEntity).IsAssignableFrom(t)));
    }
}
