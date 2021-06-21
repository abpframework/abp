using System;
using System.Collections.Generic;
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

        public static void Add(
            [NotNull] this IAutoEntityDistributedEventSelectorList selectors,
            string selectorName, 
            Func<Type, bool> predicate)
        {
            Check.NotNull(selectors, nameof(selectors));
            
            if (selectors.Any(s => s.Name == selectorName))
            {
                throw new AbpException($"There is already a selector added before with the same name: {selectorName}");
            }

            selectors.Add(
                new NamedTypeSelector(
                    selectorName,
                    predicate
                )
            );
        }
        
        public static void Add(
            [NotNull] this IAutoEntityDistributedEventSelectorList selectors,
            Func<Type, bool> predicate)
        {
            selectors.Add(Guid.NewGuid().ToString("N"), predicate);
        }

        public static bool RemoveByName(
            [NotNull] this IAutoEntityDistributedEventSelectorList selectors,
            [NotNull] string name)
        {
            Check.NotNull(selectors, nameof(selectors));
            Check.NotNull(name, nameof(name));
            
            return selectors.RemoveAll(s => s.Name == name).Count > 0;
        }

        
        public static bool IsMatch([NotNull] this IAutoEntityDistributedEventSelectorList selectors, Type entityType)
        {
            Check.NotNull(selectors, nameof(selectors));
            return selectors.Any(s => s.Predicate(entityType));
        }
    }
}