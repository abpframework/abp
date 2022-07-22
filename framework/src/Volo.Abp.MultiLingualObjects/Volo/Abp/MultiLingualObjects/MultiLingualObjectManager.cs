using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace Volo.Abp.MultiLingualObjects;

public class MultiLingualObjectManager : IMultiLingualObjectManager, ITransientDependency
{
    protected ISettingProvider SettingProvider { get; }

    protected const int MaxCultureFallbackDepth = 5;

    public MultiLingualObjectManager(ISettingProvider settingProvider)
    {
        SettingProvider = settingProvider;
    }
    public virtual async Task<TTranslation> GetTranslationAsync<TTranslation>(
        ICollection<TTranslation> translations,
        string culture,
        bool fallbackToParentCultures)
        where TTranslation : class, IObjectTranslation

    {
        culture ??= CultureInfo.CurrentUICulture.Name;

        if (translations.IsNullOrEmpty())
        {
            return null;
        }

        var translation = translations.FirstOrDefault(pt => pt.Language == culture);
        if (translation != null)
        {
            return translation;
        }

        if (fallbackToParentCultures)
        {
            translation = GetTranslationBasedOnCulturalRecursive(
                CultureInfo.CurrentUICulture.Parent,
                translations,
                0
            );

            if (translation != null)
            {
                return translation;
            }
        }

        var defaultLanguage = await SettingProvider.GetOrNullAsync(LocalizationSettingNames.DefaultLanguage);

        translation = translations.FirstOrDefault(pt => pt.Language == defaultLanguage);
        if (translation != null)
        {
            return translation;
        }

        translation = translations.FirstOrDefault();
        return translation;
    }

    public virtual Task<TTranslation> GetTranslationAsync<TMultiLingual, TTranslation>(
        TMultiLingual multiLingual,
        string culture = null,
        bool fallbackToParentCultures = true)
        where TMultiLingual : IMultiLingualObject<TTranslation>
        where TTranslation : class, IObjectTranslation
    {
        return GetTranslationAsync(multiLingual.Translations, culture: culture, fallbackToParentCultures: fallbackToParentCultures);
    }

    protected virtual TTranslation GetTranslationBasedOnCulturalRecursive<TTranslation>(
        CultureInfo culture, ICollection<TTranslation> translations, int currentDepth)
        where TTranslation : class, IObjectTranslation
    {
        if (culture == null ||
            culture.Name.IsNullOrWhiteSpace() ||
            translations.IsNullOrEmpty() ||
            currentDepth > MaxCultureFallbackDepth)
        {
            return null;
        }

        var translation = translations.FirstOrDefault(pt => pt.Language.Equals(culture.Name, StringComparison.OrdinalIgnoreCase));
        return translation ?? GetTranslationBasedOnCulturalRecursive(culture.Parent, translations, currentDepth + 1);
    }
}
