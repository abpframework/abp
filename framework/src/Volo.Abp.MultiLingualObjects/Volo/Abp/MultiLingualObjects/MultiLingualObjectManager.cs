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
        IEnumerable<TTranslation>? translations,
        string? culture,
        bool fallbackToParentCultures)
        where TTranslation : class, IObjectTranslation

    {
        culture ??= CultureInfo.CurrentUICulture.Name;

        var translationList = translations?.ToList();
        if (translationList == null || translationList.Count == 0)
        {
            return null;
        }

        var translation = translationList.FirstOrDefault(pt => pt.Language == culture);
        if (translation != null)
        {
            return translation;
        }

        if (fallbackToParentCultures)
        {
            translation = GetTranslationBasedOnCulturalRecursive(
                CultureInfo.CurrentUICulture.Parent,
                translationList,
                0
            );

            if (translation != null)
            {
                return translation;
            }
        }

        var defaultLanguage = await SettingProvider.GetOrNullAsync(LocalizationSettingNames.DefaultLanguage);

        translation = translationList.FirstOrDefault(pt => pt.Language == defaultLanguage);
        if (translation != null)
        {
            return translation;
        }

        translation = translationList.FirstOrDefault();
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
        CultureInfo? culture, IEnumerable<TTranslation>? translations, int currentDepth)
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

    public virtual async Task<List<TTranslation?>> GetBulkTranslationsAsync<TTranslation>(IEnumerable<IEnumerable<TTranslation>>? translationsCombined, string? culture, bool fallbackToParentCultures)
       where TTranslation : class, IObjectTranslation
    {
        culture ??= CultureInfo.CurrentUICulture.Name;

        var translationsCombinedList = translationsCombined?.Select(translations => translations.ToList()).ToList();
        if (translationsCombinedList == null || translationsCombinedList.Count == 0)
        {
            return new();
        }

        var someHaveNoTranslations = false;
        var res = new List<TTranslation?>();
        foreach (var translations in translationsCombinedList)
        {
            if (translations.Count == 0)
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
            foreach (var translations in translationsCombinedList)
            {
                //if the src has no translations, don't try to find a translation
                if (translations.Count > 0 && res[index] == null)
                {
                    res[index] = translations.FirstOrDefault(pt => pt.Language == defaultLanguage) ??
                                 translations.FirstOrDefault();
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
        var multiLingualList = multiLinguals.ToList();
        var resInitial = await GetBulkTranslationsAsync(multiLingualList.Select(x => x.Translations), culture, fallbackToParentCultures);
        var index = 0;
        var res = new List<(TMultiLingual entity, TTranslation? translation)>();
        foreach (var item in multiLingualList)
        {
            var t = resInitial[index++];
            res.Add((item, t));
        }
        return res;
    }
}
