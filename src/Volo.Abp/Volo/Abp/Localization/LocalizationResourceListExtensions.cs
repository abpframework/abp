using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Localization.Json;

namespace Volo.Abp.Localization
{
    public static class LocalizationResourceListExtensions
    {
        public static LocalizationResource AddJson<TResource>([NotNull] this LocalizationResourceDictionary resourceDictionary, [NotNull] string defaultCultureName)
        {
            Check.NotNull(resourceDictionary, nameof(resourceDictionary));
            Check.NotNull(defaultCultureName, nameof(defaultCultureName));

            var resourceType = typeof(TResource);

            if (resourceDictionary.ContainsKey(resourceType))
            {
                throw new AbpException("There is already a resource with given type: " + resourceType.AssemblyQualifiedName);
            }

            return resourceDictionary[resourceType] = new LocalizationResource(
                resourceType,
                defaultCultureName,
                new JsonEmbeddedFileLocalizationDictionaryProvider(
                    resourceType.Assembly,
                    resourceType.Namespace
                )
            );
        }

        public static void ExtendWithJson<TResource, TResourceExt>([NotNull] this LocalizationResourceDictionary resourceDictionary)
        {
            Check.NotNull(resourceDictionary, nameof(resourceDictionary));

            var resourceType = typeof(TResource);
            var resourceExtType = typeof(TResourceExt);

            var resource = resourceDictionary.GetOrDefault(resourceType);
            if (resource == null)
            {
                throw new AbpException("Can not find a resource with given type: " + resourceType.AssemblyQualifiedName);
            }

            resource.Extensions.Add(new JsonEmbeddedFileLocalizationDictionaryProvider(
                resourceExtType.Assembly,
                resourceExtType.Namespace
            ));
        }
    }
}