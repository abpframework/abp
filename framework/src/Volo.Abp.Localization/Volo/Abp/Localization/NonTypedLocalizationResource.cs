using JetBrains.Annotations;

namespace Volo.Abp.Localization;

public class NonTypedLocalizationResource : LocalizationResourceBase
{
    public NonTypedLocalizationResource(
        [NotNull] string resourceName,
        string? defaultCultureName = null,
        ILocalizationResourceContributor? initialContributor = null
    ) : base(
        resourceName,
        defaultCultureName,
        initialContributor)
    {
    }
}