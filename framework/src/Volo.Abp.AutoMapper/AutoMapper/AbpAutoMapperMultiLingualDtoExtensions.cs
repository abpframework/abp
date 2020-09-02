using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Localization;
using Volo.Abp.Settings;
using Volo.Abp.Threading;

namespace AutoMapper
{
    public static class AbpAutoMapperMultiLingualDtoExtensions
    {
        public static CreateMultiLingualMapResult<TMultiLingualEntity, TTranslation, TDestination> CreateMultLingualMap<
            TMultiLingualEntity,
            TTranslation, TDestination>(
            this Profile profile,
            ISettingProvider serviceProvider,
            bool fallbackToParentCultures = false)
            where TMultiLingualEntity : class, IMultiLingualEntity<TTranslation>
            where TTranslation : class, IEntityTranslation
        {
            return new CreateMultiLingualMapResult<TMultiLingualEntity, TTranslation, TDestination>
            {
                TranslationMap = profile.CreateMap<TTranslation, TDestination>(),
                EntityMap = profile.CreateMap<TMultiLingualEntity, TDestination>().BeforeMap(
                    (source, destination, context) =>
                    {
                        if (source.Translations == null || !source.Translations.Any())
                        {
                            return;
                        }

                        var translation =
                            source.Translations.FirstOrDefault(pt => pt.Language == CultureInfo.CurrentUICulture.Name);
                        if (translation != null)
                        {
                            context.Mapper.Map(translation, destination);
                            return;
                        }

                        if (fallbackToParentCultures)
                        {
                            translation =
                                GeTranslationBasedOnCulturalRecursive<TMultiLingualEntity, TTranslation>(
                                    CultureInfo.CurrentUICulture.Parent, source.Translations, 0);
                            if (translation != null)
                            {
                                context.Mapper.Map(translation, destination);
                                return;
                            }
                        }

                        var defaultLanguage = AsyncHelper.RunSync(() =>
                            serviceProvider.GetOrNullAsync(LocalizationSettingNames.DefaultLanguage));

                        translation = source.Translations.FirstOrDefault(pt => pt.Language == defaultLanguage);
                        if (translation != null)
                        {
                            context.Mapper.Map(translation, destination);
                            return;
                        }

                        translation = source.Translations.FirstOrDefault();
                        if (translation != null)
                        {
                            context.Mapper.Map(translation, destination);
                        }
                    })
            };
        }

        private const int MaxCultureFallbackDepth = 5;

        private static TTranslation GeTranslationBasedOnCulturalRecursive<TMultiLingualEntity, TTranslation>(
            CultureInfo culture, ICollection<TTranslation> translations, int currentDepth)
            where TTranslation : class, IEntityTranslation
        {
            if (culture == null || culture.Name.IsNullOrWhiteSpace() || translations.IsNullOrEmpty() ||
                currentDepth > MaxCultureFallbackDepth)
            {
                return null;
            }

            var translation = translations.FirstOrDefault(pt =>
                pt.Language.Equals(culture.Name, StringComparison.OrdinalIgnoreCase));
            return translation ??
                   GeTranslationBasedOnCulturalRecursive<TMultiLingualEntity, TTranslation>(culture.Parent,
                       translations, currentDepth + 1);
        }
    }

    public class CreateMultiLingualMapResult<TMultiLingualEntity, TTranslation, TDestination>
    {
        public IMappingExpression<TTranslation, TDestination> TranslationMap { get; set; }

        public IMappingExpression<TMultiLingualEntity, TDestination> EntityMap { get; set; }
    }
}
