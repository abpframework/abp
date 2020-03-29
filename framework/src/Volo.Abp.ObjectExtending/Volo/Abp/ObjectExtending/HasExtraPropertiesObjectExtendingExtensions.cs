using System;
using Volo.Abp.Data;

namespace Volo.Abp.ObjectExtending
{
    public static class HasExtraPropertiesObjectExtendingExtensions
    {
        /// <summary>
        /// Copies extra properties from the <paramref name="source"/> object
        /// to the <paramref name="destination"/> object.
        ///
        /// Checks property definitions (over the <see cref="ObjectExtensionManager"/>)
        /// based on the <paramref name="definitionCheck"/> preference.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source">The source object</param>
        /// <param name="destination">The destination object</param>
        /// <param name="definitionCheck">
        /// Controls which properties to map.
        /// </param>
        public static void MapExtraPropertiesTo<TSource, TDestination>(
            this TSource source,
            TDestination destination,
            MappingPropertyDefinitionCheck definitionCheck = MappingPropertyDefinitionCheck.Both)
            where TSource : IHasExtraProperties
            where TDestination : IHasExtraProperties
        {
            var sourceObjectExtension = ObjectExtensionManager.Instance.GetOrNull<TSource>();
            if (definitionCheck.HasFlag(MappingPropertyDefinitionCheck.Source) &&
                sourceObjectExtension == null)
            {
                return;
            }

            var destinationObjectExtension = ObjectExtensionManager.Instance.GetOrNull<TDestination>();
            if (definitionCheck.HasFlag(MappingPropertyDefinitionCheck.Destination) &&
                destinationObjectExtension == null)
            {
                return;
            }

            if (definitionCheck == MappingPropertyDefinitionCheck.None)
            {
                foreach (var keyValue in source.ExtraProperties)
                {
                    destination.ExtraProperties[keyValue.Key] = keyValue.Value;
                }
            }
            else if (definitionCheck == MappingPropertyDefinitionCheck.Source)
            {
                foreach (var property in sourceObjectExtension.GetProperties())
                {
                    if (!source.ExtraProperties.ContainsKey(property.Name))
                    {
                        continue;
                    }

                    destination.ExtraProperties[property.Name] = source.ExtraProperties[property.Name];
                }
            }
            else if (definitionCheck == MappingPropertyDefinitionCheck.Destination)
            {
                foreach (var keyValue in source.ExtraProperties)
                {
                    if (!destinationObjectExtension.HasProperty(keyValue.Key))
                    {
                        continue;
                    }

                    destination.ExtraProperties[keyValue.Key] = keyValue.Value;
                }
            }
            else if(definitionCheck == MappingPropertyDefinitionCheck.Both)
            {
                foreach (var property in sourceObjectExtension.GetProperties())
                {
                    if (!source.ExtraProperties.ContainsKey(property.Name))
                    {
                        continue;
                    }

                    if (!destinationObjectExtension.HasProperty(property.Name))
                    {
                        continue;
                    }

                    destination.ExtraProperties[property.Name] = source.ExtraProperties[property.Name];
                }
            }
            else
            {
                throw new NotImplementedException(definitionCheck + " was not implemented!");
            }
        }
    }
}
