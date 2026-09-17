using JetBrains.Annotations;

namespace Volo.Abp.Localization;

public static class LocalizationSettingHelper
{
    /// <summary>
    /// Gets a setting value like "en-US;en" and returns as splitted values like ("en-US", "en").
    /// </summary>
    /// <param name="settingValue"></param>
    /// <param name="defaultCultureName"></param>
    /// <returns></returns>
    public static (string cultureName, string uiCultureName) ParseLanguageSetting([NotNull] string settingValue, string defaultCultureName = "en")
    {
        Check.NotNull(settingValue, nameof(settingValue));

        if (!settingValue.Contains(";"))
        {
            return CultureHelper.IsValidCultureCode(settingValue)
                ? (settingValue, settingValue)
                : (defaultCultureName, defaultCultureName);
        }

        var splitted = settingValue.Split(';');
        if (splitted.Length == 2 && CultureHelper.IsValidCultureCode(splitted[0]) && CultureHelper.IsValidCultureCode(splitted[1]))
        {
            return (splitted[0], splitted[1]);
        }

        return (defaultCultureName, defaultCultureName);
    }
}
