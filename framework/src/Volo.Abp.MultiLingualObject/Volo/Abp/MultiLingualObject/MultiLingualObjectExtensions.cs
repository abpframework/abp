using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Volo.Abp.Localization;
using Volo.Abp.Settings;
using Volo.Abp.Threading;

namespace Volo.Abp.MultiLingualObject
{
    public static class MultiLingualObjectExtensions
    {
        public static TTranslation GetMultiLingualTranslation<TTranslation>(
            this IHasMultiLingual<TTranslation> hasMultiLingual,
            ISettingProvider settingProvider,
            bool fallbackToParentCultures = false)
            where TTranslation : class, IMultiLingualTranslation
        {
            if (hasMultiLingual.Translations == null || !hasMultiLingual.Translations.Any())
            {
                return null;
            }

            var translation =
                hasMultiLingual.Translations.FirstOrDefault(pt => pt.Language == CultureInfo.CurrentUICulture.Name);
            if (translation != null)
            {
                return translation;
            }

            if (fallbackToParentCultures)
            {
                translation =
                    GeTranslationBasedOnCulturalRecursive(
                        CultureInfo.CurrentUICulture.Parent, hasMultiLingual.Translations, 0);
                if (translation != null)
                {
                    return translation;
                }
            }

            var defaultLanguage = AsyncHelper.RunSync(() =>
                settingProvider.GetOrNullAsync(LocalizationSettingNames.DefaultLanguage));

            translation = hasMultiLingual.Translations.FirstOrDefault(pt => pt.Language == defaultLanguage);
            if (translation != null)
            {
                return translation;
            }

            translation = hasMultiLingual.Translations.FirstOrDefault();
            return translation;
        }

        private const int MaxCultureFallbackDepth = 5;

        private static TTranslation GeTranslationBasedOnCulturalRecursive<TTranslation>(
            CultureInfo culture, ICollection<TTranslation> translations, int currentDepth)
            where TTranslation : class, IMultiLingualTranslation
        {
            if (culture == null || culture.Name.IsNullOrWhiteSpace() || translations.IsNullOrEmpty() ||
                currentDepth > MaxCultureFallbackDepth)
            {
                return null;
            }

            var translation = translations.FirstOrDefault(pt =>
                pt.Language.Equals(culture.Name, StringComparison.OrdinalIgnoreCase));
            return translation ??
                   GeTranslationBasedOnCulturalRecursive(culture.Parent,
                       translations, currentDepth + 1);
        }
    }
}
