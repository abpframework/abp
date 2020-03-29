using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
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
        /// based on the <paramref name="definitionChecks"/> preference.
        /// </summary>
        /// <typeparam name="TSource">Source class type</typeparam>
        /// <typeparam name="TDestination">Destination class type</typeparam>
        /// <param name="source">The source object</param>
        /// <param name="destination">The destination object</param>
        /// <param name="definitionChecks">
        /// Controls which properties to map.
        /// </param>
        public static void MapExtraPropertiesTo<TSource, TDestination>(
            [NotNull] this TSource source,
            [NotNull] TDestination destination,
            MappingPropertyDefinitionChecks definitionChecks = MappingPropertyDefinitionChecks.Both)
            where TSource : IHasExtraProperties
            where TDestination : IHasExtraProperties
        {
            Check.NotNull(source, nameof(source));
            Check.NotNull(destination, nameof(destination));

            MapExtraPropertiesTo(
                typeof(TSource),
                typeof(TDestination),
                source.ExtraProperties,
                destination.ExtraProperties,
                definitionChecks
            );
        }

        /// <summary>
        /// Copies extra properties from the <paramref name="sourceDictionary"/> object
        /// to the <paramref name="destinationDictionary"/> object.
        ///
        /// Checks property definitions (over the <see cref="ObjectExtensionManager"/>)
        /// based on the <paramref name="definitionChecks"/> preference.
        /// </summary>
        /// <typeparam name="TSource">Source class type (for definition check)</typeparam>
        /// <typeparam name="TDestination">Destination class type (for definition check)</typeparam>
        /// <param name="sourceDictionary">The source dictionary object</param>
        /// <param name="destinationDictionary">The destination dictionary object</param>
        /// <param name="definitionChecks">
        /// Controls which properties to map.
        /// </param>
        public static void MapExtraPropertiesTo<TSource, TDestination>(
            [NotNull] Dictionary<string, object> sourceDictionary,
            [NotNull] Dictionary<string, object> destinationDictionary,
            MappingPropertyDefinitionChecks definitionChecks = MappingPropertyDefinitionChecks.Both)
            where TSource : IHasExtraProperties
            where TDestination : IHasExtraProperties
        {
            MapExtraPropertiesTo(
                typeof(TSource),
                typeof(TDestination),
                sourceDictionary,
                destinationDictionary,
                definitionChecks
            );
        }

        /// <summary>
        /// Copies extra properties from the <paramref name="sourceDictionary"/> object
        /// to the <paramref name="destinationDictionary"/> object.
        /// 
        /// Checks property definitions (over the <see cref="ObjectExtensionManager"/>)
        /// based on the <paramref name="definitionChecks"/> preference.
        /// </summary>
        /// <param name="sourceType">Source type (for definition check)</param>
        /// <param name="destinationType">Destination class type (for definition check)</param>
        /// <param name="sourceDictionary">The source dictionary object</param>
        /// <param name="destinationDictionary">The destination dictionary object</param>
        /// <param name="definitionChecks">
        /// Controls which properties to map.
        /// </param>
        public static void MapExtraPropertiesTo(
            [NotNull] Type sourceType,
            [NotNull] Type destinationType,
            [NotNull] Dictionary<string, object> sourceDictionary,
            [NotNull] Dictionary<string, object> destinationDictionary,
            MappingPropertyDefinitionChecks definitionChecks = MappingPropertyDefinitionChecks.Both)
        {
            Check.AssignableTo<IHasExtraProperties>(sourceType, nameof(sourceType));
            Check.AssignableTo<IHasExtraProperties>(destinationType, nameof(destinationType));
            Check.NotNull(sourceDictionary, nameof(sourceDictionary));
            Check.NotNull(destinationDictionary, nameof(destinationDictionary));

            var sourceObjectExtension = ObjectExtensionManager.Instance.GetOrNull(sourceType);
            if (definitionChecks.HasFlag(MappingPropertyDefinitionChecks.Source) &&
                sourceObjectExtension == null)
            {
                return;
            }

            var destinationObjectExtension = ObjectExtensionManager.Instance.GetOrNull(destinationType);
            if (definitionChecks.HasFlag(MappingPropertyDefinitionChecks.Destination) &&
                destinationObjectExtension == null)
            {
                return;
            }

            if (definitionChecks == MappingPropertyDefinitionChecks.None)
            {
                foreach (var keyValue in sourceDictionary)
                {
                    destinationDictionary[keyValue.Key] = keyValue.Value;
                }
            }
            else if (definitionChecks == MappingPropertyDefinitionChecks.Source)
            {
                Debug.Assert(sourceObjectExtension != null, nameof(sourceObjectExtension) + " != null");

                foreach (var property in sourceObjectExtension.GetProperties())
                {
                    if (!sourceDictionary.ContainsKey(property.Name))
                    {
                        continue;
                    }

                    destinationDictionary[property.Name] = sourceDictionary[property.Name];
                }
            }
            else if (definitionChecks == MappingPropertyDefinitionChecks.Destination)
            {
                Debug.Assert(destinationObjectExtension != null, nameof(destinationObjectExtension) + " != null");

                foreach (var keyValue in sourceDictionary)
                {
                    if (!destinationObjectExtension.HasProperty(keyValue.Key))
                    {
                        continue;
                    }

                    destinationDictionary[keyValue.Key] = keyValue.Value;
                }
            }
            else if (definitionChecks == MappingPropertyDefinitionChecks.Both)
            {
                Debug.Assert(sourceObjectExtension != null, nameof(sourceObjectExtension) + " != null");
                Debug.Assert(destinationObjectExtension != null, nameof(destinationObjectExtension) + " != null");

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
                throw new NotImplementedException(definitionChecks + " was not implemented!");
            }
        }
    }
}
