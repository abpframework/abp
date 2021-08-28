using System.Globalization;
using Volo.Abp.AutoMapper;
using Volo.Abp.MultiLingualObjects;


namespace AutoMapper
{
    public static class AbpAutoMapperMultiLingualDtoExtensions
    {
        public static CreateMultiLingualMapResult<TMultiLingualObject, TTranslation, TDestination>
            CreateMultiLingualMap<TMultiLingualObject, TTranslation, TDestination>(this Profile profile)
            where TTranslation : class, IObjectTranslation
            where TMultiLingualObject : IMultiLingualObject
        {
            return new(
                profile.CreateMap<TMultiLingualObject, TDestination>()
                    .AfterMap<AbpMultiLingualMapperAction<TMultiLingualObject, TTranslation, TDestination>>(),
                profile.CreateMap<TTranslation, TDestination>());
        }

        public static IMappingExpression<TMultiLingualObject, TDestination> IgnoreTranslations<TMultiLingualObject, TDestination>(
            this IMappingExpression<TMultiLingualObject, TDestination> mappingExpression)
            where TDestination : IMultiLingualObject
            where TMultiLingualObject : IMultiLingualObject
        {
            return mappingExpression.Ignore(x => x.DefaultCulture).Ignore(x => x.Translations);
        }
    }

    public class AbpMultiLingualMapperAction<TMultiLingualObject, TTranslation, TDestination>
        : IMappingAction<TMultiLingualObject, TDestination>
        where TTranslation : class, IObjectTranslation
        where TMultiLingualObject : IMultiLingualObject
    {
        private readonly IMultiLingualObjectManager _multiLingualObjectManager;

        public AbpMultiLingualMapperAction(IMultiLingualObjectManager multiLingualObjectManager)
        {
            _multiLingualObjectManager = multiLingualObjectManager;
        }

        public void Process(TMultiLingualObject source, TDestination destination, ResolutionContext context)
        {
            var translation =
                _multiLingualObjectManager.GetTranslation<TMultiLingualObject, TTranslation>(source);

            if (translation != null)
            {
                context.Mapper.Map(translation, destination);
            }
        }
    }

    public class CreateMultiLingualMapResult<TMultiLingualObject, TTranslation, TDestination>
    {
        public IMappingExpression<TMultiLingualObject, TDestination> EntityMap { get; }

        public IMappingExpression<TTranslation, TDestination> TranslateMap { get; }

        public CreateMultiLingualMapResult(
            IMappingExpression<TMultiLingualObject, TDestination> entityMap,
            IMappingExpression<TTranslation, TDestination> translateMap)
        {
            EntityMap = entityMap;
            TranslateMap = translateMap;
        }
    }
}