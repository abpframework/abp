using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.Localization
{
    public static class AbpLocalizationOptionsExtensions
    {
        public static AbpLocalizationOptions AddLanguagesMap(this AbpLocalizationOptions localizationOptions, string packageName, params NameValue[] maps)
        {
            if (localizationOptions.LanguagesMap.TryGetValue(packageName, out var existMaps))
            {
                existMaps.AddRange(maps);
            }
            else
            {
                localizationOptions.LanguagesMap.Add(packageName, maps.ToList());
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

        public static AbpLocalizationOptions AddLanguageFilesMap(this AbpLocalizationOptions localizationOptions, string packageName, params NameValue[] maps)
        {
            if (localizationOptions.LanguageFilesMap.TryGetValue(packageName, out var existMaps))
            {
                existMaps.AddRange(maps);
            }
            else
            {
                localizationOptions.LanguageFilesMap.Add(packageName, maps.ToList());
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
    }
}
