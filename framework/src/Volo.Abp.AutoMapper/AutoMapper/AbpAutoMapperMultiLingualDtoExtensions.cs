using Volo.Abp.MultiLingualObject;
using Volo.Abp.Settings;

namespace AutoMapper
{
    public static class AbpAutoMapperMultiLingualDtoExtensions
    {
        public static CreateMultiLingualMapResult<TSource, TTranslation, TDestination> MapMultiLingual<TSource,
            TTranslation, TDestination>(
            this IMappingExpression<TSource, TDestination> mappingExpression,
            Profile profile,
            ISettingProvider provider,
            bool fallbackToParentCultures = false)
            where TDestination : class
            where TSource : IHasMultiLingual<TTranslation>
            where TTranslation : class, IMultiLingualTranslation
        {
            return new CreateMultiLingualMapResult<TSource, TTranslation, TDestination>
            {
                TranslationMap = profile.CreateMap<TTranslation, TDestination>(),

                EntityMap = mappingExpression.BeforeMap((source, destination, context) =>
                {
                    var translation = source.GetMultiLingualTranslation(provider, fallbackToParentCultures);

                    if (translation != null)
                    {
                        context.Mapper.Map(translation, destination);
                    }
                })
            };
        }
    }

    public class CreateMultiLingualMapResult<TMultiLingual, TTranslation, TDestination>
    {
        public IMappingExpression<TTranslation, TDestination> TranslationMap { get; set; }

        public IMappingExpression<TMultiLingual, TDestination> EntityMap { get; set; }
    }
}
