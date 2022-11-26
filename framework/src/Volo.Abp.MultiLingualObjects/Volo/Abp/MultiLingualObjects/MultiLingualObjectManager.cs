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
    public virtual async Task<TTranslation?> GetTranslationAsync<TTranslation>(
        IEnumerable<TTranslation> translations,
        string? culture,
        bool fallbackToParentCultures)
        where TTranslation : class, IObjectTranslation

    {
        culture ??= CultureInfo.CurrentUICulture.Name;

        if (translations == null || !translations.Any())
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

    public virtual Task<TTranslation?> GetTranslationAsync<TMultiLingual, TTranslation>(
        TMultiLingual multiLingual,
        string? culture = null,
        bool fallbackToParentCultures = true)
        where TMultiLingual : IMultiLingualObject<TTranslation>
        where TTranslation : class, IObjectTranslation
    {
        return GetTranslationAsync(multiLingual.Translations, culture: culture, fallbackToParentCultures: fallbackToParentCultures);
    }

    protected virtual TTranslation? GetTranslationBasedOnCulturalRecursive<TTranslation>(
        CultureInfo culture, IEnumerable<TTranslation> translations, int currentDepth)
        where TTranslation : class, IObjectTranslation
    {
        if (culture == null ||
            culture.Name.IsNullOrWhiteSpace() ||
            translations == null || !translations.Any() ||
            currentDepth > MaxCultureFallbackDepth)
        {
            return null;
        }

        var translation = translations.FirstOrDefault(pt => pt.Language.Equals(culture.Name, StringComparison.OrdinalIgnoreCase));
        return translation ?? GetTranslationBasedOnCulturalRecursive(culture.Parent, translations, currentDepth + 1);
    }

    public virtual async Task<List<TTranslation?>> GetBulkTranslationsAsync<TTranslation>(IEnumerable<IEnumerable<TTranslation>> translationsCombined, string? culture, bool fallbackToParentCultures)
       where TTranslation : class, IObjectTranslation
    {
        culture ??= CultureInfo.CurrentUICulture.Name;

        if (translationsCombined == null || !translationsCombined.Any())
        {
            return new();
        }

        var someHaveNoTranslations = false;
        var res = new List<TTranslation?>();
        foreach (var translations in translationsCombined)
        {
            if (!translations.Any())
            {
                //if the src has no translations, don't try to find a translation
                res.Add(null);
                continue;
            }
            var translation = translations.FirstOrDefault(pt => pt.Language == culture);
            if (translation != null)
            {
                res.Add(translation);
            }
            else
            {
                if (fallbackToParentCultures)
                {
                    translation = GetTranslationBasedOnCulturalRecursive(
                        CultureInfo.CurrentUICulture.Parent,
                        translations,
                        0
                    );

                    if (translation != null)
                    {
                        res.Add(translation);
                    }
                    else
                    {
                        res.Add(null);
                        someHaveNoTranslations = true;
                    }
                }
                else
                {
                    res.Add(null);
                    someHaveNoTranslations = true;
                }
            }
        }


        if (someHaveNoTranslations)
        {
            var defaultLanguage = await SettingProvider.GetOrNullAsync(LocalizationSettingNames.DefaultLanguage);

            var index = 0;
            foreach (var translations in translationsCombined)
            {
                if (!translations.Any())
                {
                    //don't try to find a translation
                }
                else
                {
                    var translation = res[index];
                    if (translation != null)
                    {
                        continue;
                    }
                    translation = translations.FirstOrDefault(pt => pt.Language == defaultLanguage);
                    if (translation != null)
                    {
                        res[index] = translation;
                    }
                    else
                    {
                        res[index] = translations.FirstOrDefault();
                    }
                }
                index++;
            }
        }
        return res;
    }

    public virtual async Task<List<(TMultiLingual entity, TTranslation? translation)>> GetBulkTranslationsAsync<TMultiLingual, TTranslation>(IEnumerable<TMultiLingual> multiLinguals, string? culture, bool fallbackToParentCultures)
       where TMultiLingual : IMultiLingualObject<TTranslation>
       where TTranslation : class, IObjectTranslation
    {
        var resInitial = await GetBulkTranslationsAsync(multiLinguals.Select(x => x.Translations), culture, fallbackToParentCultures);
        var index = 0;
        var res = new List<(TMultiLingual entity, TTranslation? translation)>();
        foreach (var item in multiLinguals)
        {
            var t = resInitial[index++];
            res.Add((item, t));
        }
        return res;
    }
}
