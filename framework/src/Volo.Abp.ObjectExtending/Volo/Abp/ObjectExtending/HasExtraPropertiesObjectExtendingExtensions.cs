using System;
using System.Collections.Generic;
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
        /// <typeparam name="TSource">Source class type</typeparam>
        /// <typeparam name="TDestination">Destination class type</typeparam>
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
            MapExtraPropertiesTo<TSource, TDestination>(
                source.ExtraProperties,
                destination.ExtraProperties,
                definitionCheck
            );
        }

        /// <summary>
        /// Copies extra properties from the <paramref name="sourceDictionary"/> object
        /// to the <paramref name="destinationDictionary"/> object.
        ///
        /// Checks property definitions (over the <see cref="ObjectExtensionManager"/>)
        /// based on the <paramref name="definitionCheck"/> preference.
        /// </summary>
        /// <typeparam name="TSource">Source class type (for definition check)</typeparam>
        /// <typeparam name="TDestination">Destination class type (for definition check)</typeparam>
        /// <param name="sourceDictionary">The source dictionary object</param>
        /// <param name="destinationDictionary">The destination dictionary object</param>
        /// <param name="definitionCheck">
        /// Controls which properties to map.
        /// </param>
        public static void MapExtraPropertiesTo<TSource, TDestination>(
            Dictionary<string, object> sourceDictionary,
            Dictionary<string, object> destinationDictionary,
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
                foreach (var keyValue in sourceDictionary)
                {
                    destinationDictionary[keyValue.Key] = keyValue.Value;
                }
            }
            else if (definitionCheck == MappingPropertyDefinitionCheck.Source)
            {
                foreach (var property in sourceObjectExtension.GetProperties())
                {
                    if (!sourceDictionary.ContainsKey(property.Name))
                    {
                        continue;
                    }

                    destinationDictionary[property.Name] = sourceDictionary[property.Name];
                }
            }
            else if (definitionCheck == MappingPropertyDefinitionCheck.Destination)
            {
                foreach (var keyValue in sourceDictionary)
                {
                    if (!destinationObjectExtension.HasProperty(keyValue.Key))
                    {
                        continue;
                    }

                    destinationDictionary[keyValue.Key] = keyValue.Value;
                }
            }
            else if (definitionCheck == MappingPropertyDefinitionCheck.Both)
            {
                foreach (var property in sourceObjectExtension.GetProperties())
                {
                    if (!sourceDictionary.ContainsKey(property.Name))
                    {
                        continue;
                    }

                    if (!destinationObjectExtension.HasProperty(property.Name))
                    {
                        continue;
                    }

                    destinationDictionary[property.Name] = sourceDictionary[property.Name];
                }
            }
            else
            {
                throw new NotImplementedException(definitionCheck + " was not implemented!");
            }
        }
    }
}
