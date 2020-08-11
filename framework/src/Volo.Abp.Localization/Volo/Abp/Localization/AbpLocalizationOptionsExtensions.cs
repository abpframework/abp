using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Volo.Abp.Localization
{
    public static class AbpLocalizationOptionsExtensions
    {
        public static AbpLocalizationOptions AddLanguagesMapOrUpdate(this AbpLocalizationOptions localizationOptions,
            string packageName, params NameValue[] maps)
        {
            foreach (var map in maps)
            {
                AddOrUpdate(localizationOptions.LanguagesMap, packageName, map);
            }

            return localizationOptions;
        }

        public static string GetLanguagesMap(this AbpLocalizationOptions localizationOptions, string packageName,
            string language)
        {
            return localizationOptions.LanguagesMap.TryGetValue(packageName, out var maps)
                ? maps.FirstOrDefault(x => x.Name == language)?.Value ?? language
                : language;
        }

        public static string GetCurrentUICultureLanguagesMap(this AbpLocalizationOptions localizationOptions, string packageName)
        {
            return GetLanguagesMap(localizationOptions, packageName, CultureInfo.CurrentUICulture.Name);
        }

        public static AbpLocalizationOptions AddLanguageFilesMapOrUpdate(this AbpLocalizationOptions localizationOptions,
            string packageName, params NameValue[] maps)
        {
            foreach (var map in maps)
            {
                AddOrUpdate(localizationOptions.LanguageFilesMap, packageName, map);
            }

            return localizationOptions;
        }

        public static string GetLanguageFilesMap(this AbpLocalizationOptions localizationOptions, string packageName,
            string language)
        {
            return localizationOptions.LanguageFilesMap.TryGetValue(packageName, out var maps)
                ? maps.FirstOrDefault(x => x.Name == language)?.Value ?? language
                : language;
        }

        public static string GetCurrentUICultureLanguageFilesMap(this AbpLocalizationOptions localizationOptions, string packageName)
        {
            return GetLanguageFilesMap(localizationOptions, packageName, CultureInfo.CurrentUICulture.Name);
        }

        private static void AddOrUpdate(IDictionary<string, List<NameValue>> maps, string packageName, NameValue value)
        {
            if (maps.TryGetValue(packageName, out var existMaps))
            {
                existMaps.GetOrAdd(x => x.Name == value.Name, () => value).Value = value.Value;
            }
            else
            {
                maps.Add(packageName, new List<NameValue> {value});
            }
        }
    }
}
