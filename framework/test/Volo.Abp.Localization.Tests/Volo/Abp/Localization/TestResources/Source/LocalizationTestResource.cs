using Volo.Abp.Localization.TestResources.Base.CountryNames;
using Volo.Abp.Localization.TestResources.Base.Validation;

namespace Volo.Abp.Localization.TestResources.Source
{
    [InheritResource(typeof(LocalizationTestValidationResource))]
    [InheritResource(typeof(LocalizationTestCountryNamesResource))]
    public sealed class LocalizationTestResource
    {
        
    }
}
