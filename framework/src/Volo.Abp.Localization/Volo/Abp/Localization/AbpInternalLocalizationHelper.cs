using Microsoft.Extensions.Localization;

namespace Volo.Abp.Localization;

/// <summary>
/// This class is designed to be used internal by the framework.
/// </summary>
public static class AbpInternalLocalizationHelper
{
    /// <summary>
    /// Searches an array of keys in an array of localizers.
    /// </summary>
    /// <param name="localizers">
    /// An array of localizers. Search the keys on the localizers.
    /// Can contain null items in the array.
    /// </param>
    /// <param name="keys">
    /// An array of keys. Search the keys on the localizers.
    /// Should not contain null items in the array.
    /// </param>
    /// <param name="defaultValue">
    /// Return value if none of the localizers has none of the keys.
    /// </param>
    /// <returns></returns>
    public static string LocalizeWithFallback(
        IStringLocalizer[] localizers,
        string[] keys,
        string defaultValue)
    {
        foreach (var key in keys)
        {
            foreach (var localizer in localizers)
            {
                if (localizer == null)
                {
                    continue;
                }

                var localizedString = localizer[key];
                if (!localizedString.ResourceNotFound)
                {
                    return localizedString.Value;
                }
            }
        }

        return defaultValue;
    }
}
