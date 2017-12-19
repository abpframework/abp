using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Localization.Json;
using Volo.Abp.Localization.VirtualFiles.Json;

namespace Volo.Abp.Localization
{
    public static class LocalizationResourceListExtensions
    {
        public static LocalizationResource AddVirtualJson<TResource>(
            [NotNull] this LocalizationResourceDictionary resourceDictionary,
            [NotNull] string defaultCultureName,
            [NotNull] string virtualPath)
        {
            Check.NotNull(resourceDictionary, nameof(resourceDictionary));
            Check.NotNull(defaultCultureName, nameof(defaultCultureName));
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

            var resourceType = typeof(TResource);

            var resource = resourceDictionary.GetOrDefault(resourceType);
            if (resource == null)
            {
                throw new AbpException("Can not find a resource with given type: " + resourceType.AssemblyQualifiedName);
            }

            resource.Extensions.Add(new JsonEmbeddedFileLocalizationDictionaryProvider(
                virtualPath
            ));
        }
    }
}