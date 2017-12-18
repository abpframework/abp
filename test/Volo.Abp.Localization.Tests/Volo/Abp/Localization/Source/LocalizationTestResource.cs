using Volo.Abp.Localization.Base.CountryNames;
using Volo.Abp.Localization.Base.Validation;

namespace Volo.Abp.Localization.Source
{
    [InheritResource(typeof(LocalizationTestValidationResource))]
    [InheritResource(typeof(LocalizationTestCountryNamesResource))]
    public sealed class LocalizationTestResource
    {
        
    }
}
