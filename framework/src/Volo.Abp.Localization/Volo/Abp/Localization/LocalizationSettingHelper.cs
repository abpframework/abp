using JetBrains.Annotations;

namespace Volo.Abp.Localization;

public static class LocalizationSettingHelper
{
    /// <summary>
    /// Gets a setting value like "en-US;en" and returns as splitted values like ("en-US", "en").
    /// </summary>
    /// <param name="settingValue"></param>
    /// <returns></returns>
    public static (string cultureName, string uiCultureName) ParseLanguageSetting([NotNull] string settingValue)
    {
        Check.NotNull(settingValue, nameof(settingValue));

        if (!settingValue.Contains(";"))
        {
            return (settingValue, settingValue);
        }

        var splitted = settingValue.Split(';');
        return (splitted[0], splitted[1]);
    }
}
