using JetBrains.Annotations;

namespace Volo.Abp.Localization;

public class NonTypedLocalizationResource : LocalizationResourceBase
{
    public NonTypedLocalizationResource(
        [NotNull] string resourceName,
        [CanBeNull] string defaultCultureName = null,
        [CanBeNull] ILocalizationResourceContributor initialContributor = null
    ) : base(
        resourceName,
        defaultCultureName,
        initialContributor)
    {
    }
}