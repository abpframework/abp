using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;


namespace Volo.Abp.MultiLingualObjects
{
    public class MultiLingualObjectManager : IMultiLingualObjectManager, ITransientDependency
    {
        protected const int MaxCultureFallbackDepth = 5;

        public MultiLingualObjectManager()
        {
        }

        public Task<TTranslation> GetTranslationAsync<TMultiLingual, TTranslation>(TMultiLingual multiLingual,
            string culture = null,
            bool fallbackToParentCultures = true)
            where TMultiLingual : IMultiLingualObject where TTranslation : class, IObjectTranslation
        {
            return Task.FromResult(
                GetTranslation<TMultiLingual, TTranslation>(multiLingual, culture, fallbackToParentCultures));
        }

        public virtual TTranslation GetTranslation<TMultiLingual, TTranslation>(
            TMultiLingual multiLingual,
            string culture = null,
            bool fallbackToParentCultures = true)
            where TMultiLingual : IMultiLingualObject
            where TTranslation : class, IObjectTranslation
        {
            culture ??= CultureInfo.CurrentUICulture.Name;

            if (multiLingual.DefaultCulture == culture)
            {
                return null;
            }

            if (!multiLingual.Translations.Any())
            {
                return null;
            }

            if (multiLingual.Translations.TryGetValue(culture, out var translation))
            {
                return (TTranslation) translation;
            }

            if (fallbackToParentCultures)
            {
                var cultureInfo = new CultureInfo(culture);
                var currentDepth = 0;

                while (true)
                {
                    if (cultureInfo.Name.IsNullOrWhiteSpace() || cultureInfo.Name == multiLingual.DefaultCulture ||
                        currentDepth > MaxCultureFallbackDepth)
                    {
                        return null;
                    }

                    if (multiLingual.Translations.TryGetValue(cultureInfo.Name, out translation))
                    {
                        return (TTranslation) translation;
                    }

                    cultureInfo = cultureInfo.Parent;
                    currentDepth += 1;
                }
            }

            if (multiLingual.Translations.TryGetValue(multiLingual.DefaultCulture, out translation))
            {
                return (TTranslation) translation;
            }

            translation = multiLingual.Translations.FirstOrDefault().Value;
            return (TTranslation) translation;
        }
    }
}