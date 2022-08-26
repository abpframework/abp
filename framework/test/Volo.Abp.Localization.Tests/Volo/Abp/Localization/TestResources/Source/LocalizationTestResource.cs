using Volo.Abp.Localization.TestResources.Base.CountryNames;
using Volo.Abp.Localization.TestResources.Base.Validation;

namespace Volo.Abp.Localization.TestResources.Source;

[InheritResource(
    typeof(LocalizationTestValidationResource),
    typeof(LocalizationTestCountryNamesResource)
    )]
[LocalizationResourceName("Test")]
public sealed class LocalizationTestResource
{

}
