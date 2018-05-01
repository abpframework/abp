using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Localization.VirtualFiles.Json;

namespace Volo.Abp.Localization
{
    public static class LocalizationResourceListExtensions
    {
        public static LocalizationResource AddVirtualJson<TResource>(
            [NotNull] this LocalizationResourceDictionary resourceDictionary,
            [CanBeNull] string defaultCultureName, //TODO: defaultCultureName should be optional (and second argument) because it's not required for the LocalizationResource!
            [NotNull] string virtualPath)
        {
            Check.NotNull(resourceDictionary, nameof(resourceDictionary));
            Check.NotNull(virtualPath, nameof(virtualPath));

            virtualPath = virtualPath.EnsureStartsWith('/');

            var resourceType = typeof(TResource);

            if (resourceDictionary.ContainsKey(resourceType))
            {
                throw new AbpException("There is already a resource with given type: " + resourceType.AssemblyQualifiedName);
            }

            return resourceDictionary[resourceType] = new LocalizationResource(
                resourceType,
                defaultCultureName,
                new JsonEmbeddedFileLocalizationDictionaryProvider(
                    virtualPath
                )
            );
        }

        public static void ExtendWithVirtualJson<TResource>(
            [NotNull] this LocalizationResourceDictionary resourceDictionary,
            [NotNull] string virtualPath)
        {
            Check.NotNull(resourceDictionary, nameof(resourceDictionary));

            resourceDictionary.GetResource<TResource>().Extensions.Add(
                new JsonEmbeddedFileLocalizationDictionaryProvider(
                    virtualPath
                )
            );
        }

        public static void AddBaseTypes<TResource>(
            [NotNull] this LocalizationResourceDictionary resourceDictionary,
            [NotNull] params Type[] types)
        {
            Check.NotNull(resourceDictionary, nameof(resourceDictionary));
            Check.NotNull(types, nameof(types));

            var resource = resourceDictionary.GetResource<TResource>();

            foreach (var type in types)
            {
                resource.BaseResourceTypes.AddIfNotContains(type);
            }
        }

        public static LocalizationResource GetResource<TResource>(this LocalizationResourceDictionary resourceDictionary)
        {
            var resourceType = typeof(TResource);

            var resource = resourceDictionary.GetOrDefault(resourceType);
            if (resource == null)
            {
                throw new AbpException("Can not find a resource with given type: " + resourceType.AssemblyQualifiedName);
            }

            return resource;
        }
    }
}