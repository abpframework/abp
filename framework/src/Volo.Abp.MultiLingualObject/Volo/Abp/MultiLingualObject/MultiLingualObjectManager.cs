using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Settings;
using Volo.Abp.Threading;

namespace Volo.Abp.MultiLingualObject
{
    public class MultiLingualObjectManager : IMultiLingualObjectManager, ITransientDependency
    {
        protected ISettingProvider SettingProvider { get; }

        protected const int MaxCultureFallbackDepth = 5;

        public MultiLingualObjectManager(ISettingProvider settingProvider)
        {
            SettingProvider = settingProvider;
        }

        public TTranslation GetTranslation<TMultiLingual, TTranslation>(
            TMultiLingual multiLingual,
            bool fallbackToParentCultures = true,
            string culture = null)
            where TMultiLingual : IHasMultiLingual<TTranslation>
            where TTranslation : class, IMultiLingualTranslation
        {
            return AsyncHelper.RunSync(() =>
                GetTranslationAsync<TMultiLingual, TTranslation>(multiLingual, fallbackToParentCultures, culture));
        }

        public virtual async Task<TTranslation> GetTranslationAsync<TMultiLingual, TTranslation>(
            TMultiLingual multiLingual,
            bool fallbackToParentCultures = true,
            string culture = null)
            where TMultiLingual : IHasMultiLingual<TTranslation>
            where TTranslation : class, IMultiLingualTranslation
        {
            culture ??= CultureInfo.CurrentUICulture.Name;

            if (multiLingual.Translations == null || !multiLingual.Translations.Any())
            {
                return null;
            }

            var translation = multiLingual.Translations.FirstOrDefault(pt => pt.Language == culture);
            if (translation != null)
            {
                return translation;
            }

            if (fallbackToParentCultures)
            {
                translation =
                    GeTranslationBasedOnCulturalRecursive(
                        CultureInfo.CurrentUICulture.Parent, multiLingual.Translations, 0);
                if (translation != null)
                {
                    return translation;
                }
            }

            var defaultLanguage = await SettingProvider.GetOrNullAsync(LocalizationSettingNames.DefaultLanguage);

            translation = multiLingual.Translations.FirstOrDefault(pt => pt.Language == defaultLanguage);
            if (translation != null)
            {
                return translation;
            }

            translation = multiLingual.Translations.FirstOrDefault();
            return translation;
        }

        protected virtual TTranslation GeTranslationBasedOnCulturalRecursive<TTranslation>(
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
