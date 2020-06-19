using System;
using System.Linq;
using JetBrains.Annotations;

namespace Volo.Abp.Domain.Entities.Events.Distributed
{
    public static class AutoEntityDistributedEventSelectorListExtensions
    {
        public const string AllEntitiesSelectorName = "All";

        public static void AddNamespace([NotNull] this IAutoEntityDistributedEventSelectorList selectors, [NotNull] string namespaceName)
        {
            Check.NotNull(selectors, nameof(selectors));

            var selectorName = "Namespace:" + namespaceName;
            if (selectors.Any(s => s.Name == selectorName))
            {
                return;
            }

            selectors.Add(
                new NamedTypeSelector(
                    selectorName,
                    t => t.FullName?.StartsWith(namespaceName) ?? false
                )
            );
        }
        
        /// <summary>
        /// Adds a specific entity type and the types derived from that entity type.
        /// </summary>
        /// <typeparam name="TEntity">Type of the entity</typeparam>
        public static void Add<TEntity>([NotNull] this IAutoEntityDistributedEventSelectorList selectors)
            where TEntity : IEntity
        {
            Check.NotNull(selectors, nameof(selectors));

            var selectorName = "Entity:" + typeof(TEntity).FullName;
            if (selectors.Any(s => s.Name == selectorName))
            {
                return;
            }

            selectors.Add(
                new NamedTypeSelector(
                    selectorName,
                    t => typeof(TEntity).IsAssignableFrom(t)
                )
            );
        }
        
        /// <summary>
        /// Adds all entity types.
        /// </summary>
        public static void AddAll([NotNull] this IAutoEntityDistributedEventSelectorList selectors)
        {
            Check.NotNull(selectors, nameof(selectors));
            
            if (selectors.Any(s => s.Name == AllEntitiesSelectorName))
            {
                return;
            }

            selectors.Add(
                new NamedTypeSelector(
                    AllEntitiesSelectorName,
                    t => typeof(IEntity).IsAssignableFrom(t)
                )
            );
        }

        public static bool IsMatch([NotNull] this IAutoEntityDistributedEventSelectorList selectors, Type entityType)
        {
            Check.NotNull(selectors, nameof(selectors));
            return selectors.Any(s => s.Predicate(entityType));
        }
    }
}