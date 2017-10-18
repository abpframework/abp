using JetBrains.Annotations;
using Volo.Abp.Localization.Json;

namespace Volo.Abp.Localization
{
    public static class LocalizationResourceListExtensions
    {
        public static void AddJson<TResource>(this LocalizationResourceList resourceList, [NotNull] string defaultCultureName)
        {
            var type = typeof(TResource);

            resourceList.Add(
                new LocalizationResource(
                    type,
                    defaultCultureName,
                    new JsonEmbeddedFileLocalizationDictionaryProvider(
                        type.Assembly,
                        type.Namespace
                    )
                )
            );
        }
    }
}