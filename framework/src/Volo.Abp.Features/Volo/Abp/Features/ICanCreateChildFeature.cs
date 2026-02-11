using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.Features;

public interface ICanCreateChildFeature
{
    FeatureDefinition CreateChildFeature(
        string name,
        string? defaultValue = null,
        ILocalizableString? displayName = null,
        ILocalizableString? description = null,
        IStringValueType? valueType = null,
        bool isVisibleToClients = true,
        bool isAvailableToHost = true);
}
